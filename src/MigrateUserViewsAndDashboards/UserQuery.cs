using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace slautebach.MigrateUserSettings
{
    internal class UserQuery : EntityAndAccess
    {
        public UserQuery() : base()
        {
        }
        public UserQuery(Entity viewEntity) : base(viewEntity)
        {
            if (viewEntity.LogicalName != "userquery")
            {
                throw new Exception("Entity is not a userquery");
            }
            Entity.Attributes = viewEntity.Attributes;
            this.Id = viewEntity.Id;
        }


        public override string Name
        {
            get
            {
                return Entity["name"].ToString();
            }
            set
            {
                Entity["name"] = value;
            }
        }
        public string FetchXml
        {
            get
            {
                return Entity["fetchxml"].ToString();
            }
            set
            {
                Entity["fetchxml"] = value;
            }
        }

        public string LayoutXml
        {
            get
            {
                return Entity["layoutxml"].ToString();
            }
            set
            {
                Entity["layoutxml"] = value;
            }
        }

        public override MigratableReason CleanEntity()
        {



            var fetchxml = XDocument.Parse(FetchXml);
            var layoutXml = XDocument.Parse(LayoutXml);

            var listOfAllEntities = MigrationConfig.Instance.TargetEntitiesMetadata.Select(e => e.LogicalName).OrderBy(e => e).ToList();

            var entityNode = fetchxml.XPathSelectElement("/fetch/entity");
            string fetchEntityLogicalName = entityNode.Attribute("name").Value;

            if (!listOfAllEntities.Contains(fetchEntityLogicalName))
            {
                return MigratableReason.UnableToMigrate($"Target Enviornment does not define the entity: {fetchEntityLogicalName}");
            }

            bool modified = false;

            var linkEntities = fetchxml.Descendants("link-entity").ToList();
            Dictionary<string, string> linkedEntityMappings = new Dictionary<string, string>();
            foreach (var linkEntity in linkEntities.ToList())
            {
                var fetchLinkEntityLogicalName = linkEntity.Attribute("name").Value;

                var fetchAliasName = linkEntity.Attribute("alias")?.Value;
                if (fetchAliasName == null)
                {
                    continue;
                }

                linkedEntityMappings[fetchAliasName] = fetchLinkEntityLogicalName;

                string parentEntityLogicalName = linkEntity.Parent.Attribute("name")?.Value;

                string from = linkEntity.Attribute("from")?.Value;
                string to = linkEntity.Attribute("to")?.Value;

                // if the linked entity is not in the target system remove it.
                if (!listOfAllEntities.Contains(fetchLinkEntityLogicalName))
                {
                    // entity is not in the migration list
                    // remove the link entity
                    linkEntity.Remove();
                    modified = true;
                    continue;
                }

                // if the link entity does not contain the from field
                // remove the link.
                if (!MigrationConfig.Instance.DoesAttributeExistInTarget(fetchLinkEntityLogicalName, from))
                {
                    // entity is not in the migration list
                    // remove the link entity
                    linkEntity.Remove();
                    modified = true;
                    continue;
                }


                // if the link entity does not contain the to field
                // remove the link.
                if (!MigrationConfig.Instance.DoesAttributeExistInTarget(parentEntityLogicalName, to))
                {
                    // entity is not in the migration list
                    // remove the link entity
                    linkEntity.Remove();
                    modified = true;
                    continue;
                }

              
            }

            var attributes = fetchxml.Descendants("attribute")?.ToList();
            if (attributes != null)
            {
                foreach (var attrib in attributes.ToList())
                {
                    var attribEntityLogicalName = attrib.Parent.Attribute("name")?.Value;
                    var attribLogicalName = attrib.Attribute("name")?.Value;
                    if (!MigrationConfig.Instance.DoesAttributeExistInTarget(attribEntityLogicalName, attribLogicalName))
                    {
                        attrib.Remove();
                        modified = true;
                    }
                }
            }

            var orders = fetchxml.Descendants("order")?.ToList();
            if (orders != null)
            {
                foreach (var order in orders.ToList())
                {
                    var attribEntityLogicalName = order.Parent.Attribute("name")?.Value;
                    var attribLogicalName = order.Attribute("attribute")?.Value;
                    if (!MigrationConfig.Instance.DoesAttributeExistInTarget(attribEntityLogicalName, attribLogicalName))
                    {
                        order.Remove();
                        modified = true;
                    }
                }
            }

            var conditions = fetchxml.Descendants("condition")?.ToList();
            if (conditions != null)
            {
                foreach (var condition in conditions.ToList())
                {
                    var parent = FindParentEntity(condition);
                    if (parent == null)
                    {
                        continue;
                    }
                    var entityLogicalName = parent.Attribute("name")?.Value;
                    var attribLogicalName = condition.Attribute("attribute")?.Value;
                    if (!MigrationConfig.Instance.DoesAttributeExistInTarget(entityLogicalName, attribLogicalName))
                    {
                        condition.Remove();
                        modified = true;
                    }
                }
            }

            var layoutCells = layoutXml.Descendants("cell").ToList();
            foreach (var layoutCell in layoutCells.ToList())
            {
                var cellName = layoutCell.Attribute("name").Value;

                // if the cell name is aliased
                if (cellName.Contains("."))
                {
                    var parts = cellName.Split('.');
                    var alias = parts[0];
                    var attribLogicalName = parts[1];

                    var aliasEntityLogicalName = linkedEntityMappings[alias];

                    if (!MigrationConfig.Instance.DoesAttributeExistInTarget(aliasEntityLogicalName, attribLogicalName))
                    {
                        layoutCell.Remove();
                        modified = true;
                    }
                }
                else
                {
                    if (!MigrationConfig.Instance.DoesAttributeExistInTarget(fetchEntityLogicalName, cellName))
                    {
                        layoutCell.Remove();
                        modified = true;
                    }
                }
            }

            // Update the xml
            FetchXml = fetchxml.ToString();
            LayoutXml = layoutXml.ToString();


            // Update the migration mapping
            EntityReference sourceOwningBusinessUnit = null;
            if (Entity.Contains("owningbusinessunit"))
            {
                sourceOwningBusinessUnit = Entity["owningbusinessunit"] as EntityReference;
            }


            EntityReference sourceOwner = null;
            if (Entity.Contains("ownerid"))
            {
                sourceOwner = Entity["ownerid"] as EntityReference;
            }

            SourceOwner = sourceOwner;

            // Find the mapping for the owner
            var owningBusinessUnitMapping = MigrationConfig.Instance.GetMappedOwner(sourceOwningBusinessUnit);
            if (owningBusinessUnitMapping == null)
            {
                // no business unit, don't set or clear
                Entity.Attributes.Remove("owningbusinessunit");
            }
            else
            {
                Entity["owningbusinessunit"] = owningBusinessUnitMapping;
            }

            var ownerMapping = MigrationConfig.Instance.GetMappedOwner(sourceOwner);

            if (ownerMapping == null)
            {
                return MigratableReason.UnableToMigrate($"Unable to map the Owner for view: {Name}");
            }
            Entity["ownerid"] = ownerMapping;


            var createdBy = Entity["createdby"] as EntityReference;
            Entity["createdby"] = MigrationConfig.Instance.GetMappedOwner(createdBy);

            var modifiedBy = Entity["modifiedby"] as EntityReference;
            Entity["modifiedby"] = MigrationConfig.Instance.GetMappedOwner(modifiedBy);

            foreach (var attrib in Entity.Attributes.ToList())
            {
                // remove all null values
                if (attrib.Value == null)
                {
                    Entity.Attributes.Remove(attrib.Key);
                }
            }

            if (modified)
            {
                Name += MigrationConfig.Instance.ModificationString;
            }
            return MigratableReason.CanMigrate;
        }

        private XElement FindParentEntity(XElement node)
        {
            XElement currentNode = node;
            while (currentNode.Name != "link-entity" && currentNode.Name != "entity" && currentNode != null)
            {
                currentNode = currentNode.Parent;
            }
            return currentNode;
        }

    }
}
