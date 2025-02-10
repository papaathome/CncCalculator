using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace As.Applications.Converters
{
    // see: https://stackoverflow.com/questions/5992769/wpf-valueconverter-standard-return-for-unconvertible-value

    [ValueConversion(typeof(int), typeof(string))]
    public class ConverterToLongCi : IValueConverter
    {
        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
            => long.TryParse(
                value as string,
                CultureInfo.InvariantCulture,
                out long v) ? v : DependencyProperty.UnsetValue;

        public object Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
            => ((long)value).ToString(
                parameter as string ?? "0",
                CultureInfo.InvariantCulture);
    }
}
