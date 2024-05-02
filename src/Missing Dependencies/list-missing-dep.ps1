param(
    [string] $xmlDepFile = ""
)

$xml = [xml](Get-Content $xmlDepFile)

$requiredComponents = $xml.SelectNodes("//MissingDependencies/MissingDependency/Required")


enum ComponentType {
    Entity	=	1
    Attribute	=	2
    Relationship	=	3
    AttributePicklistValue	=	4
    AttributeLookupValue	=	5
    ViewAttribute	=	6
    LocalizedLabel	=	7
    RelationshipExtraCondition	=	8
    OptionSet	=	9
    EntityRelationship	=	10
    EntityRelationshipRole	=	11
    EntityRelationshipRelationships	=	12
    ManagedProperty	=	13
    Role	=	20
    RolePrivilege	=	21
    DisplayString	=	22
    DisplayStringMap	=	23
    Form	=	24
    Organization	=	25
    SavedQuery	=	26
    Workflow	=	29
    Report	=	31
    ReportEntity	=	32
    ReportCategory	=	33
    ReportVisibility	=	34
    Attachment	=	35
    EmailTemplate	=	36
    ContractTemplate	=	37
    KBArticleTemplate	=	38
    MailMergeTemplate	=	39
    DuplicateRule	=	44
    DuplicateRuleCondition	=	45
    EntityMap	=	46
    AttributeMap	=	47
    RibbonCommand	=	48
    RibbonContextGroup	=	49
    RibbonCustomization	=	50
    RibbonRule	=	52
    RibbonTabToCommandMap	=	53
    RibbonDiff	=	55
    SavedQueryVisualization	=	59
    SystemForm	=	60
    WebResource	=	61
    SiteMap	=	62
    ConnectionRole	=	63
    FieldSecurityProfile	=	70
    FieldPermission	=	71
    PluginType	=	90
    PluginAssembly	=	91
    SDKMessageProcessingStep	=	92
    SDKMessageProcessingStepImage	=	93
    ServiceEndpoint	=	95    
}

$missingComponents = [ordered]@{}
$requiredComponents | ForEach-Object {
    $component = $_
    #$_ | Format-List
    $type = [ComponentType]$component.type
    $schemaName = $component.schemaName
    $displayName = $component.displayName
    $parentSchemaName = $component.parentSchemaName
    #$parentDisplayName = $component.parentDisplayName

    $key = $schemaName
    if ($parentSchemaName) {
        $key = "$parentSchemaName" #.$schemaName"
        $schemaName = "$parentSchemaName.$schemaName"
    }

    if (!$missingComponents.Contains($key)) {
        $missingComponents[$key] = @{}
    }
    if (!$missingComponents[$key].ContainsKey($type)) {
        $missingComponents[$key][$type] = @()
    }
    $missingComponents[$key][$type] += "$schemaName - $displayName - $schemaName"

}

foreach ($key in $missingComponents.Keys) {
    Write-Output ""
    Write-Output ""
    Write-Output $key
    foreach ($type in $missingComponents[$key].Keys) {
        Write-Output ""
        Write-Output "  $type"
        $missingComponents[$key][$type] | ForEach-Object {
            Write-Output "    ==> $_"
        }
    }
}