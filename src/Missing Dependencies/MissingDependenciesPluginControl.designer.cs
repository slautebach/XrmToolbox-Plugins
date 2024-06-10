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
            this.tabMissingDependencies = new System.Windows.Forms.TabControl();
            this.tabPageXml = new System.Windows.Forms.TabPage();
            this.scintillaXml = new ScintillaNET.Scintilla();
            this.buttonRefreshSolution = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxSolutions = new System.Windows.Forms.ComboBox();
            this.tsbClose = new System.Windows.Forms.ToolStripButton();
            this.tssSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbSample = new System.Windows.Forms.ToolStripButton();
            this.toolStripMenu = new System.Windows.Forms.ToolStrip();
            this.tabPageReport = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.tabMissingDependencies.SuspendLayout();
            this.tabPageXml.SuspendLayout();
            this.toolStripMenu.SuspendLayout();
            this.tabPageReport.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabMissingDependencies
            // 
            this.tabMissingDependencies.Controls.Add(this.tabPageXml);
            this.tabMissingDependencies.Controls.Add(this.tabPageReport);
            this.tabMissingDependencies.Location = new System.Drawing.Point(3, 53);
            this.tabMissingDependencies.Name = "tabMissingDependencies";
            this.tabMissingDependencies.SelectedIndex = 0;
            this.tabMissingDependencies.Size = new System.Drawing.Size(623, 306);
            this.tabMissingDependencies.TabIndex = 5;
            // 
            // tabPageXml
            // 
            this.tabPageXml.Controls.Add(this.scintillaXml);
            this.tabPageXml.Location = new System.Drawing.Point(4, 22);
            this.tabPageXml.Name = "tabPageXml";
            this.tabPageXml.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageXml.Size = new System.Drawing.Size(615, 280);
            this.tabPageXml.TabIndex = 0;
            this.tabPageXml.Text = "Missing Dependency Xml";
            this.tabPageXml.UseVisualStyleBackColor = true;
            // 
            // scintillaXml
            // 
            this.scintillaXml.Lexer = ScintillaNET.Lexer.Xml;
            this.scintillaXml.Location = new System.Drawing.Point(7, 6);
            this.scintillaXml.Name = "scintillaXml";
            this.scintillaXml.Size = new System.Drawing.Size(602, 268);
            this.scintillaXml.TabIndex = 4;
            this.scintillaXml.WrapMode = ScintillaNET.WrapMode.Word;
            // 
            // buttonRefreshSolution
            // 
            this.buttonRefreshSolution.Location = new System.Drawing.Point(245, 28);
            this.buttonRefreshSolution.Name = "buttonRefreshSolution";
            this.buttonRefreshSolution.Size = new System.Drawing.Size(116, 23);
            this.buttonRefreshSolution.TabIndex = 3;
            this.buttonRefreshSolution.Text = "Refresh Solutions";
            this.buttonRefreshSolution.UseVisualStyleBackColor = true;
            this.buttonRefreshSolution.Click += new System.EventHandler(this.buttonRefreshSolution_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Solution";
            // 
            // comboBoxSolutions
            // 
            this.comboBoxSolutions.FormattingEnabled = true;
            this.comboBoxSolutions.Location = new System.Drawing.Point(60, 28);
            this.comboBoxSolutions.Name = "comboBoxSolutions";
            this.comboBoxSolutions.Size = new System.Drawing.Size(169, 21);
            this.comboBoxSolutions.TabIndex = 1;
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
            this.tsbSample.Size = new System.Drawing.Size(75, 22);
            this.tsbSample.Text = "Load Report";
            this.tsbSample.Click += new System.EventHandler(this.tsbSample_Click);
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
            this.toolStripMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStripMenu_ItemClicked);
            // 
            // tabPageReport
            // 
            this.tabPageReport.Controls.Add(this.splitContainer1);
            this.tabPageReport.Location = new System.Drawing.Point(4, 22);
            this.tabPageReport.Name = "tabPageReport";
            this.tabPageReport.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageReport.Size = new System.Drawing.Size(615, 280);
            this.tabPageReport.TabIndex = 1;
            this.tabPageReport.Text = "Dependency Report";
            this.tabPageReport.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listBox1);
            this.splitContainer1.Size = new System.Drawing.Size(609, 274);
            this.splitContainer1.SplitterDistance = 203;
            this.splitContainer1.TabIndex = 1;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(4, 4);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(196, 264);
            this.listBox1.TabIndex = 0;
            // 
            // MissingDependenciesPluginControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabMissingDependencies);
            this.Controls.Add(this.buttonRefreshSolution);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.toolStripMenu);
            this.Controls.Add(this.comboBoxSolutions);
            this.Name = "MissingDependenciesPluginControl";
            this.Size = new System.Drawing.Size(629, 362);
            this.Load += new System.EventHandler(this.On_Load);
            this.Resize += new System.EventHandler(this.On_Resize);
            this.tabMissingDependencies.ResumeLayout(false);
            this.tabPageXml.ResumeLayout(false);
            this.toolStripMenu.ResumeLayout(false);
            this.toolStripMenu.PerformLayout();
            this.tabPageReport.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TabControl tabMissingDependencies;
        private System.Windows.Forms.TabPage tabPageXml;
        private System.Windows.Forms.Button buttonRefreshSolution;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxSolutions;
        private System.Windows.Forms.ToolStripButton tsbClose;
        private System.Windows.Forms.ToolStripSeparator tssSeparator1;
        private System.Windows.Forms.ToolStripButton tsbSample;
        private System.Windows.Forms.ToolStrip toolStripMenu;
        private ScintillaNET.Scintilla scintillaXml;
        private System.Windows.Forms.TabPage tabPageReport;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox listBox1;
    }
}
