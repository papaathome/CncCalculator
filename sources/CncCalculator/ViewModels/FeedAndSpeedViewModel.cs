using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

using As.Applications.Data;

using Caliburn.Micro;

using Microsoft.Win32;
using Microsoft.Xaml.Behaviors.Core;

using ILogger = As.Applications.Loggers.ILogger;
using LogManager = Caliburn.Micro.LogManager;

namespace As.Applications.ViewModels
{
    public class FeedAndSpeedViewModel :
        Screen,
        IDataErrorInfo
    {
        static readonly ILogger Log
            = (ILogger)LogManager.GetLog(typeof(CncCalculatorViewModel));

        public FeedAndSpeedViewModel()
        {
            CuttingDepth = new DoubleSTViewModel(
                name: "h",
                domain: c.h_domain,
                data: c.h);
            Diameter = new DoubleSTViewModel(
                name: "D",
                domain: c.D_domain,
                data: c.D);
            Flutes = new LongSTViewModel(
                name: "Z",
                domain: c.Z_domain,
                data: c.Z);

            MaterialCuttingSpeed = new DoubleSTViewModel(
                name: "Vc",
                domain: c.Vc_domain,
                data: c.Vc)
            {
                ValueIsReadOnly = !IsMaterialCuttingSpeedPrimary
            };
            MaterialSpindleSpeed = new DoubleSTViewModel(
                name: "n",
                domain: c.n_domain,
                data: c.n)
            {
                ValueIsReadOnly = !IsMaterialSpindleSpeedPrimary
            };
            MaterialFeedPerTooth = new DoubleSTViewModel(
                name: "fz",
                domain: c.fz_domain,
                data: c.fz);

            CuttingSpeed = new DoubleSTViewModel(
                name: "Vc",
                domain: c.Vc_domain,
                data: c.Vc_result)
            {
                ValueIsReadOnly = true
            };
            SpindleSpeed = new DoubleSTViewModel(
                name: "n",
                domain: c.n_domain,
                data: c.n_result)
            {
                ValueIsReadOnly = true
            };
            Feed = new DoubleSTViewModel(
                name: "Vf",
                domain: c.Vf_domain,
                data: c.Vf_result)
            {
                ValueIsReadOnly = true
            };
        }

        readonly Models.CncCalculator c = new();

        #region Properties
        /// <summary>
        /// ListOfToolReferences list.
        /// </summary>
        public ReadOnlyDictionary<string, Tool> Tools => c.Tools;

        /// <summary>
        /// selected ToolSelected
        /// </summary>
        public string ToolSelected
        {
            get => c.ToolsKeySelected;
            set => c.ToolsKeySelected = value;
        }

        /// <summary>
        /// materials list.
        /// </summary>
        public ReadOnlyDictionary<string, Material> Materials => c.Materials;

        /// <summary>
        /// selected StockSelected
        /// </summary>
        public string MaterialSelected
        {
            get => c.MaterialsKeySelected;
            set => c.MaterialsKeySelected = value;
        }

        /// <summary>
        /// ToolSelected cutting depth
        /// </summary>
        public DoubleSTViewModel CuttingDepth { get; private set; }

        /// <summary>
        /// ToolSelected diameter
        /// </summary>
        public DoubleSTViewModel Diameter { get; private set; }

        /// <summary>
        /// ToolSelected flutes
        /// </summary>
        public LongSTViewModel Flutes { get; private set; }

        /// <summary>
        /// Radio button management for IsMaterialCuttingSpeedPrimary and IsMaterialSpindleSpeedPrimary
        /// </summary>
        bool RadioButtonPrimary
        {
            get => c.IsVcPrimary;
            set
            {
                if (c.IsVcPrimary == value) return;
                c.IsVcPrimary = value;
                MaterialCuttingSpeed.ValueIsReadOnly = !value;
                MaterialSpindleSpeed.ValueIsReadOnly = value;
                NotifyOfPropertyChange(nameof(IsMaterialCuttingSpeedPrimary));
                NotifyOfPropertyChange(nameof(IsMaterialSpindleSpeedPrimary));
            }
        }

        /// <summary>
        /// Stock cutting speed is the primary value for calculations, StockSelected spindle speed is calculated.
        /// </summary>
        public bool IsMaterialCuttingSpeedPrimary
        {
            get => RadioButtonPrimary;
            set => RadioButtonPrimary = value;
        }

        /// <summary>
        /// Stock spindle speed is the primary value for calculations, StockSelected cutting speed is calculated.
        /// </summary>
        public bool IsMaterialSpindleSpeedPrimary
        {
            get => !RadioButtonPrimary;
            set => RadioButtonPrimary = !value;
        }

        /// <summary>
        /// Stock cutting speed Vc
        /// </summary>
        public DoubleSTViewModel MaterialCuttingSpeed { get; private set; }

        /// <summary>
        /// Stock spindle speed n
        /// </summary>
        public DoubleSTViewModel MaterialSpindleSpeed { get; private set; }

        /// <summary>
        /// Stock Feed per tooth
        /// </summary>
        public DoubleSTViewModel MaterialFeedPerTooth { get; private set; }

        /// <summary>
        /// calculated cutting speed Vc
        /// </summary>
        public DoubleSTViewModel CuttingSpeed { get; private set; }

        /// <summary>
        /// calculated spindle speed n
        /// </summary>
        public DoubleSTViewModel SpindleSpeed { get; private set; }

        /// <summary>
        /// calculated feed rate Vf
        /// </summary>
        public DoubleSTViewModel Feed { get; private set; }
        #endregion Properties

        #region Menu actions
#pragma warning disable CA1416 // Validate platform compatibility
        public ICommand LoadToolsFile
            => _loadToolsFile ??= new ActionCommand(PerformLoadToolsFile);
        ActionCommand? _loadToolsFile;

        void PerformLoadToolsFile()
        {
            var f = new OpenFileDialog
            {
                InitialDirectory = c.ToolsPath,
                Filter = c.ToolsFilter,
                FilterIndex = 1,
                RestoreDirectory = true
            };
            if (f.ShowDialog() ?? false)
            {
                // TODO: show load success or fail
                c.TryLoadTools(f.FileName);
            }
        }

        public ICommand LoadMaterialsFile
            => _loadMaterialsFile ??= new ActionCommand(PerformLoadMaterialsFile);
        ActionCommand? _loadMaterialsFile;

        void PerformLoadMaterialsFile()
        {
            var f = new OpenFileDialog
            {
                InitialDirectory = c.MaterialsPath,
                Filter = c.MaterialsFilter,
                FilterIndex = 1,
                RestoreDirectory = true
            };
            if (f.ShowDialog() ?? false)
            {
                // TODO: show load success or fail
                c.TryLoadMaterials(f.FileName);
            }
        }

        public ICommand Exit => _exit ??= new ActionCommand(PerformExit);
        ActionCommand? _exit;

        void PerformExit()
        {
            Application.Current.Shutdown();
        }

#pragma warning restore CA1416 // Validate platform compatibility
        #endregion Menu actions

        #region IDataErrorInfo
        public string Error
        {
            get { return ""; }
        }

        public string this[string columnName]
        {
            get { return ""; }
        }
        #endregion IDataErrorInfo
    }
}
