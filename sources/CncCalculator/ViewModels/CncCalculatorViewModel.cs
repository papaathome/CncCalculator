using System.ComponentModel;

using Caliburn.Micro;

using ILogger = As.Applications.Loggers.ILogger;
using LogManager = Caliburn.Micro.LogManager;

namespace As.Applications.ViewModels
{
    public class CncCalculatorViewModel : Conductor<object>, IDataErrorInfo
    {
        static readonly ILogger Log
            = (ILogger)LogManager.GetLog(typeof(CncCalculatorViewModel));

        public CncCalculatorViewModel()
        {
            FeedAndSpeed = new FeedAndSpeedViewModel();
            Converter = new ConverterViewModel();
        }

        #region Properties
        public FeedAndSpeedViewModel FeedAndSpeed { get; set; }

        public ConverterViewModel Converter { get; set; }
        #endregion Properties

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
