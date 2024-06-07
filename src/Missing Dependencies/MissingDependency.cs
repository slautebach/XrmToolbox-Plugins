using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace slautebach.MissingDependencies
{

    public enum ComponentType
    {
        Unknown = 0,
        AIConfiguration = 402,
        AIProject = 401,
        AIProjectType = 400,
        Attachment = 35,
        Attribute = 2,
        AttributeLookupValue = 5,
        AttributeMap = 47,
        AttributePicklistValue = 4,
        CanvasApp = 300,
        ComplexControl = 64,


        ConnectionRole = 63,


        Connector_371 = 371,


        Connector_372 = 372,


        ContractTemplate = 37,


        ConvertRule = 154,


        ConvertRuleItem = 155,


        CustomControl = 66,


        CustomControlDefaultConfig = 68,


        DataSourceMapping = 166,


        DisplayString = 22,


        DisplayStringMap = 23,


        DuplicateRule = 44,


        DuplicateRuleCondition = 45,


        EmailTemplate = 36,


        Entity = 1,


        EntityAnalyticsConfiguration = 430,


        EntityKey = 14,


        EntityMap = 46,


        EntityRelationship = 10,


        EntityRelationshipRelationships = 12,


        EntityRelationshipRole = 11,


        EnvironmentVariableDefinition = 380,


        EnvironmentVariableValue = 381,


        FieldPermission = 71,


        FieldSecurityProfile = 70,


        Form = 24,


        HierarchyRule = 65,


        ImportMap = 208,


        Index = 18,


        KBArticleTemplate = 38,


        LocalizedLabel = 7,


        MailMergeTemplate = 39,


        ManagedProperty = 13,


        MobileOfflineProfile = 161,


        MobileOfflineProfileItem = 162,


        OptionSet = 9,


        Organization = 25,


        PluginAssembly = 91,


        PluginType = 90,


        Privilege = 16,


        PrivilegeObjectTypeCode = 17,


        Relationship = 3,


        RelationshipExtraCondition = 8,


        Report = 31,


        ReportCategory = 33,


        ReportEntity = 32,


        ReportVisibility = 34,


        RibbonCommand = 48,


        RibbonContextGroup = 49,


        RibbonCustomization = 50,


        RibbonDiff = 55,


        RibbonRule = 52,


        RibbonTabToCommandMap = 53,


        Role = 20,


        RolePrivilege = 21,


        RoutingRule = 150,


        RoutingRuleItem = 151,


        SavedQuery = 26,


        SavedQueryVisualization = 59,


        SDKMessage = 201,


        SDKMessageFilter = 202,


        SdkMessagePair = 203,


        SDKMessageProcessingStep = 92,


        SDKMessageProcessingStepImage = 93,


        SdkMessageRequest = 204,


        SdkMessageRequestField = 205,


        SdkMessageResponse = 206,


        SdkMessageResponseField = 207,


        ServiceEndpoint = 95,


        SimilarityRule = 165,


        SiteMap = 62,


        SLA = 152,


        SLAItem = 153,


        SystemForm = 60,


        ViewAttribute = 6,


        WebResource = 61,


        WebWizard = 210,


        Workflow = 29,

        // String types
        ConnectionReference = 1000000
    }


    public class Component
    {

        public Component(XElement element)
        {
            DisplayName = element.Attribute("displayName")?.Value;
            Solution = element.Attribute("solution")?.Value;
            Id = element.Attribute("id")?.Value;
            strType = element.Attribute("type")?.Value.ToLower();

            // find all additional id properties
            var alternativeId = element.Attributes().FirstOrDefault(a => a.Name.LocalName.StartsWith("id."));
            if (alternativeId != null)
            {
                Id = alternativeId.Value;
            }
        }

        // Type
        private string strType { get; set; }

        public ComponentType Type
        {
            get
            {
                if (string.IsNullOrEmpty(strType))
                {
                    return ComponentType.Unknown;
                }
                if (int.TryParse(strType, out var type))
                {
                    return (ComponentType)type;
                }
                if (strType == "connectionreference")
                {
                    return ComponentType.ConnectionReference;
                }
                throw new NotImplementedException($"Tye type: {strType}, has not been implemented");
            }
        }


        // Display Name
        public string DisplayName { get; set; }

        public string Solution { get; set; }

        public string Id { get; set; }

        public Guid IdGuid
        {
            get
            {
                if (string.IsNullOrEmpty(Id))
                {
                    return Guid.Empty;
                }
                return Guid.Parse(Id);
            }
        }

    }

    public class MissingDependencies
    {
        private List<MissingDependency> missingDependencies = new List<MissingDependency>();

        public void Add(XElement required, XElement dependent, IOrganizationService service)
        {

            var reqComponent = new Component(required);
            var depComponent = new Component(dependent);

            var existing = missingDependencies.FirstOrDefault(m => m.Required.Id == reqComponent.Id);
            if (existing == null)
            {
                existing = new MissingDependency(reqComponent);
                missingDependencies.Add(existing);
            }
            existing.Dependents.Add(depComponent);
        }

        public List<MissingDependency> Dependencies
        {
            get
            {
                return missingDependencies;
            }
        }
    }

    public class MissingDependency
    {
        public MissingDependency(Component required)
        {
            Required = required;
        }

        public Component Required { get; set; }
        public List<Component> Dependents { get; set; }
    }
}
