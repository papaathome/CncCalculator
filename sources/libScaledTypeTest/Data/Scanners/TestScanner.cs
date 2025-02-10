using System.Text.RegularExpressions;

using As.Tools.Data.Scanners;

namespace As.Tools.Test.Data.Scanners
{
    public enum TokenId
    {
        /// <summary>
        /// End of Text reached.
        /// </summary>
        _EOT_ = TokenIdBase._EOT_,

        /// <summary>
        /// Scanner in error state
        /// </summary>
        _ERROR_ = TokenIdBase._ERROR_,

        VALUE = 1, // [^ \t\f\n]+
    }

    public static class TokenIdX
    {
        public static string Text(this TokenId value)
        {
            string? result = value switch
            {
                TokenId._EOT_ => "_EOT_",
                TokenId._ERROR_ => "_ERROR_",
                TokenId.VALUE => "[^ \t\f\n]+",
                _ => $"{value}",
            };
            return result;
        }

        public static Regex Regex(this TokenId value, RegexOptions options = RegexOptions.None)
        {
            return new Regex($"\\G(?<text>(?<value>{value.Text()}))", options);
        }
    }

    public class TestScanner : Scanner
    {
        const RegexOptions REGEX_OPTIONS = RegexOptions.ExplicitCapture;

        protected override object StateNormal => ScannerState.NORMAL;

        protected override object StateError => ScannerState.ERROR;

        readonly Regex NAME = TokenId.VALUE.Regex(REGEX_OPTIONS);
        protected override void Scan(out Token result)
        {
            switch(State)
            {
                case ScannerState.NORMAL:
                    if ((Buffer == null) || (Buffer.Length <= Column)) goto case ScannerState.ERROR;
                    if (TryGetSymbol(TokenId.VALUE, NAME, out Token? r))
                    {
                        result = r!;
                        break;
                    }
                    result = new ScannerToken(State, TokenId._EOT_, Position, "Scanner end of text.");
                    break;
                case ScannerState.ERROR:
                    result = new ScannerToken(State, TokenId._ERROR_, Position, "Scanner no token recognised.");
                    break;
                default:
                    result = new ScannerToken(State, TokenId._ERROR_, Position, $"Scanner state not recognised: {State}");
                    SetState((int)ScannerState.ERROR);
                    break;
            }
        }

        bool TryGetSymbol(TokenId sysmbol, Regex re, out Token? output)
        {
            Match? match = null;
            if ((Buffer != null) && (Column < Buffer.Length)) match = re.Match(Buffer, Column);
            if (match?.Success ?? false)
            {
                var value = match.Groups["text"].Value;
                output = new ScannerToken(State, sysmbol, Position, value);
                Column += value.Length;
            }
            else output = null;
            return (output != null);
        }
    }
}
