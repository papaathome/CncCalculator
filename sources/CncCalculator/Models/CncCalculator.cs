#define USE_LOG4NET

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

using As.Applications.Config;
using As.Applications.Data;
using As.Tools.Data.Scales;

namespace As.Applications.Models
{
    public class CncCalculator : INotifyPropertyChanged, IDataErrorInfo
    {
#pragma warning disable IDE1006 // Naming Styles
#pragma warning disable CA1822 // Mark members as static

#if USE_LOG4NET
        static readonly log4net.ILog Log = log4net.LogManager.GetLogger(nameof(CncCalculator));
#endif

        /// <summary>
        /// Calculator: The maths for CNC Feeds and Speeds.
        /// </summary>
        public CncCalculator()
        {
            h = new ScaledType<double>(1.0, "[mm]");
            D = new ScaledType<double>(1.0, "[mm]");
            Z = new ScaledType<long>(1, "[tooth]");

            IsVcPrimary = true;
            Vc = new ScaledType<double>(1.0, "[mm/s]");
            n = new ScaledType<double>(1.0, "[rpm]");
            fz = new ScaledType<double>(1.0, "[mm/tooth]");

            Vc_result = new ScaledType<double>(1.0, "[mm/s]");
            n_result = new ScaledType<double>(1.0, "[rpm]");
            Vf_result = new ScaledType<double>(1.0, "[mm/s]");

            h.OnChanged += h_changed;
            D.OnChanged += D_changed;
            Z.OnChanged += Z_changed;

            Vc.OnChanged += Vc_changed;
            n.OnChanged += n_changed;
            fz.OnChanged += fz_changed;

            Vc_result.OnChanged += Vc_result_changed;
            n_result.OnChanged += n_result_changed;
            Vf_result.OnChanged += Vf_result_changed;

            TryLoadTools(Settings.App.ToolsPath);
            TryLoadMaterials(Settings.App.MaterialsPath);
        }

        #region Tools list
        /// <summary>
        /// First directory to use in a files dialog for bit files (collection of bits).
        /// </summary>
        public string ToolsPath
        {
            get
            {
                var candidate = Path.GetDirectoryName(Path.GetFullPath(Settings.App.ToolsPath));
                return (string.IsNullOrWhiteSpace(candidate))
                    ? Environment.GetFolderPath(Environment.SpecialFolder.Personal)
                    : candidate;
            }
        }

        /// <summary>
        /// Filter to use in a files dialog for tool files.
        /// </summary>
        public string ToolsFilter
            => "tool files (*.fctl)|*.fctl|All files (*.*)|*.*";

        /// <summary>
        /// Load new tools list
        /// </summary>
        /// <param name="path">Path to the new tools list</param>
        public bool TryLoadTools(string path)
        {
            try
            {
                var t = ToolsList.GetData(path);
                if (t != null)
                {
                    Tools = new ReadOnlyDictionary<string, Tool>(t.Tools);
                    Settings.App.ToolsPath = path;
                }
                else
                {
                    SetError($"Failed to load \"{path}\"", nameof(Tools));
                }
                return true;
            }
#if USE_LOG4NET
            catch (Exception x)
            {
                var msg = x.Message.Trim();
                SetError($"{msg}; path = \"{path}\"", nameof(Tools));
                Log.ErrorFormat($"LoadTools: {x.Message.Trim()}; path = \"{path}\"");
                for (var i = x.InnerException; i != null; i = i.InnerException)
                {
                    Log.ErrorFormat($"LoadTools: {x.Message.Trim()}");
                }
            }
#else
            catch { }
#endif
            return false;
        }

        /// <summary>
        /// Tools dictionary
        /// </summary>
        public ReadOnlyDictionary<string, Tool> Tools
        {
            get => _tools;
            private set
            {
                _tools = value;
                SetError("");
                NotifyPropertyChanged();
                ToolsKeySelected = _tools.Keys.First() ?? "";
            }
        }
        ReadOnlyDictionary<string, Tool> _tools
            = new(new Dictionary<string, Tool>());

