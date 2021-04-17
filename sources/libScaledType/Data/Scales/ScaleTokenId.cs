using System.Text.RegularExpressions;

using As.Tools.Data.Scanners;

namespace As.Tools.Data.Scales
{
    public enum TokenId
    {
        /// <summary>
        /// Eno of Text reached.
        /// </summary>
        _EOT_ = _TokenId._EOT_,

        /// <summary>
        /// Scanner in error state
        /// </summary>
        _ERROR_ = _TokenId._ERROR_,

        NAME              = 1, // [a-zAZ]+
        INT,                   // [+-]?[0-9]+
        BRACKET_OPEN,          // '['
        BRACKET_CLOSE,         // ']'
        PARENTESES_OPEN,       // '('
        PARENTESES_CLOSE,      // ')'
        PWR,                   // '^'
        DIV                    // '/'
    }

    internal static class TokenIdX
    {
        public static bool EQ(this int left, TokenId right) { return (left == (int)right); }

        public static bool NE(this int left, TokenId right) { return (left != (int)right); }

        public static string Text(this TokenId value, bool as_symbol = true)
        {
            var result = "";
            switch (value)
            {
                case TokenId._EOT_:               result = "_EOT_"; break;
                case TokenId._ERROR_:             result = "_ERROR_"; break;

                case TokenId.NAME:                result = (as_symbol) ? "#" : "[A-Za-z]+"; break;
                case TokenId.INT:                 result = "([+-]?[1-9][0-9]*|0)"; break;

                case TokenId.BRACKET_OPEN:        result = "["; break;
                case TokenId.BRACKET_CLOSE:       result = "]"; break;

                case TokenId.PARENTESES_OPEN:     result = "("; break;
                case TokenId.PARENTESES_CLOSE:    result = ")"; break;

                case TokenId.PWR:                 result = "^"; break;
                case TokenId.DIV:                 result = "/"; break;

                default: result = $"{value}"; break;
            }
            return result;
        }

        public static Regex Regex(this TokenId value, RegexOptions options = RegexOptions.None)
        {
            return new Regex($"\\G(?<text>(?<value>{value.Text(as_symbol: false)}))", options);
        }
    }
}
