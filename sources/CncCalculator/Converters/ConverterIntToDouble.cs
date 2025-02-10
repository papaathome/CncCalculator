using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace As.Applications.Converters
{
    [ValueConversion(typeof(double), typeof(int))]
    public class ConverterIntToDouble : IValueConverter
    {
        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
            => (value is int v)
                ? System.Convert.ToDouble(v)
                : DependencyProperty.UnsetValue;

        public object Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
            => (value is double v)
                ? System.Convert.ToInt32(v)
                : DependencyProperty.UnsetValue;
    }
}
