using System.Windows;

using As.Applications.Config;
using As.Applications.Loggers;
using As.Applications.ViewModels;

using Caliburn.Micro;

namespace As.Applications
{
    public class Bootstrapper : BootstrapperBase
    {
        static Bootstrapper()
        {
            LogManager.GetLog = Settings.LogGenerator();
        }

        public Bootstrapper()
        {
            Initialize();
        }

        protected override async void OnStartup(
            object sender,
            StartupEventArgs e)
        {
            await DisplayRootViewForAsync(typeof(CncCalculatorViewModel));
            UI.InfoFormat($"{Settings.AppName}, v{Settings.AppVersion}");
        }
    }
}
