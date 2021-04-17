using System.IO;
using System.Text.RegularExpressions;

using As.Tools.Data.Scanners;

namespace As.Tools.Data.Scales
{
    internal class ScaleScanner : Scanner
    {
        const RegexOptions REGEX_OPTIONS = RegexOptions.ExplicitCapture;

        public enum States
        {
            /// <summary>
            /// The error state.
            /// </summary>
            ERROR = _State.ERROR,

            /// <summary>
            /// SOF . '...' EOF ; the normal state.
            /// </summary>
            NORMAL = _State.NORMAL,

            /// <summary>
            /// Predefined function names not active.
            /// Include predefined function names with NAME.
            /// </summary>
            //NONF
        }

        override protected object StateNormal => States.NORMAL;
        override protected object StateError => States.ERROR;

        override public object TokenError => TokenId._ERROR_;
        override public object TokenEOT => TokenId._EOT_;
        //override public object TokenEOL => TokenId._EOL_;
        //override public object TokenComment => TokenId._COMMENT_;

        public ScaleScanner(IProgressBar progressbar = null) : base(progressbar)
        {
            BuildRegex();
        }

        public ScaleScanner(string path, IProgressBar progressbar) : base(path, progressbar)
        {
            BuildRegex();
        }

        public ScaleScanner(string buffer) : base(buffer)
        {
            BuildRegex();
        }

        public ScaleScanner(StreamReader input, IProgressBar progressbar) : base(input, progressbar)
        {
            BuildRegex();
        }

        Regex NAME;
        Regex INT;

        void BuildRegex()
        {
            NAME = TokenId.NAME.Regex(REGEX_OPTIONS);
            INT  = TokenId.INT.Regex(REGEX_OPTIONS);
        }

        protected override void Scan(ref Token result)
        {
            switch (State)
            {
                case States.NORMAL:
                    switch (buffer[column])
                    {
                        case '[': SetSymbol(TokenId.BRACKET_OPEN, ref result); break;
                        case ']': SetSymbol(TokenId.BRACKET_CLOSE, ref result); break;
                        case '(': SetSymbol(TokenId.PARENTESES_OPEN, ref result); break;
                        case ')': SetSymbol(TokenId.PARENTESES_CLOSE, ref result); break;
                        case '^': SetSymbol(TokenId.PWR, ref result); break;
                        case '/': SetSymbol(TokenId.DIV, ref result); break;
                        case '#': SetSymbol(TokenId.NAME, ref result, "#"); break;
                        default:
                            if (CheckSymbol(TokenId.INT, INT, ref result)) break;
                            CheckSymbol(TokenId.NAME, NAME, ref result);
                            break;
                    }
                    break;
                case States.ERROR:
                    result = new TokenScanner(State, TokenId._ERROR_, Position, "Scanner no token recognised.");
                    break;
                default:
                    result = new TokenScanner(State, TokenId._ERROR_, Position, $"Scanner state not recognised: {State}");
                    SetState((int)States.ERROR);
                    break;
            }
        }

        void SetSymbol(TokenId symbol, ref Token output, string value = null)
        {
            output = new TokenScanner(State, symbol, Position, value);
            column += symbol.Text(as_symbol: true).Length;
        }

        bool CheckSymbol(TokenId sysmbol, Regex re, ref Token output)
        {
            Match match;
            var result = (match = re.Match(buffer, column)).Success;
            if (result)
            {
                var value = match.Groups["text"].Value;
                output = new TokenScanner(State, sysmbol, Position, value);
                column += value.Length;
            }
            return result;
        }
    }
}
