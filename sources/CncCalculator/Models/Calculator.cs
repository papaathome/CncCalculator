#if TRACE_EVENTS
#if !USE_LOG4NET
#define USE_LOG4NET
#endif
#endif

using System;
using System.Collections.ObjectModel;

using As.Apps.Data;
using As.Tools.Data.Scales;
using System.IO;
using System.Configuration;

namespace As.Apps.Models
{
    sealed class CalculatorSettings : ApplicationSettingsBase
    {
        [UserScopedSetting()]
        [DefaultSettingValue(null)]
        public string Version
        {
            get { return (string)(this["Version"]); }
            set { this["Version"] = value; }
        }

        [UserScopedSetting()]
        [DefaultSettingValue(@"data\Library\tools.fctl")]
        public string ToolsPath
        {
            get { return (string)(this["ToolsPath"]); }
            set { this["ToolsPath"] = value; }
        }

        [UserScopedSetting()]
        [DefaultSettingValue(@"data\Library\materials.json")]
        public string MaterialsPath
        {
            get { return (string)(this["MaterialsPath"]); }
            set { this["MaterialsPath"] = value; }
        }
    }

    public class Calculator
    {
#if USE_LOG4NET
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
#endif

        public class Tool
        {
            /// <summary>
            /// Tool: information about the tool
            /// </summary>
            public Tool()
            {
                h = new DoubleST(1.0, "[mm]");
                D = new DoubleST(1.0, "[mm]");
                Z = new LongST(1, "[tooth]");
            }

            #region h
            /// <summary>
            /// Cutting depth domain
            /// </summary>
            public static readonly ReadOnlyCollection<string> h_domain = Calculator.Lengths;

            /// <summary>
            /// Cutting depth
            /// </summary>
            public ScaledType h { get; set; }

            /// <summary>
            /// Flag indicating if h is calulated or not
            /// </summary>
            public bool h_calculated { get { return false; } }
            #endregion

            #region D
            /// <summary>
            /// Diameter domain
            /// </summary>
            public static readonly ReadOnlyCollection<string> D_domain = Calculator.Lengths;

            /// <summary>
            /// Diameter
            /// </summary>
            public ScaledType D { get; set; }

            /// <summary>
            /// Flag indicating if D is calulated or not
            /// </summary>
            public bool D_calculated { get { return false; } }
            #endregion

            #region Z
            /// <summary>
            /// Flutes domain
            /// </summary>
            public static readonly ReadOnlyCollection<string> Z_domain = Calculator.Counts;

            /// <summary>
            /// Flutes
            /// </summary>
            public ScaledType Z { get; set; }

            /// <summary>
            /// Flag indicating if Z is calulated or not
            /// </summary>
            public bool Z_calculated { get { return false; } }
            #endregion
        }

        public class Material
        {
#if TRACE_EVENTS
            static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
#endif

            /// <summary>
            /// Information about the material
            /// </summary>
            /// <param name="tool">Tool to use on the material</param>
            public Material(Tool tool)
            {
                this.tool = tool;

                Vc = new DoubleST(1.0, "[mm/s]");
                n = new DoubleST(1.0, "[rpm]");
                fz = new DoubleST(1.0, "[mm/tooth]");

                this.tool.D.OnChanged += tool_D_OnChanged;

                Vc.OnChanged += Vc_OnChanged;
                n.OnChanged += n_OnChanged;
            }

            #region Tool
            /// <summary>
            /// tool for reference of Tool diameter.
            /// </summary>
            private Tool tool;

            /// <summary>
            /// Tool diameter changed.
            /// </summary>
            /// <param name="sender">Tool that has changed.</param>
            /// <param name="e">Change attributes.</param>
            private void tool_D_OnChanged(object sender, EventArgs e)
            {
#if TRACE_EVENTS
                log.Debug($"Calculator.Material: tool_D_OnChanged()");
#endif
                if (UseVc_notN) Calculate_n();
                else Calculate_Vc();
            }
            #endregion

