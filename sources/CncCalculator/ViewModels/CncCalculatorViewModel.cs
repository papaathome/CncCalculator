using System.ComponentModel;

using Caliburn.Micro;

namespace As.Applications.ViewModels
{
    public class CncCalculatorViewModel : Conductor<object>, IDataErrorInfo
    {
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