        public string ToolsKeySelected
        {
            get { return _toolskeySelected ??= ""; }
            set
            {
                if (_toolskeySelected == value) return;
                SetError(ValidateToolKeySelected(value));
                _toolskeySelected = value;
                NotifyPropertyChanged();
            }
        }
        string? _toolskeySelected;

        string ValidateToolKeySelected(string key)
        {
            if (
                !Tools.TryGetValue(key, out Tool? t) ||
                (t == null)
               )
            {
                return $"tool unknown; key=\"{key}\"";
            }

            // set h?
            D.SetValue(new ScaledType<double>(1.0, "[mm]"));

            // set D.
            var Ds = t.Parameter.TryGetValue(
                "Diameter",
                out string? ds_value)
                ? ds_value
                : "";
            D.SetValue(ScaledType<double>.Scan(Ds, 1.0, "mm"));

            // set Z.
            var Zs = t.Parameter.TryGetValue(
                "Flutes",
                out string? zs_value)
                ? zs_value
                : "";
            Z.SetValue(ScaledType<long>.Scan(Zs, 1L, "tooth"));

            return "";
        }
        #endregion Tools list

        #region Materials list
        /// <summary>
        /// First directory to use in a files dialog for material files.
        /// </summary>
        public string MaterialsPath
        {
            get
            {
                var candidate = Path.GetDirectoryName(
                    Path.GetFullPath(
                        Settings.App.MaterialsPath));
                return (string.IsNullOrWhiteSpace(candidate))
                    ? Environment.GetFolderPath(
                        Environment.SpecialFolder.Personal)
                    : candidate;
            }
        }

        /// <summary>
        /// Filter to use in a files dialog for material files.
        /// </summary>
        public string MaterialsFilter
            => "material files (*.json)|*.json|All files (*.*)|*.*";

        /// <summary>
        /// Load new material list
        /// </summary>
        /// <param name="path">Path to the new materials list</param>
        public bool TryLoadMaterials(string path)
        {
            try
            {
                var m = MaterialList.GetData(path);
                if (m != null)
                {
                    Materials = new ReadOnlyDictionary<string, Material>(m.Materials);
                    Settings.App.MaterialsPath = path;
                }
                else
                {
                    SetError($"Failed to load \"{path}\"", nameof(Materials));
                }
                return true;
            }
#if USE_LOG4NET
            catch (Exception x)
            {
                var msg = x.Message.Trim();
                SetError($"{msg}; path = \"{path}\"", nameof(Materials));
                Log.ErrorFormat($"LoadMaterials: {msg}; path = \"{path}\"");
                for (var i = x.InnerException; i != null; i = i.InnerException)
                {
                    Log.ErrorFormat($"LoadMaterials: {x.Message.Trim()}");
                }
            }
#else
            catch { }
#endif
            return false;
        }

        /// <summary>
        /// List of materials.
        /// </summary>
        public ReadOnlyDictionary<string, Material> Materials
        {
            get => _materials;
            private set
            {
                _materials = value;
                SetError("");
                NotifyPropertyChanged();
                MaterialsKeySelected = _materials.Keys.First() ?? "";
            }
        }
        ReadOnlyDictionary<string, Material> _materials
            = new (new Dictionary<string, Material>());

        public string MaterialsKeySelected
        {
            get { return _materialsKeySelected ??= ""; }
            set
            {
                if (_materialsKeySelected == value) return;
                SetError(ValidateMaterialKeySelected(value));
                _materialsKeySelected = value;
                NotifyPropertyChanged();
            }
        }
        string? _materialsKeySelected;

