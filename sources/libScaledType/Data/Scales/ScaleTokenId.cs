using System.Text.RegularExpressions;

using As.Tools.Data.Scanners;

namespace As.Tools.Data.Scales
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
            string? result = value switch
            {
                TokenId._EOT_            => "_EOT_",
                TokenId._ERROR_          => "_ERROR_",
                TokenId.NAME             => (as_symbol) ? "#" : "[A-Za-z]+",
                TokenId.INT              => "([+-]?[1-9][0-9]*|0)",
                TokenId.BRACKET_OPEN     => "[",
                TokenId.BRACKET_CLOSE    => "]",
                TokenId.PARENTESES_OPEN  => "(",
                TokenId.PARENTESES_CLOSE => ")",
                TokenId.PWR              => "^",
                TokenId.DIV              => "/",
                _                        => $"{value}",
            };
            return result;
        }

        public static Regex Regex(this TokenId value, RegexOptions options = RegexOptions.None)
        {
            return new Regex($"\\G(?<text>(?<value>{value.Text(as_symbol: false)}))", options);
        }
    }
}
