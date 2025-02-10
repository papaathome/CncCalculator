using System.Text;

namespace As.Applications.Converters
{
    static public class StringExtentions
    {
        static public readonly string FS = Environment.NewLine;

        static public List<string> ToLst(
            this string value,
            string? field_seperator = null)
        {
            if (string.IsNullOrEmpty(value)) return [];
            return new List<string>(value.Split(field_seperator ?? FS));
        }

        static public string ToStr(
            this List<string> value,
            string? field_seperator = null)
        {
            var sb = new StringBuilder();
            var first = true;
            var fs = field_seperator ?? FS;
            foreach (var line in value)
            {
                if (first) first = false;
                else sb.Append(fs);
                sb.Append(line.TrimEnd());
            }
            return sb.ToString();
        }
    }
}
