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
            ERROR = ScannerState.ERROR,

            /// <summary>
            /// SOF . '...' EOF ; the normal state.
            /// </summary>
            NORMAL = ScannerState.NORMAL,

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

        public ScaleScanner(IProgressBar? progressbar = null) : base(progressbar) { }

        public ScaleScanner(string path, IProgressBar progressbar) : base(path, progressbar) { }

        public ScaleScanner(string buffer) : base(buffer) { }

        public ScaleScanner(StreamReader input, IProgressBar progressbar) : base(input, progressbar) { }

        readonly Regex NAME = TokenId.NAME.Regex(REGEX_OPTIONS);
        readonly Regex INT = TokenId.INT.Regex(REGEX_OPTIONS);

        protected override void Scan(out Token result)
        {
            switch (State)
            {
                case States.NORMAL:
                    if ((Buffer == null) || (Buffer.Length <= Column)) goto case States.ERROR;
                    switch (Buffer[Column])
                    {
                        case '[': SetSymbol(TokenId.BRACKET_OPEN, out result); break;
                        case ']': SetSymbol(TokenId.BRACKET_CLOSE, out result); break;
                        case '(': SetSymbol(TokenId.PARENTESES_OPEN, out result); break;
                        case ')': SetSymbol(TokenId.PARENTESES_CLOSE, out result); break;
                        case '^': SetSymbol(TokenId.PWR, out result); break;
                        case '/': SetSymbol(TokenId.DIV, out result); break;
                        case '#': SetSymbol(TokenId.NAME, out result, "#"); break;
                        default:
                            if (TryGetSymbol(TokenId.INT, INT, out Token? r))
                            {
                                result = r!;
                                break;
                            }
                            if (TryGetSymbol(TokenId.NAME, NAME, out r))
                            {
                                result = r!;
                                break;
                            }
                            result = new ScannerToken(State, TokenId._ERROR_, Position, "Scanner no token available.");
                            SetState((int)States.ERROR);
                            break;
                    }
                    break;
                case States.ERROR:
                    result = new ScannerToken(State, TokenId._ERROR_, Position, "Scanner no token recognised.");
                    break;
                default:
                    result = new ScannerToken(State, TokenId._ERROR_, Position, $"Scanner state not recognised: {State}");
                    SetState((int)States.ERROR);
                    break;
            }
        }

        void SetSymbol(TokenId symbol, out Token output, string? value = null)
        {
            output = new ScannerToken(State, symbol, Position, value);
            Column += symbol.Text(as_symbol: true).Length;
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
