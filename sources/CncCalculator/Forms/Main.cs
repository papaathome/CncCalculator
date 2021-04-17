#if TRACE_EVENTS
#if !USE_LOG4NET
#define USE_LOG4NET
#endif
#endif

using System;
using System.Windows.Forms;
using System.Configuration;
using System.Drawing;

using As.Apps.Models;

namespace As.Apps.Forms
{
    public partial class Main : Form
    {
#if TRACE_EVENTS
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
#endif
        public const string L = "0, 0";
        const int L_X = 0;
        const int L_Y = 0;

        const int L_X1 = 10;
        const int L_Y1 = 10;

        public const string S = "458, 447";
        const int S_W = 458;
        const int S_H = 447;

        public Main()
        {
            InitializeComponent();
            InitializeApplication();

            Text = $"{Program.Name}, {Program.Version}";

            splitContainer1.Panel2Collapsed = true;
            splitContainer1.Panel2.Hide();
        }

        FormSettings settings = new FormSettings();

        private void Main_Load(object sender, EventArgs e)
        {
            ToolStripManager.LoadSettings(this); // managing toolStripLists

            if (settings.UpgradeNow)
            {
                settings.Upgrade();
                settings.UpgradeNow = false;
                settings.Save();

                // ToolStripManager does not provide Upgrade(), we do it here but for the side only.
                if (toolStripLists.Parent.Dock != settings.ToolStripDock)
                {
                    // relocate toolstrip.
                    switch (toolStripLists.Parent.Dock)
                    {
                        case DockStyle.Top: toolStripContainer1.TopToolStripPanel.Controls.Remove(toolStripLists); break;
                        case DockStyle.Right: toolStripContainer1.RightToolStripPanel.Controls.Remove(toolStripLists); break;
                        case DockStyle.Left: toolStripContainer1.LeftToolStripPanel.Controls.Remove(toolStripLists); break;
                        case DockStyle.Bottom: toolStripContainer1.BottomToolStripPanel.Controls.Remove(toolStripLists); break;
                    }
                    switch (settings.ToolStripDock)
                    {
                        default: goto case DockStyle.Top;
                        case DockStyle.Top: toolStripContainer1.TopToolStripPanel.Controls.Add(toolStripLists); break;
                        case DockStyle.Right: toolStripContainer1.RightToolStripPanel.Controls.Add(toolStripLists); break;
                        case DockStyle.Left: toolStripContainer1.LeftToolStripPanel.Controls.Add(toolStripLists); break;
                        case DockStyle.Bottom: toolStripContainer1.BottomToolStripPanel.Controls.Add(toolStripLists); break;
                    }
                }
            }

            // check if any part of the form is visible or not. e.g.: previous 'position' was 'collapsed', [-], which moves it out of sight.
            // Note: the form 'maximised' moves it to (-8,-8).
            var l = settings.FormLocation;
            var s = settings.FormSize;
            if ((l.X < 0) && (-l.X >= s.Width) || (l.Y < 0) && (-l.Y >= s.Height))
            {
                l = new Point(L_X1, L_Y1);
                s = new Size(S_W, S_H);
            }

            // check that the window is in a visible part on any screen.
            //var screens = Screen.AllScreens;
            var screen = Screen.FromControl(this);
            var d = new Point(l.X + s.Width - screen.Bounds.Size.Width, l.Y + s.Height - screen.Bounds.Size.Height);
            var x = l.X;
            var y = l.Y;
            var w = s.Width;
            var h = s.Height;
            if (0 < d.X)
            {
                // translate over x
                x -= d.X;
                if (x < 0)
                {
                    // relocate x and shrink over w
                    w += x;
                    x = 0;
                }
            }

            if (0 < d.Y)
            {
                // translate over y
                y -= d.Y;
                if (y < 0)
                {
                    // relocate y and shrink over h
                    h += y;
                    y = 0;
                }
            }

            if ((settings.FormLocation.X != x) || (settings.FormLocation.Y != y)) settings.FormLocation = new Point(x, y);
            if ((settings.FormSize.Width != w) || (settings.FormSize.Height != h)) settings.FormSize = new Size(w, h);

            Binding bndLocation = new Binding("Location", settings, "FormLocation", true, DataSourceUpdateMode.OnPropertyChanged);
            DataBindings.Add(bndLocation);
            Size = settings.FormSize; // Assign Size property, since databinding to Size doesn't work well.
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            ToolStripManager.SaveSettings(this);
            settings.FormSize = Size; // Preserve Size property, since databinding to Size doesn't work well.
            settings.ToolStripDock = toolStripLists.Parent.Dock;
            settings.Save();
        }

