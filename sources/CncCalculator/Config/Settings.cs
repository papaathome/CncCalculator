using System.CommandLine;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Xml.Serialization;

using As.Applications.IO;
using As.Applications.Loggers;
using As.Applications.Validation;

using ILogger = As.Applications.Loggers.ILogger;

[assembly: log4net.Config.XmlConfigurator(
    ConfigFile = @".\CncCalculator.log4net.config",
    Watch = true)]

namespace As.Applications.Config
{
    public class Settings : IChanged, IXmlProcessing
    {
        static readonly ILogger Log = new CmLog4NetLogger(typeof(Settings));

        static Settings()
        {
            AppName = System.Reflection.Assembly
                .GetExecutingAssembly()
                .GetName()
                .Name ?? APPLICATION_NAME;
            AppVersion = System.Reflection.Assembly
                .GetExecutingAssembly()
                .GetName()
                .Version;
            Log.InfoFormat($"Settings: {AppName} v{AppVersion}");

            // see: https://learn.microsoft.com/en-us/dotnet/standard/commandline/
            // see: https://stackoverflow.com/questions/20342061/disable-dll-culture-folders-on-compile (reply by David Rogers)

            // about arg0: .exe is a wrapper for the .dll and only checks if the correct runtime system is available.
            //             unfortunately, the args are mangled with an extra argument in the process to bootstrap it.
            var arg0 = new Argument<string>(
                getDefaultValue: () => "")
            {
                IsHidden = true
            };

            var opConfig = new Option<string>(
                "--configuration",
                getDefaultValue: () => ConfigPathDefault);
            opConfig.AddAlias("-c");

            var rootCommand = new RootCommand
            {
                arg0,
                opConfig
            };

            rootCommand.SetHandler(
                static (arg0Value, opConfigValue) =>
                    AppConfigPath = opConfigValue,
                    arg0,
                    opConfig);

            rootCommand.Invoke(Environment.GetCommandLineArgs());
            Log.DebugFormat($"Settings: configuration path = \"{AppConfigPath}\"");

            StoreOnExit = XmlStream.Read(
                AppConfigPath,
                out Settings app,
                create_if_missing: true,
                noexcept: true);
            App = app ?? new Settings();

            try { Application.Current.Exit += Current_Exit; }
            catch { /* never mind */ }
        }

        #region Static properties
        public const string APPLICATION_NAME = "CncCalculator";

        const string EXTENTION = ".conf";

        static public readonly string AppName;

        /// <summary>
        /// VERSION as set in the project file, node Project.PropertyGroup.Version
        /// </summary>
        /// <remarks>
        /// Application version, format is 'major.minor.build.bugfix'
        /// The major number is incremented on new functiontionality or large changeover.
        /// The minor number is incremented (in steps of 2) on breaking changes, even numbers are release versions, odd numbers are debug versions.
        /// The build number is incremented on changes not adding new functionality(e.g.on optimising performance.)
        /// The bugfix number is incremented on bugfixes.
        /// </remarks>
        static public readonly Version? AppVersion;

        static string ConfigPathDefault => Path.Combine(
            ".",
            $"{AppName}{EXTENTION}");

        static string AppConfigPath = "";

        static public Settings App { get; }

        static public bool StoreOnExit { get; set; }
        #endregion Static properties

        #region Static actions
        static void Current_Exit(object _, System.Windows.ExitEventArgs __)
        {
            // moving to _exit, try not to throw exceptions, unknown why moving to _exit.
            try
            {
                if (StoreOnExit && App.IsChanged)
                {
                    XmlStream.Write(
                        AppConfigPath,
                        App,
                        create_backup: true,
                        noexcept: true);
                }
                Log.InfoFormat($"Settings: Release {AppName} v{AppVersion}");
            }
            catch (Exception x)
            {
                try { Log.ErrorFormat("Current_Exit: exception", x); }
                catch { /* never mind */ }
            }
        }

        /// <summary>
        /// Return F(Type) -> ILogger
        /// </summary>
        /// <returns>Function that creates an ILogger for a type</returns>
        public static Func<Type, ILogger> LogGenerator()
            => type => new CmLog4NetLogger(type);
        #endregion Static actions

        public Settings()
        {
            PropertyChanged += OnChanged;
        }

        #region Properties
        //[XmlIgnore]
        [XmlAttribute("verbose")]
        public bool Verbose
        {
            get => _verbose;
            set
            {
                if (_verbose != value)
                {
                    _verbose = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Verbose)));
                }
            }
        }
        bool _verbose = false;

        //[XmlIgnore]
        [XmlAttribute("debug")]
        public bool Debug
        {
            get => _debug;
            set
            {
                if (_debug != value)
                {
                    _debug = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Debug)));
                }
            }
        }
        bool _debug = false;

        [XmlElement("tools_path")]
        public string ToolsPath
        {
            get => _tools_path;
            set
            {
                if (_tools_path == value) return;
                _tools_path = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ToolsPath)));
            }
        }
        string _tools_path = @"data\Library\tools.fctl";

        [XmlElement("materials_path")]
        public string MaterialsPath
        {
            get => _materials_path;
            set
            {
                if (_materials_path == value) return;
                _materials_path = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MaterialsPath)));
            }
        }
        string _materials_path = @"data\Library\materials.json";
        #endregion Properties

        #region IXmlProcessing
        /// <inheritdoc/>
        void IXmlProcessing.XmlWritePrepare() => WriteXmlPrepare();

        /// <summary>
        /// Prepare data for writing to XML.
        /// </summary>
        protected virtual void WriteXmlPrepare() { /* TODO: */ }

        /// <inheritdoc/>
        void IXmlProcessing.XmlWriteProcess() => WriteXmlProcess();

        /// <summary>
        /// Process data after writing to XML.
        /// </summary>
        protected virtual void WriteXmlProcess() { /* TODO: */ }

        /// <inheritdoc/>
        void IXmlProcessing.XmlReadProcess() => ReadXmlProcess();

        /// <summary>
        /// Process data after reading from XML.
        /// </summary>
        protected virtual void ReadXmlProcess() { /* TODO: */ }
        #endregion IXmlProcessing

        #region IChanged
        [XmlIgnore]
        public bool IsChanged { get; protected set; } = true;

        void OnChanged(object? sender, PropertyChangedEventArgs e)
            => IsChanged = true;

        public void ResetChanged() => IsChanged = false;

        public event PropertyChangedEventHandler? PropertyChanged;
        #endregion IChanged
    }
}