            #region Use Vc and not n.
            /// <summary>
            /// select use of Vc or use of n
            /// </summary>
            bool UseVc_notN
            {
                get { return _use_vc_not_n; }
                set
                {
                    if (_use_vc_not_n != value)
                    {
                        _use_vc_not_n = value;
                        SetVcEnabled();
                        SetNEnabled();
                    }
                }
            }

            /// <summary>
            /// Container for UseVc_notN
            /// </summary>
            bool _use_vc_not_n = true;
            #endregion

            #region Vc
            /// <summary>
            /// Cutting speed domain
            /// </summary>
            public static readonly ReadOnlyCollection<string> Vc_domain = Calculator.Speeds;

            /// <summary>
            /// Event on change of use of Vc for calculations.
            /// </summary>
            public event EventHandler OnVcEnabledChanged;

            /// <summary>
            /// True if Vc is used for calculations.
            /// </summary>
            public bool VcEnabled
            {
                get { return UseVc_notN; }
                set { UseVc_notN = value; }
            }

            /// <summary>
            /// Set the use of Vc.
            /// </summary>
            private void SetVcEnabled()
            {
                if (!VcEnabled) Calculate_Vc();
                OnVcEnabledChanged?.Invoke(this, EventArgs.Empty);
            }

            /// <summary>
            /// Cutting speed
            /// </summary>
            public ScaledType Vc { get; set; }

            /// <summary>
            /// Cutting speed changed.
            /// </summary>
            /// <param name="sender">Material manager.</param>
            /// <param name="e">Change attributes.</param>
            private void Vc_OnChanged(object sender, EventArgs e)
            {
#if TRACE_EVENTS
                log.Debug($"Calculator.Material: Vc_OnChanged()");
#endif
                if (UseVc_notN) Calculate_n();
            }

            /// <summary>
            /// Cutting speed calculation attribute.
            /// </summary>
            public bool Vc_calculated { get { return false; } }

            /// <summary>
            /// Recalculate Vc value.
            /// </summary>
            void Calculate_Vc()
            {
                // calculate Vc from n and tool D
                // Vc = pi D n

                // n from rotations to time domain. (not yet done automatic)
                var v1_3 = new DoubleST(1, "[1/s]") * (DoubleST)n / n.BaseNormal();
                var v1 = Math.PI * (DoubleST)tool.D * v1_3;
#if TRACE_EVENTS
                log.Debug($"Calculator.Material: Vc({v1})");
#endif
                Vc.SetValueScaled(v1);
            }
            #endregion

            #region n
            /// <summary>
            /// Spindle speed domain
            /// </summary>
            public static readonly ReadOnlyCollection<string> n_domain = Calculator.Rotations;

            /// <summary>
            /// Event on change of the use of N for calculations.
            /// </summary>
            public event EventHandler OnNEnabledChanged;

            /// <summary>
            /// True if N is used for calculations.
            /// </summary>
            public bool NEnabled {
                get { return !UseVc_notN; }
                set { UseVc_notN = !value; }
            }

            /// <summary>
            /// Set the use of N.
            /// </summary>
            private void SetNEnabled()
            {
                if (!NEnabled) Calculate_n();
                OnNEnabledChanged?.Invoke(this, EventArgs.Empty);
            }

            /// <summary>
            /// Spindle speed
            /// </summary>
            public ScaledType n { get; set; }

            /// <summary>
            /// Spindle rate changed.
            /// </summary>
            /// <param name="sender">Material manager.</param>
            /// <param name="e">Change attributs.</param>
            private void n_OnChanged(object sender, EventArgs e)
            {
#if TRACE_EVENTS
                log.Debug($"Calculator.Material: n_OnChanged()");
#endif
                if (NEnabled) Calculate_Vc();
            }

            /// <summary>
            /// Spindle speed calculation attribute.
            /// </summary>
            public bool n_calculated { get { return false; } }

