using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Tooling.Connector;
using Microsoft.Crm.Sdk.Messages;

using Microsoft.Xrm.Sdk.Messages;
using System.Data.Services.Client;
using System.IdentityModel.Metadata;
using XrmToolBox.Extensibility;

namespace slautebach.MigrateUserSettings
{
    internal partial class MigrationConfig
    {
        LogManager logManager = null;

        public RunOptions RunOptions = null;
        public CrmServiceClient SourceCrm { get; set; }
        public CrmServiceClient TargetDataverse { get; set; }

        public List<Entity> TargetViewsAll { get; set; }

        public List<UserDashboard> UserDashboards { get; set; }

        public List<EntityMetadata> TargetEntitiesMetadata { get; set; }
        public List<string> ListOfAllEntities { get; set; }
        public List<Entity> MigrationMappings { get; set; }
        public List<Entity> ViewShares { get; set; }
        public List<Entity> DashboardShares { get; set; }


        public List<Entity> SourceUsers { get; set; }
        public List<Entity> TargetUsers { get; set; }
        public MigrationConfig(LogManager logManager)
        {
            this.logManager = logManager;
        }

        public string ModificationString
        {
            get
            {
                return " (*)";
            }
        }

        public bool DoesAttributeExistInTarget(string entityLogicalName, string attributeLogicalName)
        {
            var entityMd = TargetEntitiesMetadata.FirstOrDefault(emd => emd.LogicalName == entityLogicalName);
            if (entityMd == null)
            {
                return false;
            }

            if (!entityMd.Attributes.Any(amd => amd.LogicalName == attributeLogicalName))
            {
                return false;
            }
            return true;
        }


        public void GetMigrationMapping()
        {
            Console.WriteLine("Retrieving Migration Ownership Mapping");
            QueryExpression q = new QueryExpression("rp_migrationmapping");
            q.ColumnSet = new ColumnSet(true);
            q.Criteria = new FilterExpression();
            // filter only to the active mappings
            q.Criteria.AddCondition(new ConditionExpression("statecode", ConditionOperator.Equal, 0));

            var entities = TargetDataverse.RetrieveMultiple(q);

            if (entities.TotalRecordCount >= 5000)
            {
                throw new Exception($"There are {entities.TotalRecordCount} user saved views, more than 5000, code needs to be updated to support more than 5000.");
            }
            MigrationMappings = entities.Entities.ToList();
        }


        public void GetTargetEntityMetadata()
        {

            // Retrieve the list of entities, to make sure they match up to what is in the spreadsheet.
            Console.WriteLine($"Retrieving a list of all Entities Metadata from the target");
            RetrieveAllEntitiesRequest retreiveAllEntities = new RetrieveAllEntitiesRequest()
            {
                EntityFilters = Microsoft.Xrm.Sdk.Metadata.EntityFilters.Entity | Microsoft.Xrm.Sdk.Metadata.EntityFilters.Attributes
            };
            RetrieveAllEntitiesResponse retreiveAllEntityResponse = (RetrieveAllEntitiesResponse)TargetDataverse.Execute(retreiveAllEntities);
            TargetEntitiesMetadata = retreiveAllEntityResponse.EntityMetadata.ToList();
            ListOfAllEntities = TargetEntitiesMetadata.Select(e => e.LogicalName).OrderBy(e => e).ToList();

        }

        /// <summary>
        /// On parsing of the options, it sets them
        /// </summary>
        /// <param name="opts"></param>
        void SetRunOptions(RunOptions opts)
        {
            RunOptions = opts;
        }


