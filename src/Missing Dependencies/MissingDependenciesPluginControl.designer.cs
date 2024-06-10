namespace slautebach.MissingDependencies
{
    partial class MissingDependenciesPluginControl
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.toolStripMenu = new System.Windows.Forms.ToolStrip();
            this.tsbClose = new System.Windows.Forms.ToolStripButton();
            this.tssSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbSample = new System.Windows.Forms.ToolStripButton();
            this.tabMissingDependencies = new System.Windows.Forms.TabControl();
            this.tabPageMissingXml = new System.Windows.Forms.TabPage();
            this.missingDependencyXmlScintilla = new ScintillaNET.Scintilla();
            this.tabPageSolutionList = new System.Windows.Forms.TabPage();
            this.solutionSplitContainer = new System.Windows.Forms.SplitContainer();
            this.toolStripMenu.SuspendLayout();
            this.tabMissingDependencies.SuspendLayout();
            this.tabPageMissingXml.SuspendLayout();
            this.tabPageSolutionList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.solutionSplitContainer)).BeginInit();
            this.solutionSplitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripMenu
            // 
            this.toolStripMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStripMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbClose,
            this.tssSeparator1,
            this.tsbSample});
            this.toolStripMenu.Location = new System.Drawing.Point(0, 0);
            this.toolStripMenu.Name = "toolStripMenu";
            this.toolStripMenu.Size = new System.Drawing.Size(1088, 40);
            this.toolStripMenu.TabIndex = 4;
            this.toolStripMenu.Text = "toolStrip1";
            // 
            // tsbClose
            // 
            this.tsbClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbClose.Name = "tsbClose";
            this.tsbClose.Size = new System.Drawing.Size(148, 34);
            this.tsbClose.Text = "Close this tool";
            this.tsbClose.Click += new System.EventHandler(this.tsbClose_Click);
            // 
            // tssSeparator1
            // 
            this.tssSeparator1.Name = "tssSeparator1";
            this.tssSeparator1.Size = new System.Drawing.Size(6, 40);
            // 
            // tsbSample
            // 
            this.tsbSample.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbSample.Name = "tsbSample";
            this.tsbSample.Size = new System.Drawing.Size(78, 34);
            this.tsbSample.Text = "Try me";
            this.tsbSample.Click += new System.EventHandler(this.tsbSample_Click);
            // 
            // tabMissingDependencies
            // 
            this.tabMissingDependencies.Controls.Add(this.tabPageMissingXml);
            this.tabMissingDependencies.Controls.Add(this.tabPageSolutionList);
            this.tabMissingDependencies.Location = new System.Drawing.Point(3, 43);
            this.tabMissingDependencies.Name = "tabMissingDependencies";
            this.tabMissingDependencies.SelectedIndex = 0;
            this.tabMissingDependencies.Size = new System.Drawing.Size(1082, 683);
            this.tabMissingDependencies.TabIndex = 5;
            // 
            // tabPageMissingXml
            // 
            this.tabPageMissingXml.Controls.Add(this.missingDependencyXmlScintilla);
            this.tabPageMissingXml.Location = new System.Drawing.Point(4, 33);
            this.tabPageMissingXml.Name = "tabPageMissingXml";
            this.tabPageMissingXml.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMissingXml.Size = new System.Drawing.Size(1074, 646);
            this.tabPageMissingXml.TabIndex = 0;
            this.tabPageMissingXml.Text = "Missing Dependency Xml";
            this.tabPageMissingXml.UseVisualStyleBackColor = true;
            // 
            // missingDependencyXmlScintilla
            // 
            this.missingDependencyXmlScintilla.Location = new System.Drawing.Point(7, 7);
            this.missingDependencyXmlScintilla.Name = "missingDependencyXmlScintilla";
            this.missingDependencyXmlScintilla.Size = new System.Drawing.Size(1061, 633);
            this.missingDependencyXmlScintilla.TabIndex = 0;
            // 
            // tabPageSolutionList
            // 
            this.tabPageSolutionList.Controls.Add(this.solutionSplitContainer);
            this.tabPageSolutionList.Location = new System.Drawing.Point(4, 33);
            this.tabPageSolutionList.Name = "tabPageSolutionList";
            this.tabPageSolutionList.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSolutionList.Size = new System.Drawing.Size(1074, 646);
            this.tabPageSolutionList.TabIndex = 1;
            this.tabPageSolutionList.Text = "tabPage2";
            this.tabPageSolutionList.UseVisualStyleBackColor = true;
            // 
            // solutionSplitContainer
            // 
            this.solutionSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.solutionSplitContainer.Location = new System.Drawing.Point(3, 3);
            this.solutionSplitContainer.Name = "solutionSplitContainer";
            this.solutionSplitContainer.Size = new System.Drawing.Size(1068, 640);
            this.solutionSplitContainer.SplitterDistance = 356;
            this.solutionSplitContainer.TabIndex = 0;
            // 
            // MissingDependenciesPluginControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabMissingDependencies);
            this.Controls.Add(this.toolStripMenu);
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Name = "MissingDependenciesPluginControl";
            this.Size = new System.Drawing.Size(1088, 729);
            this.Load += new System.EventHandler(this.MyPluginControl_Load);
            this.toolStripMenu.ResumeLayout(false);
            this.toolStripMenu.PerformLayout();
            this.tabMissingDependencies.ResumeLayout(false);
            this.tabPageMissingXml.ResumeLayout(false);
            this.tabPageSolutionList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.solutionSplitContainer)).EndInit();
            this.solutionSplitContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStripMenu;
        private System.Windows.Forms.ToolStripButton tsbClose;
        private System.Windows.Forms.ToolStripButton tsbSample;
        private System.Windows.Forms.ToolStripSeparator tssSeparator1;
        private System.Windows.Forms.TabControl tabMissingDependencies;
        private System.Windows.Forms.TabPage tabPageMissingXml;
        private System.Windows.Forms.TabPage tabPageSolutionList;
        private ScintillaNET.Scintilla missingDependencyXmlScintilla;
        private System.Windows.Forms.SplitContainer solutionSplitContainer;
    }
}