            /// <summary>
            /// Recalculate spindle speed.
            /// </summary>
            void Calculate_n()
            {
                // calculate n from Vc and tool D
                // n = Vc / (pi D)
                var v1 = (DoubleST)Vc;
                var v2 = Math.PI * (DoubleST)tool.D;

                // n from time to rotations domain. (not yet done automatic)
                var v3 = new DoubleST(1, "[rps s]") * v1 / v2;
#if TRACE_EVENTS
                log.Debug($"Calculator.Material: n({v3})");
#endif
                n.SetValueScaled(v3);
            }
            #endregion

            #region fz
            /// <summary>
            /// Feed per tooth domain
            /// </summary>
            public static readonly ReadOnlyCollection<string> fz_domain = Calculator.LengthsOverCounts;

            /// <summary>
            /// Feed per tooth
            /// </summary>
            public ScaledType fz { get; set; }

            /// <summary>
            /// Feed per tooth calculation attribute.
            /// </summary>
            public bool fz_calculated { get { return false; } }
            #endregion
        }

        #region Domains
        /// <summary>
        /// Lengths.
        /// </summary>
        public static readonly ReadOnlyCollection<string> Lengths = new ReadOnlyCollection<string>(
            new string[] { "[mm]", "[cm]", "[in]", "[ft]" }
        );

        /// <summary>
        /// Counts.
        /// </summary>
        public static readonly ReadOnlyCollection<string> Counts = new ReadOnlyCollection<string>(
            new string[] { "[tooth]" }
        );

        /// <summary>
        /// Lengths over counts.
        /// </summary>
        public static readonly ReadOnlyCollection<string> LengthsOverCounts = new ReadOnlyCollection<string>(
            new string[] { "[mm/tooth]", "[in/tooth]" }
        );

        /// <summary>
        /// Speeds (or lengths over times)
        /// </summary>
        public static readonly ReadOnlyCollection<string> Speeds = new ReadOnlyCollection<string>(
          new string[] { "[mm/s]", "[mm/min]", "[cm/min]", "[in/s]", "[in/min]", "[ft/min]" }
        );

        /// <summary>
        /// Rotations (or one over times)
        /// </summary>
        public static readonly ReadOnlyCollection<string> Rotations = new ReadOnlyCollection<string>(
            new string[] { "[rps]", "[rpm]" }
        );
        #endregion

        /// <summary>
        /// Calculator: Do the maths for CNC Feeds and Speeds.
        /// </summary>
        public Calculator()
        {
            InitialiseSettings();
            InitialseToolsList();
            InitialseMaterialsList();

            tool = new Tool();
            material = new Material(tool);

            Vc = new DoubleST(1.0, "[mm/s]");
            n = new DoubleST(1.0, "[rpm]");
            Vf = new DoubleST(1.0, "[mm/s]");

            tool.D.OnChanged += tool_D_OnChanged;
            tool.Z.OnChanged += tool_Z_OnChanged;

            material.OnVcEnabledChanged += material_Vc_OnChanged;
            material.OnNEnabledChanged += material_n_OnChanged;

            material.Vc.OnChanged += material_Vc_OnChanged;
            material.n.OnChanged += material_n_OnChanged;
            material.fz.OnChanged += material_Fz_OnChanged;
        }

        /// <summary>
        /// .dtor: save changed settings on exit.
        /// </summary>
        ~Calculator() { settings?.Save(); }

        /// <summary>
        /// Calculator settings.
        /// </summary>
        CalculatorSettings settings = new CalculatorSettings();

        /// <summary>
        /// Initialise settings, upgrade if a new version is detected.
        /// </summary>
        void InitialiseSettings()
        {
            if (settings.Version == null)
            {
                settings.Upgrade();
#if USE_LOG4NET
                if (settings.Version != null) log.Info($"{Program.Name}: upgrade from v'{settings.Version}' to v'{Program.Version}'");
#endif
                settings.Version = $"{Program.Version}";
            }
        }

