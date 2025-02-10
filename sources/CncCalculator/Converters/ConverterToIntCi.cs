using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace As.Applications.Converters
{
    // see: https://stackoverflow.com/questions/5992769/wpf-valueconverter-standard-return-for-unconvertible-value

    [ValueConversion(typeof(int), typeof(string))]
    public class ConverterToIntCi : IValueConverter
    {
        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
            => int.TryParse(
                value as string,
                CultureInfo.InvariantCulture,
                out int v) ? v : DependencyProperty.UnsetValue;

        public object Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
            => ((int)value).ToString(
                parameter as string ?? "0",
                CultureInfo.InvariantCulture);
    }
}
