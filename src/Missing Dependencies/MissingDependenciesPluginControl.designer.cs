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
            this.tabPageXml = new System.Windows.Forms.TabPage();
            this.tabPageReport = new System.Windows.Forms.TabPage();
            this.textBoxDependencyXml = new System.Windows.Forms.TextBox();
            this.toolStripMenu.SuspendLayout();
            this.tabMissingDependencies.SuspendLayout();
            this.tabPageXml.SuspendLayout();
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
            this.toolStripMenu.Size = new System.Drawing.Size(629, 25);
            this.toolStripMenu.TabIndex = 4;
            this.toolStripMenu.Text = "toolStrip1";
            // 
            // tsbClose
            // 
            this.tsbClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbClose.Name = "tsbClose";
            this.tsbClose.Size = new System.Drawing.Size(86, 22);
            this.tsbClose.Text = "Close this tool";
            this.tsbClose.Click += new System.EventHandler(this.tsbClose_Click);
            // 
            // tssSeparator1
            // 
            this.tssSeparator1.Name = "tssSeparator1";
            this.tssSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbSample
            // 
            this.tsbSample.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbSample.Name = "tsbSample";
            this.tsbSample.Size = new System.Drawing.Size(46, 22);
            this.tsbSample.Text = "Try me";
            this.tsbSample.Click += new System.EventHandler(this.tsbSample_Click);
            // 
            // tabMissingDependencies
            // 
            this.tabMissingDependencies.Controls.Add(this.tabPageXml);
            this.tabMissingDependencies.Controls.Add(this.tabPageReport);
            this.tabMissingDependencies.Location = new System.Drawing.Point(3, 28);
            this.tabMissingDependencies.Name = "tabMissingDependencies";
            this.tabMissingDependencies.SelectedIndex = 0;
            this.tabMissingDependencies.Size = new System.Drawing.Size(623, 331);
            this.tabMissingDependencies.TabIndex = 5;
            // 
            // tabPageXml
            // 
            this.tabPageXml.Controls.Add(this.textBoxDependencyXml);
            this.tabPageXml.Location = new System.Drawing.Point(4, 22);
            this.tabPageXml.Name = "tabPageXml";
            this.tabPageXml.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageXml.Size = new System.Drawing.Size(615, 305);
            this.tabPageXml.TabIndex = 0;
            this.tabPageXml.Text = "Missing Dependency Xml";
            this.tabPageXml.UseVisualStyleBackColor = true;
            // 
            // tabPageReport
            // 
            this.tabPageReport.Location = new System.Drawing.Point(4, 22);
            this.tabPageReport.Name = "tabPageReport";
            this.tabPageReport.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageReport.Size = new System.Drawing.Size(615, 305);
            this.tabPageReport.TabIndex = 1;
            this.tabPageReport.Text = "Dependency Report";
            this.tabPageReport.UseVisualStyleBackColor = true;
            // 
            // textBoxDependencyXml
            // 
            this.textBoxDependencyXml.Location = new System.Drawing.Point(6, 6);
            this.textBoxDependencyXml.Multiline = true;
            this.textBoxDependencyXml.Name = "textBoxDependencyXml";
            this.textBoxDependencyXml.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxDependencyXml.Size = new System.Drawing.Size(603, 293);
            this.textBoxDependencyXml.TabIndex = 0;
            // 
            // MissingDependenciesPluginControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabMissingDependencies);
            this.Controls.Add(this.toolStripMenu);
            this.Name = "MissingDependenciesPluginControl";
            this.Size = new System.Drawing.Size(629, 362);
            this.Load += new System.EventHandler(this.On_Load);
            this.Resize += new System.EventHandler(this.On_Resize);
            this.toolStripMenu.ResumeLayout(false);
            this.toolStripMenu.PerformLayout();
            this.tabMissingDependencies.ResumeLayout(false);
            this.tabPageXml.ResumeLayout(false);
            this.tabPageXml.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStripMenu;
        private System.Windows.Forms.ToolStripButton tsbClose;
        private System.Windows.Forms.ToolStripButton tsbSample;
        private System.Windows.Forms.ToolStripSeparator tssSeparator1;
        private System.Windows.Forms.TabControl tabMissingDependencies;
        private System.Windows.Forms.TabPage tabPageXml;
        private System.Windows.Forms.TabPage tabPageReport;
        private System.Windows.Forms.TextBox textBoxDependencyXml;
    }
}