        #region Tools list
        /// <summary>
        /// Initialise tools list with the last used list.
        /// </summary>
        void InitialseToolsList()
        {
            //var tools = ToolsFc18.GetData(Properties.Settings.Default.Tools);
            //settings.ToolsPath = @"data\Library\tools1.fctl";
            tools = ToolsList.GetData(settings.ToolsPath);
            Tools = new ReadOnlyDictionary<string, Bit>(tools.Bits);
        }

        /// <summary>
        /// First directory to use in a files dialog for tool files.
        /// </summary>
        public string ToolsPath
        {
            get
            {
                var candidate = Path.GetDirectoryName(Path.GetFullPath(settings.ToolsPath));
                return (string.IsNullOrWhiteSpace(candidate))
                    ? Environment.GetFolderPath(Environment.SpecialFolder.Personal)
                    : candidate;
            }
        }

        /// <summary>
        /// Filter to use in a files dialog for tool files.
        /// </summary>
        public string ToolsFilter { get { return "tool files (*.fctl)|*.fctl|All files (*.*)|*.*"; } }

        /// <summary>
        /// Load new tools list
        /// </summary>
        /// <param name="path">Path to the new tools list</param>
        public void LoadTools(string path)
        {
            try
            {
                var t = ToolsList.GetData(path);
                if (t != null)
                {
                    tools = t;
                    Tools = new ReadOnlyDictionary<string, Bit>(tools.Bits);
                    settings.ToolsPath = path;
                    OnToolsChanged?.Invoke(this, EventArgs.Empty);
                }
            }
            catch { }
        }

        /// <summary>
        /// Event on changing of the tools list.
        /// </summary>
        public event EventHandler OnToolsChanged;

        /// <summary>
        /// Container for Tools.
        /// </summary>
        ToolsList tools;

        /// <summary>
        /// List of tools.
        /// </summary>
        public ReadOnlyDictionary<string, Bit> Tools;

        /// <summary>
        /// New tool selected from the list.
        /// </summary>
        /// <param name="selected">Selected tool</param>
        public void SelectTool(object selected)
        {
#if TRACE_EVENTS
            log.Debug($"Calculator.Material: SelectTool(bit: {(selected as Bit)})");
#endif

            var b = (selected as Bit);
            if (b == null) return;

            // set h?
            tool.D.SetValue(new DoubleST(1.0, "[mm]"));

            // set D.
            var Ds = b.Parameter.ContainsKey("Diameter") ? b.Parameter["Diameter"] : null;
            tool.D.SetValue(DoubleST.Scan(Ds, 1.0, "mm"));

            // set Z.
            var Zs = b.Parameter.ContainsKey("Flutes") ? b.Parameter["Flutes"] : null;
            tool.Z.SetValue(LongST.Scan(Zs, 1L, "tooth"));
        }
        #endregion

        #region Materials list
        /// <summary>
        /// Initialise material list with the last used list.
        /// </summary>
        void InitialseMaterialsList()
        {
            try { materials = MaterialList.GetData(settings.MaterialsPath); }
            catch { materials = new MaterialList(); }
            Materials = new ReadOnlyDictionary<string, Data.Material>(materials.Materials);
        }

        /// <summary>
        /// First directory to use in a files dialog for material files.
        /// </summary>
        public string MaterialsPath
        {
            get
            {
                var candidate = Path.GetDirectoryName(Path.GetFullPath(settings.MaterialsPath));
                return (string.IsNullOrWhiteSpace(candidate))
                    ? Environment.GetFolderPath(Environment.SpecialFolder.Personal)
                    : candidate;
            }
        }

        /// <summary>
        /// Filter to use in a files dialog for material files.
        /// </summary>
        public string MaterialsFilter { get { return "material files (*.json)|*.json|All files (*.*)|*.*"; } }