        string ValidateMaterialKeySelected(string key)
        {
            if (!Materials.TryGetValue(key, out Material? m))
            {
                return $"material unknown; key=\"{key}\""; ;
            }

            // Set primary argument, Vc or n.
            if (m.UseCuttingSpeed)
            {
                // Set Vc.
                IsVcPrimary = true;
                Vc.SetValue(ScaledType<double>.Scan(m.CuttingSpeed, 1.0, "mm/s"));
            }
            else
            {
                // Set n.
                IsNPrimary = true;
                n.SetValue(ScaledType<double>.Scan(m.SpindleSpeed, 1.0, "rpm"));
            }

            // set fz.
            fz.SetValue(ScaledType<double>.Scan(m.FeedPerTooth, 1.0, "mm/tooth"));

            return "";
        }
        #endregion Materials list

        #region Calculations control
        readonly object calculating_key = new object();
        bool Calculating
        {
            get { lock (calculating_key) { return _calculating; } }
            set { lock (calculating_key) { _calculating = value; } }
        }
        bool _calculating = false;

        bool TrySetCalculating(out bool result)
        {
            lock (calculating_key)
            {
                result = !_calculating;
                _calculating = true;
            }
            return result;
        }

        void CalculateOnChange(Action action)
        {
            bool in_control = false;
            try { if (TrySetCalculating(out in_control)) action(); }
            finally { if (in_control) Calculating = false; }
        }
        #endregion Calculations control

        #region Bit selected
        #region h
        /// <summary>
        /// Cutting depth domain
        /// </summary>
        public ReadOnlyCollection<string> h_domain => Domain.Lengths;

        /// <summary>
        /// Cutting depth
        /// </summary>
        public ScaledType<double> h { get; private set; }

        /// <summary>
        /// Indication of a change in height.
        /// </summary>
        /// <param name="sender">Tool manager</param>
        /// <param name="e">Change attributes.</param>
        private void h_changed(object caller, ScaledTypeEventArgs e)
        {
            CalculateOnChange(Set_Vf_result);
            NotifyPropertyChanged(nameof(h));
        }

        /// <summary>
        /// Reduce factor on speeds relative to h and D
        /// </summary>
        double h_factor
        {
            get
            {
                // Ch = 1.25 - 0.25*h/D for h/D in [1..3]

                var ch = h / D;
                if (ch.Value < 1.0) ch.Value = 1.0;
                else if (3.0 < ch.Value) ch.Value = 3.0;

                return 1.25 - 0.25 * ch.Value;
            }
        }
        #endregion

        #region D
        /// <summary>
        /// Diameter domain
        /// </summary>
        public ReadOnlyCollection<string> D_domain => Domain.Lengths;

        /// <summary>
        /// Diameter
        /// </summary>
        public ScaledType<double> D { get; private set; }

        /// <summary>
        /// Indication of a change in diameter.
        /// </summary>
        /// <param name="sender">Tool manager</param>
        /// <param name="e">Change attributes.</param>
        private void D_changed(object caller, ScaledTypeEventArgs e)
        {
            CalculateOnChange(() =>
            {
                if (IsVcPrimary)
                {
                    Set_n();
                    Set_n_result();
                }
                else // IsNPrimary
                {
                    Set_Vc();
                    Set_Vc_result();
                }
                Set_Vf_result();
            });
            NotifyPropertyChanged(nameof(D));
        }
        #endregion

        #region Z
        /// <summary>
        /// Flutes domain
        /// </summary>
        public ReadOnlyCollection<string> Z_domain => Domain.Counts;

        /// <summary>
        /// Flutes
        /// </summary>
        public ScaledType<long> Z { get; private set; }

        /// <summary>
        /// Indication of a change in flutes.
        /// </summary>
        /// <param name="sender">Tool manager</param>
        /// <param name="e">Change attributes.</param>
        private void Z_changed(object caller, ScaledTypeEventArgs e)
        {
            CalculateOnChange(Set_Vf_result);
            NotifyPropertyChanged(nameof(Z));
        }
        #endregion Z
        #endregion Bit selected