        public void Initialize()
        {
            Console.Write("Command Line Args: ");
            Console.WriteLine();

            ////Parse command line opions
            //CommandLine.Parser.Default.ParseArguments<RunOptions>(args)
            //    .WithParsed(SetRunOptions)
            //    .WithNotParsed(HandleParseError);

            // if we were unable to parse them exist (the help is outputted)
            if (RunOptions == null)
            {
                RunOptions = new RunOptions();
            }


            RunOptions.LogDetails();

            // Log the connection details
            string targetConnectionString = $"AuthType=ClientSecret; Url=https://{RunOptions.TargetEnv}-conv.crm3.dynamics.com/;ClientId={RunOptions.ClientId}; ClientSecret={RunOptions.ClientSecret};Timeout=00:30:00";
            string targetConnectionStringLoggable = targetConnectionString.Replace(RunOptions.ClientSecret, "**************************");
            Console.WriteLine($"Connection to Dataverse:");
            Console.WriteLine(targetConnectionStringLoggable);

            Console.WriteLine("");
            Console.WriteLine("Connecting to the target environment ...");

            // Try and verify the connection
            try
            {
                // Connect
                TargetDataverse = new Microsoft.Xrm.Tooling.Connector.CrmServiceClient(targetConnectionString);
                // verify connection works.
                TargetDataverse.Execute(new WhoAmIRequest());
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error connecting to {RunOptions.TargetEnv}");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                return;
            }
            Console.WriteLine("");

            // Log the connection details
            string sourceConnectionStringLoggable = $"AuthType=Ad;ServiceUri={RunOptions.SourceUrl}/XRMServices/2011/Organization.svc;Timeout=00:30:00;Username={RunOptions.Username};Domain={RunOptions.Domain};Password=************************";
            string sourceConnectionString = $"AuthType=Ad;ServiceUri={RunOptions.SourceUrl}/XRMServices/2011/Organization.svc;Timeout=00:30:00;Username={RunOptions.Username};Domain={RunOptions.Domain};Password={RunOptions.Password}";
            Console.WriteLine($"Connection to source CRM ... ");
            Console.WriteLine(sourceConnectionStringLoggable);

            Console.WriteLine("");
            Console.WriteLine("Connecting to the source environment ...");
            // Try and verify the connection
            try
            {
                // Connect
                SourceCrm = new Microsoft.Xrm.Tooling.Connector.CrmServiceClient(sourceConnectionString);
                // verify connection works.
                SourceCrm.Execute(new WhoAmIRequest());
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error connecting to {RunOptions.SourceUrl}");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                return;
            }

            if (RunOptions.AddDevOpsAppUserToConvPlatform)
            {
                try
                {
                    this.AddDevOpsAppUserToConvPlatformTeam();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error attempting to add DevOps user to team");
                    Console.WriteLine(ex.Message);
                }
            }

            GetTargetEntityMetadata();

            GetMigrationMapping();

        }

        public EntityReference GetMappedOwner(EntityReference er)
        {
            if (er == null)
            {
                return null;
            }
            return GetMappedOwner(er.LogicalName, er.Id);
        }
        public EntityReference GetMappedOwner(string logicalName, Guid id)
        {
            var targetMap = MigrationMappings.FirstOrDefault(m =>
                    m["rp_sourceentitylogicalname"].ToString() == logicalName
                    && m["rp_sourceid"].ToString().ToLowerInvariant() == id.ToString().ToLowerInvariant());

            if (targetMap == null)
            {
                return null;
            }
            var er = new EntityReference(targetMap["rp_targetentitylogicalname"].ToString(), Guid.Parse(targetMap["rp_targetid"].ToString()));
            if (targetMap.Contains("rp_name"))
            {
                er.Name = targetMap["rp_name"].ToString(); ;
            }

            return er;
        }

        public Entity GetMapping(EntityReference er)
        {
            var mapping = MigrationMappings.FirstOrDefault(m =>
                    m["rp_sourceentitylogicalname"].ToString() == er.LogicalName
                    && m["rp_sourceid"].ToString().ToLowerInvariant() == er.Id.ToString().ToLowerInvariant());
            if (mapping == null)
            {
                mapping = MigrationMappings.FirstOrDefault(m =>
                        m["rp_targetentitylogicalname"].ToString() == er.LogicalName
                        && m["rp_targetid"].ToString().ToLowerInvariant() == er.Id.ToString().ToLowerInvariant());
            }
            return mapping;
        }

        public List<Entity> GetSourceUsers()
        {
            if (SourceUsers != null)
            {
                return SourceUsers;
            }
            string usersFetch = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
  <entity name='systemuser'>
    <attribute name='internalemailaddress' />
    <attribute name='isdisabled' />
    <attribute name='systemuserid' />
    <filter>
      <condition attribute='isdisabled' operator='eq' value='0' />
      <condition attribute='internalemailaddress' operator='not-null'/>
    </filter>
  </entity>
</fetch>";


            Console.WriteLine("Retrieving all enabled users, that have an email address in the source system.");
            SourceUsers = SourceCrm.RetrieveMultiple(new FetchExpression(usersFetch)).Entities.ToList();
            return SourceUsers;
        }

        public List<string> GetSourceUserEmails()
        {
            GetSourceUsers();
            return SourceUsers.Select(u => u["internalemailaddress"].ToString().ToLowerInvariant()).ToList();
        }

        public List<Entity> GetTargetUsers()
        {
            if (TargetUsers != null)
            {
                return TargetUsers;
            }
            string usersFetch = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
  <entity name='systemuser'>
    <attribute name='internalemailaddress' />
    <attribute name='isdisabled' />
    <attribute name='systemuserid' />
    <attribute name='accessmode' />
    <filter>
      <condition attribute='isdisabled' operator='eq' value='0' />
      <condition attribute='internalemailaddress' operator='not-null'/>
      <!-- Not non-interactive users -->
      <condition attribute='accessmode' operator='ne' value='4' />
      <condition attribute='accessmode' operator='ne' value='1' />
    </filter>
  </entity>
</fetch>";

            TargetUsers = TargetDataverse.RetrieveMultiple(new FetchExpression(usersFetch)).Entities.ToList();

            return TargetUsers;

        }