        /// <summary>
        /// Load new material list
        /// </summary>
        /// <param name="path">Path to the new materials list</param>
        public void LoadMaterials(string path)
        {
            try
            {
                var m = MaterialList.GetData(path);
                if (m != null)
                {
                    materials = m;
                    Materials = new ReadOnlyDictionary<string, Data.Material>(materials.Materials);
                    settings.MaterialsPath = path;
                    OnMaterialsChanged?.Invoke(this, EventArgs.Empty);
                }
            }
            catch { }
        }

        /// <summary>
        /// Event on changing of the materials list.
        /// </summary>
        public event EventHandler OnMaterialsChanged;

        /// <summary>
        /// Container for Materials.
        /// </summary>
        MaterialList materials;

        /// <summary>
        /// List of materials.
        /// </summary>
        public ReadOnlyDictionary<string, Data.Material> Materials;

        /// <summary>
        /// New material selected from the list.
        /// </summary>
        /// <param name="selected">New selected material</param>
        public void SelectMaterial(object selected)
        {
            Data.Material m = null;
            try
            {
                if (selected is System.Collections.Generic.KeyValuePair<string, Data.Material>)
                {
                    m = ((System.Collections.Generic.KeyValuePair<string, Data.Material>)selected).Value;
                }
            }
            catch
            {
                m = null;
            }
#if TRACE_EVENTS
            log.Debug($"Calculator.Material: SelectMaterial(material: {(m != null)})");
#endif
            if (m == null) return;

            // Set Use.
            if (m.UseCuttingSpeed)
            {
                // Set Vc.
                material.VcEnabled = true;
                material.Vc.SetValue(DoubleST.Scan(m.CuttingSpeed, 1.0, "mm/s"));
            }
            else
            {
                // Set n.
                material.NEnabled = true;
                material.n.SetValue(DoubleST.Scan(m.SpindleSpeed, 1.0, "rpm"));
            }

            // set fz.
            material.fz.SetValue(DoubleST.Scan(m.FeedPerTooth, 1.0, "mm/tooth"));
        }
        #endregion

        #region Tool
        /// <summary>
        /// Tool details used for calculations.
        /// </summary>
        public Tool tool { get; private set; }

        /// <summary>
        /// Toolmanager indicates a change in flutes.
        /// </summary>
        /// <param name="sender">Tool manager</param>
        /// <param name="e">Change attributes.</param>
        private void tool_Z_OnChanged(object sender, EventArgs e)
        {
#if TRACE_EVENTS
            log.Debug($"Calculator: tool_Z_OnChanged()");
#endif
            calculate_Vf();
        }

        /// <summary>
        /// Toolmanager indicates a change in diameter.
        /// </summary>
        /// <param name="sender">Tool manager</param>
        /// <param name="e">Change attributes.</param>
        private void tool_D_OnChanged(object sender, EventArgs e)
        {
#if TRACE_EVENTS
            log.Debug($"Calculator: tool_D_OnChanged()");
#endif
            calculate_Vf();
        }
        #endregion

        #region Material
        /// <summary>
        /// Material details used for calculations.
        /// </summary>
        public Material material { get; private set; }

        /// <summary>
        /// Material manager indicates a change in cutting speed.
        /// </summary>
        /// <param name="sender">Material manager.</param>
        /// <param name="e">Change attributes.</param>
        private void material_Vc_OnChanged(object sender, EventArgs e)
        {
#if TRACE_EVENTS
            log.Debug($"Calculator: material_Vc_OnChanged()");
#endif
            Calculate_Vc();
            if (material.VcEnabled) calculate_Vf();
        }

        /// <summary>
        /// Material manager inicates a change in spindle speed.
        /// </summary>
        /// <param name="sender">Material manager.</param>
        /// <param name="e">Change attributes.</param>
        private void material_n_OnChanged(object sender, EventArgs e)
        {
#if TRACE_EVENTS
            log.Debug($"Calculator: material_n_OnChanged()");
#endif
            calculate_n();
            if (material.NEnabled) Calculate_Vf_from_n();
        }

