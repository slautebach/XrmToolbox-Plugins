using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace slautebach.MigrateUserSettings
{
    internal class UserDashboard : EntityAndAccess
    {

        public UserDashboard() : base()
        {
        }
        public UserDashboard(Entity userDashboard)
        {
            if (userDashboard.LogicalName != "userform")
            {
                throw new Exception("Entity is not a userform");
            }
        }


        public string FormXml
        {
            get
            {
                return Entity["formxml"].ToString();
            }
            set
            {
                Entity["formxml"] = value;
            }
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

        public override MigratableReason CleanEntity()
        {
            // Update the migration mapping
            var sourceOwningBusinessUnit = Entity["owningbusinessunit"] as EntityReference;
            var sourceOwner = Entity["ownerid"] as EntityReference;

            // Find the mapping for the owner
            var owningBusinessUnitMapping = MigrationConfig.Instance.GetMappedOwner(sourceOwningBusinessUnit);
            Entity["owningbusinessunit"] = owningBusinessUnitMapping;

            var ownerMapping = MigrationConfig.Instance.GetMappedOwner(sourceOwner);
            if (ownerMapping == null)
            {
                return MigratableReason.UnableToMigrate($"Unable to map the Owner for view: {Name}");
            }
            Entity["ownerid"] = ownerMapping;

            var modified = false;
            var formxml = XDocument.Parse(FormXml);

            var listOfAllEntities = MigrationConfig.Instance.TargetEntitiesMetadata.Select(e => e.LogicalName.ToLower()).OrderBy(e => e).ToList();

            var targetViews = MigrationConfig.Instance.GetTargetAllTargetViews().Select(v => v.Id).ToList();

            var targetEntityTypes = formxml.Descendants("TargetEntityType").ToList();
            foreach (var targetEntityType in targetEntityTypes)
            {
                if (listOfAllEntities.Contains(targetEntityType.Value))
                {
                    // the target entity is in the target system, we can leave the control in the dashboard
                    continue;
                }
                // the target entity is not in the target system, so we must remove it from the dashboard

                // Navigate to the anncestor cell
                // TargetEntityType : parameters -> control -> cell
                var ancestorCell = targetEntityType?.Parent?.Parent?.Parent;

                System.Diagnostics.Debug.Assert(ancestorCell != null, "Anncestor 'cell' node is null");
                System.Diagnostics.Debug.Assert(ancestorCell.Name.ToString() == "cell", "Cannot find Anncestor 'cell' node");

                var label = ancestorCell.Descendants("label").FirstOrDefault()?.Attribute("description")?.Value ?? "";
                Console.WriteLine($"    Removing Cell '{label}' from the dashboard, because the target entity: '{targetEntityType.Value}' doesn't exist in the target environment");

                modified = true;
                // Remove the ancestor cell.
                ancestorCell.Remove();
            }


            var viewIds = formxml.Descendants("ViewId").ToList();
            foreach (var viewId in viewIds)
            {
                if (string.IsNullOrEmpty(viewId?.Value))
                {
                    // not a view
                    continue;
                }
                Guid id = Guid.Parse(viewId.Value);
                if (targetViews.Contains(id))
                {
                    // target environment conatins the view
                    continue;
                }


                // Navigate to the anncestor cell
                // ViewId : parameters -> control -> cell
                var ancestorCell = viewId?.Parent?.Parent?.Parent;

                System.Diagnostics.Debug.Assert(ancestorCell != null, "Anncestor 'cell' node is null");
                System.Diagnostics.Debug.Assert(ancestorCell.Name.ToString() == "cell", "Cannot find Anncestor 'cell' node");


                var label = ancestorCell.Descendants("label").FirstOrDefault()?.Attribute("description")?.Value ?? "";
                Console.WriteLine($"    Removing Cell '{label}' from the dashboard, because the view {id} doesn't exist in the target environment");

                modified = true;
                // Remove the ancestor cell.
                ancestorCell.Remove();
            }

            var numberCellsLeft = formxml.Descendants("cell").ToList().Count;

            if (numberCellsLeft == 0)
            {
                return MigratableReason.UnableToMigrate($"Dashboard '{Name}' has no cells left after cleanup, not migrating."); ;
            }
            if (modified)
            {
                Name += MigrationConfig.Instance.ModificationString;
            }
            FormXml = formxml.ToString();
            return MigratableReason.CanMigrate;
        }


    }
}
