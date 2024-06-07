using McTools.Xrm.Connection;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using ScintillaNET;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using XrmToolBox.Extensibility;

namespace slautebach.MissingDependencies
{
    public partial class MissingDependenciesPluginControl : PluginControlBase
    {
        private Settings mySettings;

        public MissingDependenciesPluginControl()
        {
            InitializeComponent();
            this.ConnectionUpdated += MissingDependenciesPluginControl_ConnectionUpdated;
            comboBoxSolutions.Format += ComboBoxSolutions_Format;
            toolStripMenu.Visible = true;
            scintillaXml.Lexer = Lexer.Xml;

            scintillaXml.Margins[0].Width = 25;

            scintillaXml.Styles[Style.Xml.Tag].ForeColor = Color.Blue;
            scintillaXml.Styles[Style.Xml.Attribute].ForeColor = Color.Red;
            scintillaXml.Styles[Style.Xml.DoubleString].ForeColor = Color.Purple;
            scintillaXml.Styles[Style.Xml.SingleString].ForeColor = Color.Purple;

            LoadSolutions();
        }

        private void MissingDependenciesPluginControl_ConnectionUpdated(object sender, ConnectionUpdatedEventArgs e)
        {
            LoadSolutions();
        }

        private void ComboBoxSolutions_Format(object sender, ListControlConvertEventArgs e)
        {
            e.Value = "";
            Entity entity = e.ListItem as Entity;
            if (entity.Contains("friendlyname"))
            {
                e.Value = entity["friendlyname"];
            }

        }

        private void On_Load(object sender, EventArgs e)
        {
            ShowInfoNotification("This is a notification that can lead to XrmToolBox repository", new Uri("https://github.com/MscrmTools/XrmToolBox"));

            // Loads or creates the settings for the plugin
            if (!SettingsManager.Instance.TryLoad(GetType(), out mySettings))
            {
                mySettings = new Settings();

                LogWarning("Settings not found => a new settings file has been created!");
            }
            else
            {
                LogInfo("Settings found and loaded");
            }

            tabMissingDependencies.Top = toolStripMenu.Height + 5 + comboBoxSolutions.Height + 5;
            tabMissingDependencies.Left = 0;
        }

        private void tsbClose_Click(object sender, EventArgs e)
        {
            CloseTool();
        }

        private void tsbSample_Click(object sender, EventArgs e)
        {
            // The ExecuteMethod method handles connecting to an
            // organization if XrmToolBox is not yet connected
            ExecuteMethod(LoadDependencies);
        }

        private XDocument FormatXml()
        {
            string xmlString = scintillaXml.Text;
            int nodePostition = xmlString.IndexOf("<MissingDependencies>");
            xmlString = xmlString.Substring(nodePostition);
            string endTag = "</MissingDependencies>";
            nodePostition = xmlString.IndexOf(endTag);
            if (xmlString.Length >= nodePostition + endTag.Length + 1)
            {
                xmlString = xmlString.Substring(0, nodePostition + endTag.Length + 1);
            }
            XDocument doc = XDocument.Parse(xmlString);
            xmlString = doc.ToString();
            scintillaXml.Text = xmlString;
            return doc;
        }



        private void LoadDependencies()
        {
            XDocument xdoc = FormatXml();
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Find Missing Dependencies",
                Work = (worker, args) =>
                {
                    MissingDependencies missingDependencies = new MissingDependencies();
                    var missingDeps = xdoc.XPathSelectElements("/MissingDependencies/MissingDependency");
                    foreach (var missingDep in missingDeps)
                    {
                        var required = missingDep.Elements().FirstOrDefault(e => e.Name == "Required");
                        var dependent = missingDep.Elements().FirstOrDefault(e => e.Name == "Dependent");
                        if (required == null || dependent == null)
                        {
                            // TODO hanlde ERROR Condtition.
                            continue;
                        }
                        missingDependencies.Add(required, dependent, Service);
                    }
                    args.Result = missingDependencies;


                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var results = args.Result as MissingDependencies;

                    results.Dependencies.ForEach(dependency =>
                    {
                       // listViewResults.Items.Add()
                    });
                    
                    tabMissingDependencies.TabPages[1].Show();
                }
            });
        }


        private void LoadSolutions()
        {
            if (Service == null)
            {
                return;
            }
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Loading Solutions",
                Work = (worker, args) =>
                {
                    string fetch = @"<fetch>
  <entity name=""solution"">
    <attribute name=""friendlyname"" />
    <attribute name=""uniquename"" />
    <attribute name=""solutionid"" />
  </entity>
</fetch>";
                    args.Result = Service.RetrieveMultiple(new FetchExpression(fetch)).Entities.ToList();
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    var result = args.Result as List<Entity>;

                    comboBoxSolutions.Items.Clear();
                    comboBoxSolutions.Items.AddRange(result.ToArray());
                    comboBoxSolutions.SelectedItem = result.FirstOrDefault();
                }
            });
        }



        /// <summary>
        /// This event occurs when the plugin is closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void On_CloseTool(object sender, EventArgs e)
        {
            // Before leaving, save the settings
            SettingsManager.Instance.Save(GetType(), mySettings);
        }

        /// <summary>
        /// This event occurs when the connection has been updated in XrmToolBox
        /// </summary>
        public override void UpdateConnection(IOrganizationService newService, ConnectionDetail detail, string actionName, object parameter)
        {
            base.UpdateConnection(newService, detail, actionName, parameter);

            if (mySettings != null && detail != null)
            {
                mySettings.LastUsedOrganizationWebappUrl = detail.WebApplicationUrl;
                LogInfo("Connection has changed to: {0}", detail.WebApplicationUrl);
            }
        }

        private void On_Resize(object sender, EventArgs e)
        {
            tabMissingDependencies.Width = this.Width - 1;
            tabMissingDependencies.Height = this.Height - (toolStripMenu.Height + comboBoxSolutions.Height +  15);

            scintillaXml.Width = tabMissingDependencies.Width - 20;
            scintillaXml.Height = tabMissingDependencies.Height - 35;
        }

        private void buttonRefreshSolution_Click(object sender, EventArgs e)
        {
            LoadSolutions();
        }

        private void toolStripMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}