        #region Stock type selected
        #region Radio button primary calculations argument, true: Vc or false: n.
        /// <summary>
        ///  Radio button management for Vc (true) or n (false) as primary value in calculations
        /// </summary>
        bool RadioButtonPrimary
        {
            get { return _primary; }
            set
            {
                if (_primary != value)
                {
                    _primary = value;
                    NotifyPropertyChanged(nameof(IsVcPrimary));
                    NotifyPropertyChanged(nameof(IsNPrimary));
                    if (IsVcPrimary) Vc_changed(Vc, new());
                    else n_changed(n, new());
                }
            }
        }
        bool _primary = true;
        #endregion Radio button primary calculations argument, true: Vc or false: n.

        #region Vc
        /// <summary>
        /// Cutting speed domain
        /// </summary>
        public ReadOnlyCollection<string> Vc_domain => Domain.Speeds;

        /// <summary>
        /// Cutting speed Vc
        /// </summary>
        public ScaledType<double> Vc { get; set; }

        /// <summary>
        /// Recalculate Vc
        /// </summary>
        void Set_Vc()
        {
            if (!IsVcPrimary)
            {
                // calculate Vc from n and bit D
                // Vc = pi D n

                // n from rotations to time domain. (not yet done automatic)
                var v1_3 = new ScaledType<double>(1, "[1/s]") * n / n.BaseNormal();
                var v1 = Math.PI * D * v1_3;
                Vc.SetValueScaled(v1);
            }
        }

        /// <summary>
        /// Cutting speed Vc as primary value in calculations
        /// </summary>
        public bool IsVcPrimary
        {
            get { return RadioButtonPrimary; }
            set { RadioButtonPrimary = value; }
        }

        /// <summary>
        /// Indication of a change in cutting speed.
        /// </summary>
        /// <param name="sender">Material manager.</param>
        /// <param name="e">Change attributes.</param>
        private void Vc_changed(object caller, ScaledTypeEventArgs e)
        {
            CalculateOnChange(() =>
            {
                if (IsVcPrimary)
                {
                    Set_n();
                    Set_n_result();
                    Set_Vf_result();
                }
                else // IsNPrimary
                {
                    Set_Vc();
                }
                Set_Vc_result();
            });
            NotifyPropertyChanged(nameof(Vc));
        }
        #endregion Vc

        #region n
        /// <summary>
        /// Spindle speed domain
        /// </summary>
        public ReadOnlyCollection<string> n_domain => Domain.Rotations;

        /// <summary>
        /// Spindle speed n
        /// </summary>
        public ScaledType<double> n { get; set; }

        /// <summary>
        /// Recalculate spindle speed.
        /// </summary>
        void Set_n()
        {
            if (!IsNPrimary)
            {
                // calculate n from Vc and bit D
                // n = Vc / (pi D)

                // n from time to rotations domain. (not yet done automatic)
                var v1 = new ScaledType<double>(1, "[rps s]") * Vc / (Math.PI * D);
                n.SetValueScaled(v1);
            }
        }

        /// <summary>
        /// Spindle speed n as primary value in calculations
        /// </summary>
        public bool IsNPrimary
        {
            get { return !RadioButtonPrimary; }
            set { RadioButtonPrimary = !value; }
        }

        /// <summary>
        /// Indication of a change in spindle speed.
        /// </summary>
        /// <param name="sender">Material manager.</param>
        /// <param name="e">Change attributes.</param>
        private void n_changed(object caller, ScaledTypeEventArgs e)
        {
            CalculateOnChange(() =>
            {
                if (IsNPrimary)
                {
                    Set_Vc();
                    Set_Vc_result();
                    Set_Vf_result();
                }
                else // IsVcPriry
                {
                    Set_n();
                }
                Set_n_result();
            });
            NotifyPropertyChanged(nameof(n));
        }
        #endregion n

        #region fz
        /// <summary>
        /// Feed per tooth domain
        /// </summary>
        public ReadOnlyCollection<string> fz_domain => Domain.LengthsOverCounts;

        /// <summary>
        /// Feed per tooth fz
        /// </summary>
        public ScaledType<double> fz { get; set; }