        private void LoadMaterialsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var f = new OpenFileDialog
            {
                InitialDirectory = c.MaterialsPath,
                Filter = c.MaterialsFilter,
                FilterIndex = 1,
                RestoreDirectory = true
            };
            if (f.ShowDialog(this) == DialogResult.OK)
            {
                c.LoadMaterials(f.FileName);
            }
        }

        private void LoadToolsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var f = new OpenFileDialog
            {
                InitialDirectory = c.ToolsPath,
                Filter = c.ToolsFilter,
                FilterIndex = 1,
                RestoreDirectory = true
            };
            if (f.ShowDialog(this) == DialogResult.OK)
            {
                c.LoadTools(f.FileName);
            }
        }

        private void QuitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var a = new About();
            a.ShowDialog(this);
        }

        #region Application
        void InitializeApplication()
        {
            c = new Calculator();

            // bind tools.
            toolStripComboBoxTools.ComboBox.DropDownStyle = ComboBoxStyle.DropDown;
            C_OnToolsChanged(this, EventArgs.Empty);
            c.OnToolsChanged += C_OnToolsChanged;

            // bind materials
            toolStripComboBoxMaterials.ComboBox.DropDownStyle = ComboBoxStyle.DropDown;
            C_OnMaterialsChanged(this, EventArgs.Empty);
            c.OnMaterialsChanged += C_OnMaterialsChanged;

            // bind tool cutting depth h
            scaledTypeToolCuttingDepth.DataSource = Calculator.Tool.h_domain;
            scaledTypeToolCuttingDepth.ScaledValue = c.tool.h;

            // bind tool cutting diameter D
            scaledTypeToolCuttingDiameter.DataSource = Calculator.Tool.D_domain;
            scaledTypeToolCuttingDiameter.ScaledValue = c.tool.D;

            // bind tool flutes Z
            scaledTypeToolFlutes.DataSource = Calculator.Tool.Z_domain;
            scaledTypeToolFlutes.ScaledValue = c.tool.Z;

            // bind material cutting speed Vc
            scaledTypeMaterialCuttingSpeed.DataSource = Calculator.Material.Vc_domain;
            scaledTypeMaterialCuttingSpeed.ScaledValue = c.material.Vc;

            // bind material use of cutting speed Vc
            Material_OnVcEnabledChanged(this, EventArgs.Empty);
            c.material.OnVcEnabledChanged += Material_OnVcEnabledChanged;

            // bind material spindle speed n
            scaledTypeMaterialSpindleSpeed.DataSource = Calculator.Material.n_domain;
            scaledTypeMaterialSpindleSpeed.ScaledValue = c.material.n;

            // bind material use of spindle speed n
            Material_OnNEnabledChanged(this, EventArgs.Empty);
            c.material.OnNEnabledChanged += Material_OnNEnabledChanged;

            // bind materialfeed per tooth fz
            scaledTypeMaterialFeedPerTooth.DataSource = Calculator.Material.fz_domain;
            scaledTypeMaterialFeedPerTooth.ScaledValue = c.material.fz;

            // bind material feeds and speeds cutting speed Vc
            scaledTypeResultCuttingSpeed.DataSource = Calculator.Vc_domain;
            scaledTypeResultCuttingSpeed.ScaledValue = c.Vc;
            scaledTypeResultCuttingSpeed.ValueConvert = c.Vc_calculated;
            scaledTypeResultCuttingSpeed.ValueReadonly = c.Vc_calculated;

            // bind material feeds and speeds spindle speed n
            scaledTypeResultSpindleSpeed.DataSource = Calculator.n_domain;
            scaledTypeResultSpindleSpeed.ScaledValue = c.n;
            scaledTypeResultSpindleSpeed.ValueConvert = c.n_calculated;
            scaledTypeResultSpindleSpeed.ValueReadonly = c.n_calculated;

            // bind material feeds and speeds Feed Vf
            scaledTypeResultFeed.DataSource = Calculator.Vf_domain;
            scaledTypeResultFeed.ScaledValue = c.Vf;
            scaledTypeResultFeed.ValueConvert = c.Vf_calculated;
            scaledTypeResultFeed.ValueReadonly = c.Vf_calculated;
        }

        private void C_OnToolsChanged(object sender, EventArgs e)
        {
            toolStripComboBoxTools.ComboBox.DataSource = null;
            toolStripComboBoxTools.ComboBox.DataSource = new BindingSource(c.Tools.Values, null);
            toolStripComboBoxTools.ComboBox.DisplayMember = "Name";
            toolStripComboBoxTools.ComboBox.ValueMember = "Parameter";
        }

        private void C_OnMaterialsChanged(object sender, EventArgs e)
        {
            toolStripComboBoxMaterials.ComboBox.DataSource = null;
            toolStripComboBoxMaterials.ComboBox.DataSource = new BindingSource(c.Materials, null);
            toolStripComboBoxMaterials.ComboBox.DisplayMember = "Key";
            toolStripComboBoxMaterials.ComboBox.ValueMember = "Value";
        }

        private void ToolStripComboBoxTools_SelectedIndexChanged(object sender, EventArgs e)
        {
#if TRACE_EVENTS
            log.Debug($"main: toolStripComboBoxTools_SelectedIndexChanged({toolStripComboBoxTools.SelectedIndex})");
#endif
            c.SelectTool(toolStripComboBoxTools.SelectedItem);
        }

        private void ToolStripComboBoxMaterials_SelectedIndexChanged(object sender, EventArgs e)
        {
#if TRACE_EVENTS
            log.Debug($"main: toolStripComboBoxMaterials_SelectedIndexChanged({toolStripComboBoxMaterials.SelectedIndex})");
#endif
            c.SelectMaterial(toolStripComboBoxMaterials.SelectedItem);
        }

        private void Material_OnVcEnabledChanged(object sender, EventArgs e)
        {
#if TRACE_EVENTS
            log.Debug($"main: Material_OnVcEnabledChanged({c.material.VcEnabled})");
#endif
            radioButtonMaterialCuttingSpeed.Checked = c.material.VcEnabled;
            scaledTypeMaterialCuttingSpeed.ValueConvert = !c.material.VcEnabled;
            scaledTypeMaterialCuttingSpeed.ValueReadonly = !c.material.VcEnabled;
        }

        private void Material_OnNEnabledChanged(object sender, EventArgs e)
        {
#if TRACE_EVENTS
            log.Debug($"main: Material_OnNEnabledChanged({c.material.NEnabled})");
#endif
            radioButtonMaterialSpindleSpeed.Checked = c.material.NEnabled;
            scaledTypeMaterialSpindleSpeed.ValueConvert = !c.material.NEnabled;
            scaledTypeMaterialSpindleSpeed.ValueReadonly = !c.material.NEnabled;
        }

        Calculator c;

        private void RadioButtonMaterialCuttingSpeed_CheckedChanged(object sender, EventArgs e)
        {
#if TRACE_EVENTS
            log.Debug($"main: radioButtonMaterialCuttingSpeed_CheckedChanged({radioButtonMaterialCuttingSpeed.Checked})");
#endif
            c.material.VcEnabled = radioButtonMaterialCuttingSpeed.Checked;
        }

        private void RadioButtonMaterialSpindleSpeed_CheckedChanged(object sender, EventArgs e)
        {
#if TRACE_EVENTS
            log.Debug($"main: radioButtonMaterialSpindleSpeed_CheckedChanged({radioButtonMaterialSpindleSpeed.Checked})");
#endif
            c.material.NEnabled = radioButtonMaterialSpindleSpeed.Checked;
        }

        private void ScaledTypeControl1_OnScaleRejected(object sender, Tools.Controls.ScaleRejectedEventArgs e)
        {
#if TRACE_EVENTS
            log.Debug($"main: scaledTypeControl1_OnScaleRejected({e.Text})");
#endif
            toolStripStatusLabelWarning.Text = $"Warning: {e.Text}";
        }
        #endregion
    }

    sealed class FormSettings : ApplicationSettingsBase
    {
        [UserScopedSetting()]
        [DefaultSettingValue("true")]
        public bool UpgradeNow
        {
            get { return (bool)this["UpgradeNow"]; }
            set { this["UpgradeNow"] = value; }
        }

        [UserScopedSetting()]
        [DefaultSettingValue(Main.L)]
        public Point FormLocation
        {
            get { return (Point)this["FormLocation"]; }
            set { this["FormLocation"] = value; }
        }

        [UserScopedSetting()]
        [DefaultSettingValue(Main.S)]
        public Size FormSize
        {
            get { return (Size)this["FormSize"]; }
            set { this["FormSize"] = value; }
        }

        [UserScopedSetting()]
        [DefaultSettingValue("Top")]
        public DockStyle ToolStripDock
        {
            get { return (DockStyle)this["ToolStripDock"]; }
            set { this["ToolStripDock"] = value; }
        }
    }
}
