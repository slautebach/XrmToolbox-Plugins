
//using Microsoft.Crm.Sdk.Messages;
//using Microsoft.Xrm.Sdk;
//using Microsoft.Xrm.Sdk.Messages;
//using Microsoft.Xrm.Sdk.Metadata;
//using Microsoft.Xrm.Sdk.Query;
//using Microsoft.Xrm.Tooling.Connector;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Xml.Linq;
//using System.Xml.XPath;

//internal class Program
//{
//    /// <summary>
//    /// Parsed command line options
//    /// </summary>



//    private static void Main(string[] args)
//    {

//        try
//        {

//            MigrationConfig.Instance.Initialize(args);

       
//            Console.WriteLine("***********************************");
//            Console.WriteLine("       Views");
//            Console.WriteLine("***********************************");
//            MigrateEntitiesWithPermission<UserQuery> migrateUserQueries = new MigrateEntitiesWithPermission<UserQuery>("userquery");
//            //migrateUserQueries.PreDeleteFromTarget = true; // uncomment to cleanup target environment
//            migrateUserQueries.MigrateEntities();

//            Console.WriteLine("");
//            Console.WriteLine("");
//            Console.WriteLine("***********************************");
//            Console.WriteLine("       Finding Dependent Target Views");
//            Console.WriteLine("***********************************");
//            // Load the target user views before we start the dashboard migration
//            MigrationConfig.Instance.GetTargetAllTargetViews(true);

//            Console.WriteLine(""); 
//            Console.WriteLine("");
//            Console.WriteLine("***********************************");
//            Console.WriteLine("       Dashboards");
//            Console.WriteLine("***********************************");

//            MigrateEntitiesWithPermission<UserDashboard> migrateUserDashboards = new MigrateEntitiesWithPermission<UserDashboard>("userform");
//            //migrateUserDashboards.PreDeleteFromTarget = true; // uncomment to cleanup target environment
//            migrateUserDashboards.MigrateEntities();

//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Error Migrationg with exception message: {ex.Message}");
//            return;
//        }
//    }

//}