using System.Windows;

using As.Applications.Config;
using As.Applications.ViewModels;

using Caliburn.Micro;

namespace As.Applications
{
    public class Bootstrapper : BootstrapperBase
    {
        public Bootstrapper()
        {
            Initialize();
        }

        protected override async void OnStartup(
            object sender,
            StartupEventArgs e)
        {
            await DisplayRootViewForAsync(typeof(CncCalculatorViewModel));
        }
    }
}
