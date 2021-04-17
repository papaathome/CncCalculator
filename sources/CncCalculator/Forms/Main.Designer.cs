namespace As.Apps.Forms
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.filesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadMaterialsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelWarning = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBoxResults = new System.Windows.Forms.GroupBox();
            this.labelResultFeed = new System.Windows.Forms.Label();
            this.labelResultSpindleSpeed = new System.Windows.Forms.Label();
            this.labelResultCuttingSpeed = new System.Windows.Forms.Label();
            this.groupBoxMaterial = new System.Windows.Forms.GroupBox();
            this.radioButtonMaterialSpindleSpeed = new System.Windows.Forms.RadioButton();
            this.radioButtonMaterialCuttingSpeed = new System.Windows.Forms.RadioButton();
            this.labelMaterialFeedPerTooth = new System.Windows.Forms.Label();
            this.labelMaterialSpindleSpeed = new System.Windows.Forms.Label();
            this.labelMaterialCuttingSpeed = new System.Windows.Forms.Label();
            this.groupBoxTool = new System.Windows.Forms.GroupBox();
            this.labelToolFlutes = new System.Windows.Forms.Label();
            this.labelToolCuttingDiameter = new System.Windows.Forms.Label();
            this.labelToolCuttingDepth = new System.Windows.Forms.Label();
            this.toolStripLists = new System.Windows.Forms.ToolStrip();
            this.toolStripLabelTools = new System.Windows.Forms.ToolStripLabel();
            this.toolStripComboBoxTools = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabelMaterial = new System.Windows.Forms.ToolStripLabel();
            this.toolStripComboBoxMaterials = new System.Windows.Forms.ToolStripComboBox();
            this.scaledTypeResultFeed = new As.Tools.Controls.ScaledTypeControl();
            this.scaledTypeResultSpindleSpeed = new As.Tools.Controls.ScaledTypeControl();
            this.scaledTypeResultCuttingSpeed = new As.Tools.Controls.ScaledTypeControl();
            this.scaledTypeMaterialFeedPerTooth = new As.Tools.Controls.ScaledTypeControl();
            this.scaledTypeMaterialSpindleSpeed = new As.Tools.Controls.ScaledTypeControl();
            this.scaledTypeMaterialCuttingSpeed = new As.Tools.Controls.ScaledTypeControl();
            this.scaledTypeToolFlutes = new As.Tools.Controls.ScaledTypeControl();
            this.scaledTypeToolCuttingDiameter = new As.Tools.Controls.ScaledTypeControl();
            this.scaledTypeToolCuttingDepth = new As.Tools.Controls.ScaledTypeControl();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBoxResults.SuspendLayout();
            this.groupBoxMaterial.SuspendLayout();
            this.groupBoxTool.SuspendLayout();
            this.toolStripLists.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.filesToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(442, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // filesToolStripMenuItem
            // 
            this.filesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadToolsToolStripMenuItem,
            this.loadMaterialsToolStripMenuItem,
            this.toolStripSeparator1,
            this.quitToolStripMenuItem});
            this.filesToolStripMenuItem.Name = "filesToolStripMenuItem";
            this.filesToolStripMenuItem.Size = new System.Drawing.Size(42, 20);
            this.filesToolStripMenuItem.Text = "Files";
            // 
            // loadToolsToolStripMenuItem
            // 
            this.loadToolsToolStripMenuItem.Name = "loadToolsToolStripMenuItem";
            this.loadToolsToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.loadToolsToolStripMenuItem.Text = "Load Tools...";
            this.loadToolsToolStripMenuItem.Click += new System.EventHandler(this.LoadToolsToolStripMenuItem_Click);
            // 
            // loadMaterialsToolStripMenuItem
            // 
            this.loadMaterialsToolStripMenuItem.Name = "loadMaterialsToolStripMenuItem";
            this.loadMaterialsToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.loadMaterialsToolStripMenuItem.Text = "Load Materials...";
            this.loadMaterialsToolStripMenuItem.Click += new System.EventHandler(this.LoadMaterialsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(157, 6);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.quitToolStripMenuItem.Text = "Quit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.QuitToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelWarning});
            this.statusStrip1.Location = new System.Drawing.Point(0, 387);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(442, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabelWarning
            // 
            this.toolStripStatusLabelWarning.Name = "toolStripStatusLabelWarning";
            this.toolStripStatusLabelWarning.Size = new System.Drawing.Size(79, 17);
            this.toolStripStatusLabelWarning.Text = "No Warnings.";
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.splitContainer1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(442, 338);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 24);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(442, 363);
            this.toolStripContainer1.TabIndex = 3;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStripLists);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBoxResults);
            this.splitContainer1.Panel1.Controls.Add(this.groupBoxMaterial);
            this.splitContainer1.Panel1.Controls.Add(this.groupBoxTool);
            this.splitContainer1.Size = new System.Drawing.Size(442, 338);
            this.splitContainer1.SplitterDistance = 407;
            this.splitContainer1.TabIndex = 0;
            // 
            // groupBoxResults
            // 
            this.groupBoxResults.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxResults.Controls.Add(this.scaledTypeResultFeed);
            this.groupBoxResults.Controls.Add(this.scaledTypeResultSpindleSpeed);
            this.groupBoxResults.Controls.Add(this.scaledTypeResultCuttingSpeed);
            this.groupBoxResults.Controls.Add(this.labelResultFeed);
            this.groupBoxResults.Controls.Add(this.labelResultSpindleSpeed);
            this.groupBoxResults.Controls.Add(this.labelResultCuttingSpeed);
            this.groupBoxResults.Location = new System.Drawing.Point(12, 218);
            this.groupBoxResults.Name = "groupBoxResults";
            this.groupBoxResults.Size = new System.Drawing.Size(387, 103);
            this.groupBoxResults.TabIndex = 15;
            this.groupBoxResults.TabStop = false;
            this.groupBoxResults.Text = "Feeds and Speeds";
            // 
            // labelResultFeed
            // 
            this.labelResultFeed.AutoSize = true;
            this.labelResultFeed.Location = new System.Drawing.Point(25, 77);
            this.labelResultFeed.Name = "labelResultFeed";
            this.labelResultFeed.Size = new System.Drawing.Size(31, 13);
            this.labelResultFeed.TabIndex = 10;
            this.labelResultFeed.Text = "Feed";
            // 
            // labelResultSpindleSpeed
            // 
            this.labelResultSpindleSpeed.AutoSize = true;
            this.labelResultSpindleSpeed.Location = new System.Drawing.Point(25, 50);
            this.labelResultSpindleSpeed.Name = "labelResultSpindleSpeed";
            this.labelResultSpindleSpeed.Size = new System.Drawing.Size(74, 13);
            this.labelResultSpindleSpeed.TabIndex = 6;
            this.labelResultSpindleSpeed.Text = "Spindle speed";
            // 
            // labelResultCuttingSpeed
            // 
            this.labelResultCuttingSpeed.AutoSize = true;
            this.labelResultCuttingSpeed.Location = new System.Drawing.Point(25, 24);
            this.labelResultCuttingSpeed.Name = "labelResultCuttingSpeed";
            this.labelResultCuttingSpeed.Size = new System.Drawing.Size(72, 13);
            this.labelResultCuttingSpeed.TabIndex = 2;
            this.labelResultCuttingSpeed.Text = "Cutting speed";
            // 
            // groupBoxMaterial
            // 
            this.groupBoxMaterial.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxMaterial.Controls.Add(this.scaledTypeMaterialFeedPerTooth);
            this.groupBoxMaterial.Controls.Add(this.scaledTypeMaterialSpindleSpeed);
            this.groupBoxMaterial.Controls.Add(this.scaledTypeMaterialCuttingSpeed);
            this.groupBoxMaterial.Controls.Add(this.radioButtonMaterialSpindleSpeed);
            this.groupBoxMaterial.Controls.Add(this.radioButtonMaterialCuttingSpeed);
            this.groupBoxMaterial.Controls.Add(this.labelMaterialFeedPerTooth);
            this.groupBoxMaterial.Controls.Add(this.labelMaterialSpindleSpeed);
            this.groupBoxMaterial.Controls.Add(this.labelMaterialCuttingSpeed);
            this.groupBoxMaterial.Location = new System.Drawing.Point(12, 109);
            this.groupBoxMaterial.Name = "groupBoxMaterial";
            this.groupBoxMaterial.Size = new System.Drawing.Size(387, 103);
            this.groupBoxMaterial.TabIndex = 5;
            this.groupBoxMaterial.TabStop = false;
            this.groupBoxMaterial.Text = "Material";
            // 
            // radioButtonMaterialSpindleSpeed
            // 
            this.radioButtonMaterialSpindleSpeed.AutoSize = true;
            this.radioButtonMaterialSpindleSpeed.Location = new System.Drawing.Point(120, 50);
            this.radioButtonMaterialSpindleSpeed.Name = "radioButtonMaterialSpindleSpeed";
            this.radioButtonMaterialSpindleSpeed.Size = new System.Drawing.Size(14, 13);
            this.radioButtonMaterialSpindleSpeed.TabIndex = 14;
            this.radioButtonMaterialSpindleSpeed.UseVisualStyleBackColor = true;
            this.radioButtonMaterialSpindleSpeed.CheckedChanged += new System.EventHandler(this.RadioButtonMaterialSpindleSpeed_CheckedChanged);
            // 
            // radioButtonMaterialCuttingSpeed
            // 
            this.radioButtonMaterialCuttingSpeed.AutoSize = true;
            this.radioButtonMaterialCuttingSpeed.Location = new System.Drawing.Point(120, 24);
            this.radioButtonMaterialCuttingSpeed.Name = "radioButtonMaterialCuttingSpeed";
            this.radioButtonMaterialCuttingSpeed.Size = new System.Drawing.Size(14, 13);
            this.radioButtonMaterialCuttingSpeed.TabIndex = 13;
            this.radioButtonMaterialCuttingSpeed.UseVisualStyleBackColor = true;
            this.radioButtonMaterialCuttingSpeed.CheckedChanged += new System.EventHandler(this.RadioButtonMaterialCuttingSpeed_CheckedChanged);
            // 
            // labelMaterialFeedPerTooth
            // 
            this.labelMaterialFeedPerTooth.AutoSize = true;
            this.labelMaterialFeedPerTooth.Location = new System.Drawing.Point(25, 77);
            this.labelMaterialFeedPerTooth.Name = "labelMaterialFeedPerTooth";
            this.labelMaterialFeedPerTooth.Size = new System.Drawing.Size(76, 13);
            this.labelMaterialFeedPerTooth.TabIndex = 10;
            this.labelMaterialFeedPerTooth.Text = "Feed per tooth";
            // 
            // labelMaterialSpindleSpeed
            // 
            this.labelMaterialSpindleSpeed.AutoSize = true;
            this.labelMaterialSpindleSpeed.Location = new System.Drawing.Point(25, 50);
            this.labelMaterialSpindleSpeed.Name = "labelMaterialSpindleSpeed";
            this.labelMaterialSpindleSpeed.Size = new System.Drawing.Size(74, 13);
            this.labelMaterialSpindleSpeed.TabIndex = 6;
            this.labelMaterialSpindleSpeed.Text = "Spindle speed";
            // 
            // labelMaterialCuttingSpeed
            // 
            this.labelMaterialCuttingSpeed.AutoSize = true;
            this.labelMaterialCuttingSpeed.Location = new System.Drawing.Point(25, 24);
            this.labelMaterialCuttingSpeed.Name = "labelMaterialCuttingSpeed";
            this.labelMaterialCuttingSpeed.Size = new System.Drawing.Size(72, 13);
            this.labelMaterialCuttingSpeed.TabIndex = 2;
            this.labelMaterialCuttingSpeed.Text = "Cutting speed";
            // 
            // groupBoxTool
            // 
            this.groupBoxTool.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxTool.Controls.Add(this.scaledTypeToolFlutes);
            this.groupBoxTool.Controls.Add(this.scaledTypeToolCuttingDiameter);
            this.groupBoxTool.Controls.Add(this.scaledTypeToolCuttingDepth);
            this.groupBoxTool.Controls.Add(this.labelToolFlutes);
            this.groupBoxTool.Controls.Add(this.labelToolCuttingDiameter);
            this.groupBoxTool.Controls.Add(this.labelToolCuttingDepth);
            this.groupBoxTool.Location = new System.Drawing.Point(12, 3);
            this.groupBoxTool.Name = "groupBoxTool";
            this.groupBoxTool.Size = new System.Drawing.Size(387, 100);
            this.groupBoxTool.TabIndex = 4;
            this.groupBoxTool.TabStop = false;
            this.groupBoxTool.Text = "Tool";
            // 
            // labelToolFlutes
            // 
            this.labelToolFlutes.AutoSize = true;
            this.labelToolFlutes.Location = new System.Drawing.Point(25, 77);
            this.labelToolFlutes.Name = "labelToolFlutes";
            this.labelToolFlutes.Size = new System.Drawing.Size(35, 13);
            this.labelToolFlutes.TabIndex = 10;
            this.labelToolFlutes.Text = "Flutes";
            // 
            // labelToolCuttingDiameter
            // 
            this.labelToolCuttingDiameter.AutoSize = true;
            this.labelToolCuttingDiameter.Location = new System.Drawing.Point(25, 50);
            this.labelToolCuttingDiameter.Name = "labelToolCuttingDiameter";
            this.labelToolCuttingDiameter.Size = new System.Drawing.Size(49, 13);
            this.labelToolCuttingDiameter.TabIndex = 6;
            this.labelToolCuttingDiameter.Text = "Diameter";
            // 
            // labelToolCuttingDepth
            // 
            this.labelToolCuttingDepth.AutoSize = true;
            this.labelToolCuttingDepth.Location = new System.Drawing.Point(25, 24);
            this.labelToolCuttingDepth.Name = "labelToolCuttingDepth";
            this.labelToolCuttingDepth.Size = new System.Drawing.Size(70, 13);
            this.labelToolCuttingDepth.TabIndex = 2;
            this.labelToolCuttingDepth.Text = "Cutting depth";
            // 
            // toolStripLists
            // 
            this.toolStripLists.AllowItemReorder = true;
            this.toolStripLists.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStripLists.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabelTools,
            this.toolStripComboBoxTools,
            this.toolStripLabelMaterial,
            this.toolStripComboBoxMaterials});
            this.toolStripLists.Location = new System.Drawing.Point(3, 0);
            this.toolStripLists.Name = "toolStripLists";
            this.toolStripLists.Size = new System.Drawing.Size(421, 25);
            this.toolStripLists.TabIndex = 0;
            // 
            // toolStripLabelTools
            // 
            this.toolStripLabelTools.Name = "toolStripLabelTools";
            this.toolStripLabelTools.Size = new System.Drawing.Size(31, 22);
            this.toolStripLabelTools.Text = "tool:";
            // 
            // toolStripComboBoxTools
            // 
            this.toolStripComboBoxTools.Name = "toolStripComboBoxTools";
            this.toolStripComboBoxTools.Size = new System.Drawing.Size(200, 25);
            this.toolStripComboBoxTools.SelectedIndexChanged += new System.EventHandler(this.ToolStripComboBoxTools_SelectedIndexChanged);
            // 
            // toolStripLabelMaterial
            // 
            this.toolStripLabelMaterial.Name = "toolStripLabelMaterial";
            this.toolStripLabelMaterial.Size = new System.Drawing.Size(53, 22);
            this.toolStripLabelMaterial.Text = "material:";
            // 
            // toolStripComboBoxMaterials
            // 
            this.toolStripComboBoxMaterials.Name = "toolStripComboBoxMaterials";
            this.toolStripComboBoxMaterials.Size = new System.Drawing.Size(121, 25);
            this.toolStripComboBoxMaterials.SelectedIndexChanged += new System.EventHandler(this.ToolStripComboBoxMaterials_SelectedIndexChanged);
            // 
            // scaledTypeResultFeed
            // 
            this.scaledTypeResultFeed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scaledTypeResultFeed.DataSource = ((object)(resources.GetObject("scaledTypeResultFeed.DataSource")));
            this.scaledTypeResultFeed.DataSourceLocked = true;
            this.scaledTypeResultFeed.DisplayMember = "";
            this.scaledTypeResultFeed.Label = "Vf";
            this.scaledTypeResultFeed.Location = new System.Drawing.Point(150, 70);
            this.scaledTypeResultFeed.Name = "scaledTypeResultFeed";
            this.scaledTypeResultFeed.ScaleEnabled = true;
            this.scaledTypeResultFeed.SelectedIndex = 0;
            this.scaledTypeResultFeed.SelectedScale = "[#]";
            this.scaledTypeResultFeed.SelectedValue = "0";
            this.scaledTypeResultFeed.Size = new System.Drawing.Size(231, 27);
            this.scaledTypeResultFeed.TabIndex = 16;
            this.scaledTypeResultFeed.ValueConvert = false;
            this.scaledTypeResultFeed.ValueMember = "";
            this.scaledTypeResultFeed.ValueReadonly = false;
            this.scaledTypeResultFeed.OnScaleRejected += new As.Tools.Controls.ScaledTypeControl.ScaleRejectedEventHandler(this.ScaledTypeControl1_OnScaleRejected);
            // 
            // scaledTypeResultSpindleSpeed
            // 
            this.scaledTypeResultSpindleSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scaledTypeResultSpindleSpeed.DataSource = ((object)(resources.GetObject("scaledTypeResultSpindleSpeed.DataSource")));
            this.scaledTypeResultSpindleSpeed.DataSourceLocked = true;
            this.scaledTypeResultSpindleSpeed.DisplayMember = "";
            this.scaledTypeResultSpindleSpeed.Label = "n";
            this.scaledTypeResultSpindleSpeed.Location = new System.Drawing.Point(150, 41);
            this.scaledTypeResultSpindleSpeed.Name = "scaledTypeResultSpindleSpeed";
            this.scaledTypeResultSpindleSpeed.ScaleEnabled = true;
            this.scaledTypeResultSpindleSpeed.SelectedIndex = 0;
            this.scaledTypeResultSpindleSpeed.SelectedScale = "[#]";
            this.scaledTypeResultSpindleSpeed.SelectedValue = "0";
            this.scaledTypeResultSpindleSpeed.Size = new System.Drawing.Size(231, 27);
            this.scaledTypeResultSpindleSpeed.TabIndex = 17;
            this.scaledTypeResultSpindleSpeed.ValueConvert = false;
            this.scaledTypeResultSpindleSpeed.ValueMember = "";
            this.scaledTypeResultSpindleSpeed.ValueReadonly = false;
            // 
            // scaledTypeResultCuttingSpeed
            // 
            this.scaledTypeResultCuttingSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scaledTypeResultCuttingSpeed.DataSource = ((object)(resources.GetObject("scaledTypeResultCuttingSpeed.DataSource")));
            this.scaledTypeResultCuttingSpeed.DataSourceLocked = true;
            this.scaledTypeResultCuttingSpeed.DisplayMember = "";
            this.scaledTypeResultCuttingSpeed.Label = "Vc";
            this.scaledTypeResultCuttingSpeed.Location = new System.Drawing.Point(150, 14);
            this.scaledTypeResultCuttingSpeed.Name = "scaledTypeResultCuttingSpeed";
            this.scaledTypeResultCuttingSpeed.ScaleEnabled = true;
            this.scaledTypeResultCuttingSpeed.SelectedIndex = 0;
            this.scaledTypeResultCuttingSpeed.SelectedScale = "[#]";
            this.scaledTypeResultCuttingSpeed.SelectedValue = "0";
            this.scaledTypeResultCuttingSpeed.Size = new System.Drawing.Size(231, 27);
            this.scaledTypeResultCuttingSpeed.TabIndex = 17;
            this.scaledTypeResultCuttingSpeed.ValueConvert = false;
            this.scaledTypeResultCuttingSpeed.ValueMember = "";
            this.scaledTypeResultCuttingSpeed.ValueReadonly = false;
            // 
            // scaledTypeMaterialFeedPerTooth
            // 
            this.scaledTypeMaterialFeedPerTooth.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scaledTypeMaterialFeedPerTooth.DataSource = ((object)(resources.GetObject("scaledTypeMaterialFeedPerTooth.DataSource")));
            this.scaledTypeMaterialFeedPerTooth.DataSourceLocked = true;
            this.scaledTypeMaterialFeedPerTooth.DisplayMember = "";
            this.scaledTypeMaterialFeedPerTooth.Label = "fz";
            this.scaledTypeMaterialFeedPerTooth.Location = new System.Drawing.Point(150, 70);
            this.scaledTypeMaterialFeedPerTooth.Name = "scaledTypeMaterialFeedPerTooth";
            this.scaledTypeMaterialFeedPerTooth.ScaleEnabled = true;
            this.scaledTypeMaterialFeedPerTooth.SelectedIndex = 0;
            this.scaledTypeMaterialFeedPerTooth.SelectedScale = "[#]";
            this.scaledTypeMaterialFeedPerTooth.SelectedValue = "0";
            this.scaledTypeMaterialFeedPerTooth.Size = new System.Drawing.Size(231, 27);
            this.scaledTypeMaterialFeedPerTooth.TabIndex = 6;
            this.scaledTypeMaterialFeedPerTooth.ValueConvert = false;
            this.scaledTypeMaterialFeedPerTooth.ValueMember = "";
            this.scaledTypeMaterialFeedPerTooth.ValueReadonly = false;
            // 
            // scaledTypeMaterialSpindleSpeed
            // 
            this.scaledTypeMaterialSpindleSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scaledTypeMaterialSpindleSpeed.DataSource = ((object)(resources.GetObject("scaledTypeMaterialSpindleSpeed.DataSource")));
            this.scaledTypeMaterialSpindleSpeed.DataSourceLocked = true;
            this.scaledTypeMaterialSpindleSpeed.DisplayMember = "";
            this.scaledTypeMaterialSpindleSpeed.Label = "n";
            this.scaledTypeMaterialSpindleSpeed.Location = new System.Drawing.Point(150, 41);
            this.scaledTypeMaterialSpindleSpeed.Name = "scaledTypeMaterialSpindleSpeed";
            this.scaledTypeMaterialSpindleSpeed.ScaleEnabled = true;
            this.scaledTypeMaterialSpindleSpeed.SelectedIndex = 0;
            this.scaledTypeMaterialSpindleSpeed.SelectedScale = "[#]";
            this.scaledTypeMaterialSpindleSpeed.SelectedValue = "0";
            this.scaledTypeMaterialSpindleSpeed.Size = new System.Drawing.Size(231, 27);
            this.scaledTypeMaterialSpindleSpeed.TabIndex = 5;
            this.scaledTypeMaterialSpindleSpeed.ValueConvert = false;
            this.scaledTypeMaterialSpindleSpeed.ValueMember = "";
            this.scaledTypeMaterialSpindleSpeed.ValueReadonly = false;
            // 
            // scaledTypeMaterialCuttingSpeed
            // 
            this.scaledTypeMaterialCuttingSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scaledTypeMaterialCuttingSpeed.DataSource = ((object)(resources.GetObject("scaledTypeMaterialCuttingSpeed.DataSource")));
            this.scaledTypeMaterialCuttingSpeed.DataSourceLocked = true;
            this.scaledTypeMaterialCuttingSpeed.DisplayMember = "";
            this.scaledTypeMaterialCuttingSpeed.Label = "Vc";
            this.scaledTypeMaterialCuttingSpeed.Location = new System.Drawing.Point(150, 14);
            this.scaledTypeMaterialCuttingSpeed.Name = "scaledTypeMaterialCuttingSpeed";
            this.scaledTypeMaterialCuttingSpeed.ScaleEnabled = true;
            this.scaledTypeMaterialCuttingSpeed.SelectedIndex = 0;
            this.scaledTypeMaterialCuttingSpeed.SelectedScale = "[#]";
            this.scaledTypeMaterialCuttingSpeed.SelectedValue = "0";
            this.scaledTypeMaterialCuttingSpeed.Size = new System.Drawing.Size(231, 27);
            this.scaledTypeMaterialCuttingSpeed.TabIndex = 4;
            this.scaledTypeMaterialCuttingSpeed.ValueConvert = false;
            this.scaledTypeMaterialCuttingSpeed.ValueMember = "";
            this.scaledTypeMaterialCuttingSpeed.ValueReadonly = false;
            // 
            // scaledTypeToolFlutes
            // 
            this.scaledTypeToolFlutes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scaledTypeToolFlutes.DataSource = ((object)(resources.GetObject("scaledTypeToolFlutes.DataSource")));
            this.scaledTypeToolFlutes.DataSourceLocked = true;
            this.scaledTypeToolFlutes.DisplayMember = "";
            this.scaledTypeToolFlutes.Label = "Z";
            this.scaledTypeToolFlutes.Location = new System.Drawing.Point(150, 67);
            this.scaledTypeToolFlutes.Name = "scaledTypeToolFlutes";
            this.scaledTypeToolFlutes.ScaleEnabled = true;
            this.scaledTypeToolFlutes.SelectedIndex = 0;
            this.scaledTypeToolFlutes.SelectedScale = "[#]";
            this.scaledTypeToolFlutes.SelectedValue = "0";
            this.scaledTypeToolFlutes.Size = new System.Drawing.Size(231, 27);
            this.scaledTypeToolFlutes.TabIndex = 3;
            this.scaledTypeToolFlutes.ValueConvert = false;
            this.scaledTypeToolFlutes.ValueMember = "";
            this.scaledTypeToolFlutes.ValueReadonly = false;
            // 
            // scaledTypeToolCuttingDiameter
            // 
            this.scaledTypeToolCuttingDiameter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scaledTypeToolCuttingDiameter.DataSource = ((object)(resources.GetObject("scaledTypeToolCuttingDiameter.DataSource")));
            this.scaledTypeToolCuttingDiameter.DataSourceLocked = true;
            this.scaledTypeToolCuttingDiameter.DisplayMember = "";
            this.scaledTypeToolCuttingDiameter.Label = "D";
            this.scaledTypeToolCuttingDiameter.Location = new System.Drawing.Point(150, 41);
            this.scaledTypeToolCuttingDiameter.Name = "scaledTypeToolCuttingDiameter";
            this.scaledTypeToolCuttingDiameter.ScaleEnabled = true;
            this.scaledTypeToolCuttingDiameter.SelectedIndex = 0;
            this.scaledTypeToolCuttingDiameter.SelectedScale = "[#]";
            this.scaledTypeToolCuttingDiameter.SelectedValue = "0";
            this.scaledTypeToolCuttingDiameter.Size = new System.Drawing.Size(231, 27);
            this.scaledTypeToolCuttingDiameter.TabIndex = 2;
            this.scaledTypeToolCuttingDiameter.ValueConvert = false;
            this.scaledTypeToolCuttingDiameter.ValueMember = "";
            this.scaledTypeToolCuttingDiameter.ValueReadonly = false;
            // 
            // scaledTypeToolCuttingDepth
            // 
            this.scaledTypeToolCuttingDepth.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scaledTypeToolCuttingDepth.DataSource = ((object)(resources.GetObject("scaledTypeToolCuttingDepth.DataSource")));
            this.scaledTypeToolCuttingDepth.DataSourceLocked = true;
            this.scaledTypeToolCuttingDepth.DisplayMember = "";
            this.scaledTypeToolCuttingDepth.Label = "h";
            this.scaledTypeToolCuttingDepth.Location = new System.Drawing.Point(150, 14);
            this.scaledTypeToolCuttingDepth.Name = "scaledTypeToolCuttingDepth";
            this.scaledTypeToolCuttingDepth.ScaleEnabled = true;
            this.scaledTypeToolCuttingDepth.SelectedIndex = 0;
            this.scaledTypeToolCuttingDepth.SelectedScale = "[#]";
            this.scaledTypeToolCuttingDepth.SelectedValue = "0";
            this.scaledTypeToolCuttingDepth.Size = new System.Drawing.Size(231, 27);
            this.scaledTypeToolCuttingDepth.TabIndex = 1;
            this.scaledTypeToolCuttingDepth.ValueConvert = false;
            this.scaledTypeToolCuttingDepth.ValueMember = "";
            this.scaledTypeToolCuttingDepth.ValueReadonly = false;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(442, 409);
            this.Controls.Add(this.toolStripContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Main";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBoxResults.ResumeLayout(false);
            this.groupBoxResults.PerformLayout();
            this.groupBoxMaterial.ResumeLayout(false);
            this.groupBoxMaterial.PerformLayout();
            this.groupBoxTool.ResumeLayout(false);
            this.groupBoxTool.PerformLayout();
            this.toolStripLists.ResumeLayout(false);
            this.toolStripLists.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBoxMaterial;
        private System.Windows.Forms.RadioButton radioButtonMaterialSpindleSpeed;
        private System.Windows.Forms.RadioButton radioButtonMaterialCuttingSpeed;
        private System.Windows.Forms.Label labelMaterialFeedPerTooth;
        private System.Windows.Forms.Label labelMaterialSpindleSpeed;
        private System.Windows.Forms.Label labelMaterialCuttingSpeed;
        private System.Windows.Forms.GroupBox groupBoxTool;
        private System.Windows.Forms.Label labelToolFlutes;
        private System.Windows.Forms.Label labelToolCuttingDiameter;
        private System.Windows.Forms.Label labelToolCuttingDepth;
        private System.Windows.Forms.GroupBox groupBoxResults;
        private System.Windows.Forms.Label labelResultFeed;
        private System.Windows.Forms.Label labelResultSpindleSpeed;
        private System.Windows.Forms.Label labelResultCuttingSpeed;
        private Tools.Controls.ScaledTypeControl scaledTypeResultFeed;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelWarning;
        private Tools.Controls.ScaledTypeControl scaledTypeToolCuttingDepth;
        private Tools.Controls.ScaledTypeControl scaledTypeToolCuttingDiameter;
        private Tools.Controls.ScaledTypeControl scaledTypeToolFlutes;
        private Tools.Controls.ScaledTypeControl scaledTypeMaterialCuttingSpeed;
        private Tools.Controls.ScaledTypeControl scaledTypeMaterialSpindleSpeed;
        private Tools.Controls.ScaledTypeControl scaledTypeMaterialFeedPerTooth;
        private Tools.Controls.ScaledTypeControl scaledTypeResultCuttingSpeed;
        private Tools.Controls.ScaledTypeControl scaledTypeResultSpindleSpeed;
        private System.Windows.Forms.ToolStrip toolStripLists;
        private System.Windows.Forms.ToolStripLabel toolStripLabelTools;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxTools;
        private System.Windows.Forms.ToolStripLabel toolStripLabelMaterial;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxMaterials;
        private System.Windows.Forms.ToolStripMenuItem filesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadMaterialsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
    }
}

