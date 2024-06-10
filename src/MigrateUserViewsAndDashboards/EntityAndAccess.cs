using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slautebach.MigrateUserSettings
{
    /// <summary>
    /// An abstract class used to represent a migratable entity
    /// And common properties/interface to access the underlying entity
    /// </summary>
    internal abstract class EntityAndAccess
    {

        public EntityAndAccess()
        {
            Entity = new Entity();
            AssignedToOwner = null;
        }
        public EntityAndAccess(Entity e)
        {
            Entity = e;
        }
        /// <summary>
        /// Id of the underlying entity
        /// </summary>
        public Guid Id
        {
            get
            {
                return Entity.Id;
            }
            set
            {
                Entity.Id = value;
            }
        }


        /// <summary>
        /// The Entity
        /// </summary>
        public Entity Entity { get; set; }


        /// <summary>
        /// The name of the entity
        /// </summary>
        public abstract string Name { get; set; }


        /// <summary>
        /// The owner of the entity.
        /// </summary>
        public virtual EntityReference Owner
        {
            get
            {
                return Entity["ownerid"] as EntityReference;
            }
        }

        public string AssignedToOwner { get; set; }

        public virtual EntityReference SourceOwner
        {
            get; set;
        }

        /// <summary>
        /// create a new instance of an entity with only the attributes we want to upsert
        /// </summary>
        /// <returns></returns>
        public virtual Entity GetMigrationEntity()
        {
            Entity entity = new Entity(Entity.LogicalName);
            entity.Id = Id;
            foreach (var attrib in Entity.Attributes)
            {
                if (attrib.Value == null)
                {
                    // if null don't copy the attribute
                    continue;
                }
                entity.Attributes[attrib.Key] = attrib.Value;
            }

            if (entity.Contains("ownerid"))
            {
                // remove owner id
                entity.Attributes.Remove("ownerid");
            }

            return entity;
        }


        /// <summary>
        /// Cleans the entity data, so it can be migrated into the target system.
        /// </summary>
        /// <returns></returns>
        public abstract MigratableReason CleanEntity();


      
    }
}