        /// <summary>
        /// Indication of a change in cutting rate per tooth
        /// </summary>
        /// <param name="sender">Material manager.</param>
        /// <param name="e">Change attributes.</param>
        private void fz_changed(object caller, ScaledTypeEventArgs e)
        {
            CalculateOnChange(Set_Vf_result);
            NotifyPropertyChanged(nameof(fz));
        }
        #endregion fz
        #endregion Stock type selected

        #region Results
        #region Vc result
        /// <summary>
        /// Cutting speed.
        /// </summary>
        public ScaledType<double> Vc_result { get; private set; }

        /// <summary>
        /// Recalculate cutting speed.
        /// </summary>
        void Set_Vc_result() => Vc_result.SetValueScaled(Vc);

        private void Vc_result_changed(object caller, ScaledTypeEventArgs e)
        {
            if (e.IsScaleChanged) CalculateOnChange(Set_Vc_result);
            NotifyPropertyChanged(nameof(Vc_result));
        }
        #endregion Vc result

        #region n result
        /// <summary>
        /// Spindle speed
        /// </summary>
        public ScaledType<double> n_result { get; private set; }

        /// <summary>
        /// Recalculate spindle speed.
        /// </summary>
        void Set_n_result() => n_result.SetValueScaled(n);

        private void n_result_changed(object caller, ScaledTypeEventArgs e)
        {
            if (e.IsScaleChanged) Set_n_result();
            NotifyPropertyChanged(nameof(n_result));
        }
        #endregion n result

        #region Vf result
        /// <summary>
        /// Feed speed domain
        /// </summary>
        public ReadOnlyCollection<string> Vf_domain => Domain.Speeds;

        /// <summary>
        /// Feed speed
        /// </summary>
        public ScaledType<double> Vf_result { get; private set; }

        /// <summary>
        /// Recalculate Feed rate.
        /// </summary>
        void Set_Vf_result()
        {
            ScaledType<double> temp;
            if (IsVcPrimary)
            {
                // Vf = ch (Vc fz Z)/(pi D)
                var v1_3 = new ScaledType<double>(Z.Value, "[tooth]");
                var v1 = Vc * fz * v1_3;
                var v2 = Math.PI * D;
                temp = v1 / v2;
            }
            else
            {
                // Vf = ch n fz Z

                // n from rotations to time domain. (not yet done automatic)
                var v1_1 = new ScaledType<double>(1, "[1/s]") * n / n.BaseNormal();
                var v1_3 = new ScaledType<double>(Z.Value, "[tooth]");
                temp = v1_1 * fz * v1_3;
            }
            var ch = h_factor;
            Vf_result.SetValueScaled(ch * temp);
        }

        private void Vf_result_changed(object caller, ScaledTypeEventArgs e)
        {
            if (e.IsScaleChanged) Set_Vf_result();
            NotifyPropertyChanged(nameof(Vf_result));
        }
        #endregion Vf
        #endregion Results

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;

        void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propertyName));
        }
        #endregion INotifyPropertyChanged

        #region IDataErrorInfo
        readonly Dictionary<string, string> _named_errors = [];

        void SetError(string err, [CallerMemberName] string propertyName = "")
        {
            if ((_named_errors.TryGetValue(propertyName, out string? v) ? v : null) != err)
            {
                _named_errors[propertyName] = err;
            }
        }

        public string this[string propertyName]
        {
            get => _named_errors.TryGetValue(propertyName, out string? v)
                ? v
                : string.Empty;
            set => SetError(value, propertyName);
        }

        public string Error
        {
            get
            {
                var _sb = new StringBuilder();
                _sb.Clear();
                foreach (var kv in _named_errors)
                {
                    if (string.IsNullOrWhiteSpace(kv.Value)) continue;

                    if (0 < _sb.Length) _sb.Append("; ");
                    _sb.Append(kv.Key);
                    _sb.Append(":\"");
                    _sb.Append(kv.Value);
                    _sb.Append('"');
                }
                return (0 < _sb.Length) ? _sb.ToString() : "";
            }
        }
        #endregion IDataErrorInfo

#pragma warning restore CA1822 // Mark members as static
#pragma warning restore IDE1006 // Naming Styles
    }
}
