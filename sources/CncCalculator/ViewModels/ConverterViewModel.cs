using System.ComponentModel;

using Caliburn.Micro;

namespace As.Applications.ViewModels
{
    public class ConverterViewModel :
        Screen,
        IDataErrorInfo
    {
        public ConverterViewModel()
        {
            X1 = new DoubleSTViewModel(name: "x1", domain: c.XScales, data: c.X1);
            X2 = new DoubleSTViewModel(name: "x2", domain: c.XScales, data: c.X2);
            X3 = new DoubleSTViewModel(name: "x3", domain: c.XScales, data: c.X3);

            Y1 = new DoubleSTViewModel(name: "y1", domain: c.YScales, data: c.Y1);
            Y2 = new DoubleSTViewModel(name: "y2", domain: c.YScales, data: c.Y2);
            Y3 = new DoubleSTViewModel(name: "y3", domain: c.YScales, data: c.Y3);
        }

        readonly Models.Converter c = new();

        /// <summary>
        /// Register X1
        /// </summary>
        public DoubleSTViewModel X1 { get; private set; }

        /// <summary>
        /// Register X2
        /// </summary>
        public DoubleSTViewModel X2 { get; private set; }

        /// <summary>
        /// Register X3
        /// </summary>
        public DoubleSTViewModel X3 { get; private set; }

        /// <summary>
        /// Register Y1
        /// </summary>
        public DoubleSTViewModel Y1 { get; private set; }

        /// <summary>
        /// Register Y2
        /// </summary>
        public DoubleSTViewModel Y2 { get; private set; }

        /// <summary>
        /// Register Y3
        /// </summary>
        public DoubleSTViewModel Y3 { get; private set; }

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