        /// <summary>
        /// Get the user views from the source
        /// </summary>
        /// <exception cref="Exception"></exception>
        internal List<Entity> GetTargetAllTargetViews(bool forceReload = false)
        {
            if (TargetViewsAll != null && !forceReload)
            {
                return TargetViewsAll;
            }
            TargetViewsAll = new List<Entity>();

            var targetUsers = GetTargetUsers();

            var targetDataverse = TargetDataverse;

            QueryExpression q = new QueryExpression("savedquery");
            q.ColumnSet = new ColumnSet("savedqueryid", "name");
            var entities = targetDataverse.RetrieveMultiple(q);

            TargetViewsAll.AddRange(entities.Entities);

            #region Get User Views
            q = new QueryExpression("userquery");
            q.ColumnSet = new ColumnSet("userqueryid", "name");

            Console.WriteLine($"Retrieving All userquery from the source system as each user");
            int count = 0;
            // for each user id query the views that the user can see
            foreach (var user in targetUsers)
            {
                count++;
                targetDataverse.CallerId = user.Id;
                Console.WriteLine($"   Retrieving ({count}/{targetUsers.Count}) {user["internalemailaddress"]} user views.");
                try
                {
                    entities = targetDataverse.RetrieveMultiple(q);
                    TargetViewsAll.AddRange(entities.Entities);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"    Error retrieving views for {user["internalemailaddress"]}:");
                    Console.WriteLine($"        {ex.Message}");
                    Console.WriteLine($"        skipping");
                    continue;
                }
                if (entities.TotalRecordCount >= 5000)
                {
                    throw new Exception($"There are {entities.TotalRecordCount} user saved views, more than 5000, code needs to be updated to support more than 5000.");
                }

            }
            targetDataverse.CallerId = Guid.Empty;
            #endregion Get User Views


            return TargetViewsAll;
        }

        internal ExecuteMultipleResponse ExecuteMultipleOnTarget(List<OrganizationRequest> requests)
        {
            ExecuteMultipleRequest multipleRequest = new ExecuteMultipleRequest()
            {
                Settings = new ExecuteMultipleSettings()
                {
                    ContinueOnError = true,
                    ReturnResponses = true
                },
                Requests = new OrganizationRequestCollection()
            };

            multipleRequest.Requests.AddRange(requests);

            TargetDataverse.CallerId = Guid.Empty;
            return TargetDataverse.Execute(multipleRequest) as ExecuteMultipleResponse;

        }

        internal void AddDevOpsAppUserToConvPlatformTeam()
        {
            var appUserAppId = "b20b2cc2-382a-4447-b935-2c525174a895";
            string getDevOpsAppUserFetch = $@"<fetch>
  <entity name='systemuser'>
    <attribute name='domainname' />
    <attribute name='fullname' />
    <attribute name='azureactivedirectoryobjectid' />
    <attribute name='applicationid' />
    <attribute name='systemuserid' />
    <filter>
      <condition attribute='applicationid' operator='eq' value='{appUserAppId}' uitype='systemuser' />
    </filter>
  </entity>
</fetch>";
            var userId = TargetDataverse.RetrieveMultiple(new FetchExpression(getDevOpsAppUserFetch)).Entities.FirstOrDefault()?.Id;
            if (userId == null)
            {
                Console.WriteLine($"Unable to find user with app id {appUserAppId}");
                return;
            }

            string convPlatformTeamFetch = @"<fetch>
  <entity name='team'>
    <attribute name='teamid' />
    <attribute name='name' />
    <attribute name='new_identificationcode' />
    <filter>
      <condition attribute='new_identificationcode' operator='eq' value='CONVERGENCE_PLATFORM' />
    </filter>
  </entity>
</fetch>";
            var teamId = TargetDataverse.RetrieveMultiple(new FetchExpression(convPlatformTeamFetch)).Entities.FirstOrDefault()?.Id;
            if (teamId == null)
            {
                Console.WriteLine($"Unable to find team CONVERGENCE_PLATFORM");
                return;
            }
            AddMembersTeamRequest addUserToTeam = new AddMembersTeamRequest()
            {
                MemberIds = new Guid[] { userId.Value },
                TeamId = teamId.Value
            };
            Console.WriteLine($"Adding Application Id: {appUserAppId} to Team: CONVERGENCE_PLATFORM");
            TargetDataverse.Execute(addUserToTeam);
        }

    }
}
