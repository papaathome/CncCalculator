using As.Tools.Data.Scanners;

namespace As.Tools.Data.Parsers
{
    /// <summary>
    /// Base class for parser implementations
    /// </summary>
    public abstract class Parser
    {
#if USE_LOG4NET
        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(nameof(Parser));
#endif

        /// <summary>
        /// .ctor for the parser, given a scanner.
        /// </summary>
        /// <param name="scanner">Scanner to use</param>
        /// <param name="log">hook to give feedback on progress and problems.</param>
        protected Parser(IScanner scanner, IBuilder? builder = null)
        {
            Scanner = scanner ?? throw new ArgumentNullException(nameof(scanner));
            Builder = builder;

            HandleErrors = true;
            SkipComment = true;
            SkipEol = true;
        }

        /// <summary>
        /// Interface to the parser implementation
        /// </summary>
        abstract protected void ParseGrammar();

        /// <summary>
        /// Parse all data, optionally use a Builder for the result
        /// </summary>
        /// <param name="builder">Builder accepting parse results</param>
        public void Execute(IBuilder? builder = null)
        {
            Builder = builder;
            try
            {
                ParseTried = true;
                ParseOK = false;
                ParseError = null;
                builder?.BeginLoadData();
                ParseGrammar();
                var token = GetToken();

                if (token.Id == (int)Scanner.TokenEOT) ParseOK = true;
                else Error($"Expected EOT token {Scanner.TokenEOT}", token);
            }
            catch (ParserException x)
            {
#if USE_LOG4NET
                log.Error("Parse problem", x);
#endif
                ParseError = x;
            }
            catch (Exception x)
            {
#if USE_LOG4NET
                log.Error("Parser problem", x);
#endif
                ParseError = x;
            }
            finally
            {
                builder?.EndLoadData();
            }
        }

        /// <summary>
        /// Scanner to use while parsing
        /// </summary>
        protected IScanner Scanner { get; private set; }

        /// <summary>
        /// Builder to use while parsing
        /// </summary>
        IBuilder? Builder;

        /// <summary>
        /// Has the parse already been tried?
        /// </summary>
        public bool ParseTried { get; private set; }

        /// <summary>
        /// Was the tried parse successfull?
        /// </summary>
        public bool ParseOK { get; private set; }

        /// <summary>
        /// Last major problem seen before the parse failed
        /// </summary>
        public Exception? ParseError { get; private set; }

        /// <summary>
        /// Look for the next token
        /// </summary>
        /// <returns>The next token</returns>
        protected Token PeekToken()
        {
            return Fetch();
        }

        protected bool TryPeek(out Token t, object symbol)
        {
            t = Fetch();
            return (t.Id == (int)symbol);
        }

        /// <summary>
        /// Consume the next token
        /// </summary>
        /// <returns>The next token</returns>
        protected Token GetToken()
        {
            return Fetch(true);
        }

        /// <summary>
        /// Handle error tokens in fetch phase (when set true)
        /// </summary>
        protected bool HandleErrors { get; set; }

        /// <summary>
        /// Skip comment tokens in fetch phase (when set true)
        /// </summary>
        protected bool SkipComment { get; set; }

        /// <summary>
        /// Skip EOL tokens in fetch phase (when set true)
        /// </summary>
        protected bool SkipEol { get; set; }

        protected List<int> CommentToken = [];

        /// <summary>
        /// Fetch tokens from the scanner until a usefull one or an error.
        /// </summary>
        /// <param name="get">get (true) or Lookahead (false)</param>
        /// <returns>The next usefull token form the scanner, or an error</returns>
        Token Fetch(bool get = false)
        {
            Token token;
            bool skip = true;
            do
            {
                token = (get)
                    ? Scanner.GetToken()
                    : Scanner.PeekToken();
                if (CommentToken.Contains(token.Id))
                {
                    skip = SkipComment;
                    if (skip && !get) Scanner.GetToken();
                }
                else
                {
                    switch (token.Id)
                    {
                        case (int)TokenIdBase._COMMENT_:
                            if (SkipComment)
                            {
                                if (!get) Scanner.GetToken();
                            }
                            else skip = false;
                            break;
                        case (int)TokenIdBase._ERROR_:
                            if (HandleErrors)
                            {
                                Error("Scanner error", token);
                            }
                            skip = false;
                            break;
                        case (int)TokenIdBase._EOL_:
                            if (SkipEol)
                            {
                                if (!get) Scanner.GetToken();
                            }
                            else skip = false;
                            break;
                        default:
                            skip = false;
                            break;
                    }
                }
            }
            while (skip);
            return token;
        }

        /// <summary>
        /// Bumped on an unexpected token.
        /// </summary>
        /// <param name="token">Actual token found</param>
        /// <param name="msg">More details from the parser implementation</param>
        static void UnexpectedTokenError(Token token, string msg)
        {
            throw new ParserException(token, $"Unexpected token: {msg}");
        }

        /// <summary>
        /// Bumped on a problem.
        /// </summary>
        /// <param name="msg">More detaild from the parser implementation</param>
        /// <param name="token">Last token seen</param>
        static void Error(string msg, Token token)
        {
            throw new ParserException(token, msg);
        }
    }
}
