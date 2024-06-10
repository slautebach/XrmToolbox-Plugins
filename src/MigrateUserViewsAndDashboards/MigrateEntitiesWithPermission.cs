using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Messages;
using System.IdentityModel.Metadata;
using System.Xml.Linq;
using XrmToolBox.Extensibility;

namespace slautebach.MigrateUserSettings
{
    /// <summary>
    /// Generic class that will load all the migration dependencies related to the Entity to migratge
    /// and preform the migration in a generic way.
    /// </summary>
    /// <typeparam name="TEntityAccess"></typeparam>
    internal class MigrateEntitiesWithPermission<TEntityAccess> where TEntityAccess : EntityAndAccess, new()
    {

        LogManager logManager = null;
        MigrationConfig migrationConfig = null;
        /// <summary>
        /// List of Source Entities to migrate
        /// </summary>
        public List<EntityAndAccess> SourceEntities { get; set; }


        /// <summary>
        /// List of entity principal access shares to mirgrate
        /// </summary>
        public List<Entity> EntityShares { get; set; }

        /// <summary>
        /// Enitity logical name to migatee
        /// </summary>
        protected string EntityLogicalName { get; set; }

        /// <summary>
        /// Attribute Logical Name of the entities ID attribute
        /// </summary>
        protected string EntityIdLogicalName { get; set; }

        /// <summary>
        /// Attribute Logical Name of the entities Name attribute
        /// </summary>
        protected string EntityNameLogicalName { get; set; }