        /// <summary>
        /// Material manager indicates a change in cutting rate per tooth
        /// </summary>
        /// <param name="sender">Material manager.</param>
        /// <param name="e">Change attributes.</param>
        private void material_Fz_OnChanged(object sender, EventArgs e)
        {
#if TRACE_EVENTS
            log.Debug($"Calculator: material_Fz_OnChanged()");
#endif
            calculate_Vf();
        }
        #endregion

        #region Vc
        /// <summary>
        /// Cutting speed domain
        /// </summary>
        public static readonly ReadOnlyCollection<string> Vc_domain = Speeds;

        /// <summary>
        /// Cutting speed.
        /// </summary>
        public ScaledType Vc { get; private set; }

        /// <summary>
        /// Cutting speed flag to indicate calculations
        /// </summary>
        public bool Vc_calculated { get { return true; } }

        /// <summary>
        /// Spindle speed domain
        /// </summary>
        public static readonly ReadOnlyCollection<string> n_domain = Rotations;

        /// <summary>
        /// Recalculate cutting speed.
        /// </summary>
        void Calculate_Vc()
        {
#if TRACE_EVENTS
            log.Debug($"Calculator: Vc({material.Vc})");
#endif
            Vc.SetValueScaled(material.Vc);
        }
        #endregion

        #region n
        /// <summary>
        /// Spindle speed
        /// </summary>
        public ScaledType n { get; private set; }

        /// <summary>
        /// N flag to indicate calculations
        /// </summary>
        public bool n_calculated { get { return true; } }

        /// <summary>
        /// Recalculate spindle speed.
        /// </summary>
        void calculate_n()
        {
#if TRACE_EVENTS
            log.Debug($"Calculator: n({material.n})");
#endif
            n.SetValueScaled(material.n);
        }
        #endregion

        #region Vf
        /// <summary>
        /// Feed speed domain
        /// </summary>
        public static readonly ReadOnlyCollection<string> Vf_domain = Speeds;

        /// <summary>
        /// Feed speed
        /// </summary>
        public ScaledType Vf { get; private set; }

        /// <summary>
        /// Fv flag to indicate calculations
        /// </summary>
        public bool Vf_calculated { get { return true; } }

        /// <summary>
        /// Recalculate Feed rate.
        /// </summary>
        void calculate_Vf()
        {
            if (material.VcEnabled) Calculate_Vf_from_vc();
            else Calculate_Vf_from_n();
        }

        /// <summary>
        /// Recalculate Feed rate.
        /// </summary>
        void Calculate_Vf_from_vc()
        {
            // Vf = (Vc fz Z)/(pi D)
            var v1_3 = new DoubleST((long)tool.Z.GetValue(), "[tooth]");
            var v1 = (DoubleST)material.Vc * (DoubleST)material.fz * v1_3;
            var v2 = Math.PI * (DoubleST)tool.D;
#if TRACE_EVENTS
            log.Debug($"Calculator: Vf from vc({v2})");
#endif
            Vf.SetValueScaled(v1 / v2);
        }

        /// <summary>
        /// Recalculate Feed rate.
        /// </summary>
        void Calculate_Vf_from_n()
        {
            // Vf = n fz Z

            // n from rotations to time domain. (not yet done automatic)
            var v1_1 = new DoubleST(1, "[1/s]") * (DoubleST)n / n.BaseNormal();
            var v1_3 = new DoubleST((long)tool.Z.GetValue(), "[tooth]");
            var v1 = v1_1 * (DoubleST)material.fz * v1_3;
#if TRACE_EVENTS
            log.Debug($"Calculator: Vf from n({v1})");
#endif
            Vf.SetValueScaled(v1);
        }
        #endregion
    }
}