        /// <summary>
        /// Used for testing/debugging set to True
        /// then it will attempt to pre delete all 
        /// instances of the source entities that 
        /// may exist in the target environment.
        /// Note: this is a time consuming process,
        ///     and you only need to run it if the target data needs to be cleared.
        /// </summary>
        public bool PreDeleteFromTarget { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="entityLogicalName"></param>
        public MigrateEntitiesWithPermission(string entityLogicalName, MigrationConfig migrationConfig, LogManager logManager)
        {
            this.migrationConfig = migrationConfig;
            this.logManager = logManager;
            PreDeleteFromTarget = false;
            EntityLogicalName = entityLogicalName;
            EntityIdLogicalName = $"{EntityLogicalName}id";
            EntityNameLogicalName = "name";
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="entityLogicalName"></param>
        /// <param name="entityIdLogicalName"></param>
        /// <param name="entityNameLogicalName"></param>
        public MigrateEntitiesWithPermission(string entityLogicalName, string entityIdLogicalName, string entityNameLogicalName)
        {
            PreDeleteFromTarget = false;
            EntityLogicalName = entityLogicalName;
            EntityIdLogicalName = entityIdLogicalName;
            EntityNameLogicalName = entityIdLogicalName;
        }

        /// <summary>
        /// Get the entities to migrate from the source
        /// </summary>
        /// <exception cref="Exception"></exception>
        protected void GetSourceEntities()
        {
            if (SourceEntities != null)
            {
                return;
            }
            // get a list of source users
            var sourceUsers = migrationConfig.GetSourceUsers();

            // Dictionary of the retrieved entities by id.
            Dictionary<Guid, EntityAndAccess> entitiesById = new Dictionary<Guid, EntityAndAccess>();

            QueryExpression q = new QueryExpression(EntityLogicalName);
            q.ColumnSet = new ColumnSet(true);

            logManager.LogInfo($"Retrieving All {EntityLogicalName} from the source system as each user");

            int count = 0;
            // for each user id query the entities that the user can see
            foreach (var user in sourceUsers)
            {
                count++;
                migrationConfig.SourceCrm.CallerId = user.Id;
                logManager.LogInfo($"   Retrieving ({count}/{sourceUsers.Count}) {user["internalemailaddress"]} user {EntityLogicalName}.");
                var entities = migrationConfig.SourceCrm.RetrieveMultiple(q);
                int totalCount = entities.Entities.Count;
                if (totalCount >= 5000)
                {
                    throw new Exception($"There are {totalCount} user {EntityLogicalName}, more than 5000, code needs to be updated to support more than 5000.");
                }
                logManager.LogInfo($"        User has {totalCount} views to migrate.");
                foreach (var entity in entities.Entities)
                {
                    if (!entitiesById.ContainsKey(entity.Id))
                    {
                        // add or update the view the user can see.
                        entitiesById[entity.Id] = new TEntityAccess();
                        entitiesById[entity.Id].Entity = entity;
                    }
                }
            }
            migrationConfig.SourceCrm.CallerId = Guid.Empty;

            // set the source entities to all the retrieved eneties.
            SourceEntities = entitiesById.Values.ToList();
        }


        /// <summary>
        /// Get the shares of the user view
        /// </summary>
        protected void GetEntityPrincipalAccess()
        {
            if (EntityShares != null)
            {
                return;
            }
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
  <entity name='principalobjectaccess'>
    <attribute name='objectid' />
    <attribute name='principaltypecode' />
    <attribute name='accessrightsmask' />
    <attribute name='inheritedaccessrightsmask' />
    <attribute name='principalobjectaccessid' />
    <attribute name='objecttypecode' />
    <attribute name='principalid' />
    <link-entity name='{EntityLogicalName}' from='{EntityIdLogicalName}' to='objectid' link-type='inner' />
  </entity>
</fetch>";
            var sourceUsers = migrationConfig.GetSourceUsers();


            Dictionary<Guid, Entity> entitiesById = new Dictionary<Guid, Entity>();

            logManager.LogInfo($"");
            logManager.LogInfo($"Retrieving All principalobjectaccess ralted to Entity: {EntityLogicalName} from the source system as each user");
            int count = 0;
            // for each user id query the views that the user can see
            foreach (var user in sourceUsers)
            {
                count++;
                migrationConfig.SourceCrm.CallerId = user.Id;
                logManager.LogInfo($"   Retrieving ({count}/{sourceUsers.Count}) {user["internalemailaddress"]} user {EntityLogicalName} principalobjectaccess.");
                var entities = migrationConfig.SourceCrm.RetrieveMultiple(new FetchExpression(fetchXml));
                if (entities.MoreRecords)
                {
                    throw new Exception($"There are {entities.TotalRecordCount} user {EntityLogicalName}, more than {entities.Entities.Count}, code needs to be updated to support more than 5000.");
                }
                foreach (var entity in entities.Entities)
                {
                    if (!entitiesById.ContainsKey(entity.Id))
                    {
                        entitiesById[entity.Id] = entity;
                    }
                }
            }
            EntityShares = entitiesById.Values.ToList();

            migrationConfig.SourceCrm.CallerId = Guid.Empty;
        }


        /// <summary>
        /// Try to delete the record as all users, as the admin user doesn't have permission.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        internal bool DeleteTargetEntityAsUsers(EntityReference e)
        {
            var targetDataverse = migrationConfig.TargetDataverse;

            var targetUsers = migrationConfig.GetTargetUsers();

            bool deleteSuccessful = false;
            foreach (var user in targetUsers)
            {
                targetDataverse.CallerId = user.Id;
                try
                {
                    targetDataverse.Delete(e.LogicalName, e.Id);
                    deleteSuccessful = true;
                    break;
                }
                catch (Exception)
                {
                    // retry as a different user.
                }
            }
            targetDataverse.CallerId = Guid.Empty;
            return deleteSuccessful;

        }


        internal void DeleteAllTargetEntityAsUsers(List<EntityReference> entityReferences)
        {
            var targetDataverse = migrationConfig.TargetDataverse;

            var targetUsers = migrationConfig.GetTargetUsers();

            var deleteRequests = entityReferences.Select(er => new DeleteRequest()
            {
                Target = er
            } as OrganizationRequest).ToList();


            logManager.LogInfo($"Attempted to delete {entityReferences.Count} records ...");
            int usercount = 0;
            foreach (var user in targetUsers)
            {
                usercount++;
                targetDataverse.CallerId = user.Id;

                if (!deleteRequests.Any())
                {
                    // no more to delete break
                    break;
                }
                logManager.LogInfo($"   ({usercount}/{targetUsers.Count}) Attempting to delete {deleteRequests.Count} records as: {user["internalemailaddress"]}");
                var originalDeleteRequestList = deleteRequests.ToList();
                // try to delete all request
                var bulkDeleteResponse = migrationConfig.ExecuteMultipleOnTarget(deleteRequests);
                foreach (var response in bulkDeleteResponse.Responses)
                {
                    var deleteRequest = originalDeleteRequestList.ElementAt(response.RequestIndex);
                    var deleteResponse = response.Response as DeleteResponse;
                    if (response.Fault == null)
                    {
                        // request was successful, remove it from the list to try as other uers
                        deleteRequests.Remove(deleteRequest);
                        continue;
                    }
                }

            }
            targetDataverse.CallerId = Guid.Empty;
            int deleteCount = entityReferences.Count - deleteRequests.Count;
            logManager.LogInfo($"Attempted to delete {entityReferences.Count} {EntityLogicalName}, was able to successfully delete {deleteCount}, any records not able to be deleted is assumed to not have existed.");

        }

        /// <summary>
        /// Preform the migration of the source entities to the target
        /// </summary>
        internal void MigrateEntities()
        {
            var targetDataverse = migrationConfig.TargetDataverse;

            GetSourceEntities();

            // clear impersonation.
            targetDataverse.CallerId = Guid.Empty;
            logManager.LogInfo($"");
            logManager.LogInfo($"Migrating Records for {EntityLogicalName}");

            List<OrganizationRequest> requestBatchQueue = new List<OrganizationRequest>();

            List<Entity> migratableEntites = new List<Entity>();
            foreach (var entity in SourceEntities)
            {
                logManager.LogInfo($"   Cleaning {EntityLogicalName}: {entity.Name}");

                // clean the source entity and determine if it is migratable
                var canMigrateEntity = entity.CleanEntity();

                // cannot migrate
                if (!canMigrateEntity.Migratable)
                {
                    logManager.LogInfo($"   Unable to migrate {entity.Name}, skipping.");
                    logManager.LogInfo($"      Reason: {canMigrateEntity.Reason}");

                    // entity cannot be migrated, so it will be dropped.
                    continue;
                }

                // add to the list of migratable entities
                var upsertEntity = entity.GetMigrationEntity();
                migratableEntites.Add(upsertEntity);

            }


            if (PreDeleteFromTarget)
            {
                var entitiesToDelete = migratableEntites.Select(e => e.ToEntityReference()).ToList();
                DeleteAllTargetEntityAsUsers(entitiesToDelete);
            }

            List<EntityAndAccess> migratedEntites = new List<EntityAndAccess>();
            int totalCount = migratableEntites.Count;
            int migratedCount = 0;

            foreach (var target in migratableEntites)
            {
                UpsertRequest upsertRequest = new UpsertRequest()
                {
                    Target = target
                };
                migratedCount++;
                var sourceEntityAndAccess = SourceEntities.FirstOrDefault(e => e.Id == target.Id);
                logManager.LogInfo($"   ({migratedCount}/{totalCount}) Upserting {EntityLogicalName}: {sourceEntityAndAccess.Name}.");
                UpsertResponse upsertResponse;
                try
                {
                    upsertResponse = targetDataverse.Execute(upsertRequest) as UpsertResponse;
                }
                catch (Exception ex)
                {
                    logManager.LogInfo($"      Unable to Upsert, skipping");
                    logManager.LogInfo($"         Error: {ex.Message}");
                    continue;
                }
                // update the target id.
                sourceEntityAndAccess.Id = upsertResponse.Target.Id;
                migratedEntites.Add(sourceEntityAndAccess);
                logManager.LogInfo($"   Created {EntityLogicalName} {sourceEntityAndAccess.Name} in target.");
            }

            logManager.LogInfo($"Migrated {migratedCount} of {totalCount}");
            // once all entities have been migrated, we then need
            // to migrate the ownership and shares of them.
            MigrateEntityPrincipalAcessOwnerShip(migratedEntites);
        }



        /// <summary>
        /// Migrates the shares and ownership on the migrated entities.
        /// </summary>
        /// <param name="migratedEntites"></param>
        protected void MigrateEntityPrincipalAcessOwnerShip(List<EntityAndAccess> migratedEntites)
        {
            var targetDataverse = migrationConfig.TargetDataverse;

            targetDataverse.CallerId = Guid.Empty;

            var whoami = targetDataverse.Execute(new WhoAmIRequest()) as WhoAmIResponse;

            // gets/loads all the shares
            GetEntityPrincipalAccess();

            logManager.LogInfo($"");
            logManager.LogInfo($"");
            logManager.LogInfo($"Migrating Princiapal Access and Ownership on a a total of: {migratedEntites.Count} {EntityLogicalName}");
            targetDataverse.CallerId = Guid.Empty;
            int count = 0;

            // for each migrated entity, we are going to build up a collection 
            // of grant access request to execute them at once. 
            foreach (var entity in migratedEntites)
            {
                count++;

                logManager.LogInfo($"   ({count}/{migratedEntites.Count}) Sharing {EntityLogicalName} Record: {entity.Name}");
                List<OrganizationRequest> requests = new List<OrganizationRequest>();

                // Grant all access to the user of this migration process
                // this will prevent errors if the process needs to re-run.
                GrantAccessRequest grantAccessRequest = new GrantAccessRequest()
                {
                    PrincipalAccess = new PrincipalAccess()
                    {
                        AccessMask = AccessRights.DeleteAccess | AccessRights.ReadAccess | AccessRights.ShareAccess | AccessRights.WriteAccess | AccessRights.AssignAccess,
                        Principal = new EntityReference("systemuser", whoami.UserId)
                    },
                    Target = entity.Entity.ToEntityReference()
                };

                logManager.LogInfo($"      With: Admin");
                requests.Add(grantAccessRequest);

                // Find all the shares reference the current entity.
                var entitySharesFiltered = EntityShares.Where(e => ((Guid)e["objectid"]) == entity.Id).ToList();

                // for each share, create and add a grant access request copying it.
                foreach (var entityShare in entitySharesFiltered)
                {
                    var sourcePrincipalId = (Guid)entityShare["principalid"];
                    var sourcePrincipalLogicalName = entityShare["principaltypecode"].ToString();
                    var targetPrincipal = migrationConfig.GetMappedOwner(sourcePrincipalLogicalName, sourcePrincipalId);
                    var accessMask = (int)entityShare["accessrightsmask"];
                    grantAccessRequest = new GrantAccessRequest()
                    {
                        PrincipalAccess = new PrincipalAccess()
                        {
                            AccessMask = (AccessRights)accessMask,
                            Principal = targetPrincipal
                        },
                        Target = entity.Entity.ToEntityReference()
                    };

                    logManager.LogInfo($"      With: {targetPrincipal.LogicalName} - {targetPrincipal.Name} ({targetPrincipal.Id})");
                    requests.Add(grantAccessRequest);
                }

                try
                {
                    // for the entity execute all grant access requests.
                    logManager.LogInfo($"   Sharing {EntityLogicalName} '{entity.Name}' with {requests.Count} Users/Teams");
                    ExecuteMultipleResponse multipleResponse = migrationConfig.ExecuteMultipleOnTarget(requests);

                    // If there is a fault, log it so it can be debugged.
                    if (multipleResponse.IsFaulted)
                    {
                        logManager.LogInfo("*** Error enabling some shares, investigate and re-run");

                        var failedResponses = multipleResponse.Responses.Where(r => r.Fault != null).ToList();
                        failedResponses.ForEach(response =>
                        {
                            var failedRequest = requests.ElementAt(response.RequestIndex) as GrantAccessRequest;

                            logManager.LogInfo($"   **************************************");
                            logManager.LogInfo($"   Failed to share with: {failedRequest.PrincipalAccess.Principal.Name}");
                            logManager.LogInfo($"   Error Message: {response.Fault.Message}");
                            logManager.LogInfo($"   TraceText: {response.Fault.TraceText}");
                            logManager.LogInfo($"   Inner Fault Message: {response.Fault?.InnerFault.Message ?? ""}");
                            logManager.LogInfo($"   **************************************");
                        });
                        logManager.LogInfo("*** Skipping share with the above errors.");
                    }

                }
                catch (Exception ex)
                {
                    logManager.LogInfo($"*** Unable to {EntityLogicalName} view with teams/users.");
                    logManager.LogInfo($"*** Error running multiple request to share: {ex.Message}");
                }
             
                // if the entity owner was mapped
                // assign the entity to the target
                // environment mapped owner
                if (entity.Owner != null)
                {
                    // Assign the record
                    AssignRequest assignRequest = new AssignRequest()
                    {
                        Assignee = entity.Owner,
                        Target = entity.Entity.ToEntityReference()
                    };

                    logManager.LogInfo($"   Assigning {EntityLogicalName} to Owner: {entity.Owner.LogicalName} - {entity.Owner.Name} ({entity.Owner.Id})");
                    var assignResponse = targetDataverse.Execute(assignRequest) as AssignResponse;

                    entity.AssignedToOwner = entity.Owner.Name;
                }
                else
                {
                    var ownerMapping = migrationConfig.GetMapping(entity.SourceOwner);
                    var userName = "unknown";
                    if (ownerMapping != null && ownerMapping.Contains("rp_name"))
                    {
                        userName = ownerMapping["rp_name"].ToString();
                    }
                    logManager.LogInfo($"   Source Owner for {entity.Name} is not in target environment, not setting owner {userName}.");
                }
            }

            logManager.LogInfo($"");
            logManager.LogInfo($"");
            logManager.LogInfo($"************************************************");
            logManager.LogInfo($" Output of successful ownership mapping");
            logManager.LogInfo($"***********************************************************");
            foreach (var entity in migratedEntites.OrderBy(o => o.AssignedToOwner).ToList())
            {
                if (entity.AssignedToOwner == null)
                {
                    logManager.LogInfo($"**** Unable to assign owner to {entity.Name} {EntityLogicalName} id: ({entity.Id})");
                }
                else
                {
                    string owner =  $"{entity.AssignedToOwner} ({entity.Owner.Id})".PadRight(100); 
                    logManager.LogInfo($"   Owner: {owner} : {entity.Name}");
                }
            }
        }
    }
}
