using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace As.Tools.Data.Scanners
{
    /// <summary>
    /// Base class for scanner implementations.
    /// </summary>
    public abstract class Scanner : IScanner
    {
#if USE_LOG4NET
        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
#endif
        protected const char NEWLINE = '\n';

        /// <summary>
        /// Known parser states.
        /// </summary>
        public enum _State
        {
            /// <summary>
            /// The normal state.
            /// </summary>
            NORMAL = 0,

            /// <summary>
            /// The error state.
            /// </summary>
            ERROR = -1
        }

        /// <summary>
        /// Token extended with scanner state details.
        /// </summary>
        protected class TokenScanner : Token
        {
            public TokenScanner(object state, object id, Position position, string value=null, bool valueparts=false)
                : base(state, id, position, value, valueparts)
            { }

            /// <summary>
            /// Assign a value to an token attribute.
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            public void SetValue(string key, string value)
            {
                if (Values == null) throw new AggregateException("Token without Values list");
                if (Values.ContainsKey(key)) Values[key] = value;
                else Values.Add(key, value);
            }
        }

        /// <summary>
        /// Base class constructor for scanners.
        /// </summary>
        /// <param name="log">hook to give feedback on progress and problems.</param>
        protected Scanner(IProgressBar progressbar)
        {
            buffer = null;
            states = new Stack<object>();
            lookahead = null;

            FileName = null;
            fs = null;
            input = null;

            this.progressbar = progressbar;
            tryUseProgress = (progressbar != null);

            IncludeNewlines = false; // add newlines to the buffer.
            ScanNewlines = false; // treat newline as whitespace.
            ScanWhitespace = false; // treat whitespace not as a token.
        }

        protected Scanner() : this((IProgressBar)null)
        { }

        /// <summary>
        /// Initialise the scanner. (Read from file version)
        /// </summary>
        /// <param name="path">Path to text file</param>
        /// <param name="progressbar">Progress bar</param>
        /// <param name="log">hook to give feedback on progress and problems.</param>
        /// <remarks>When path==null: use Initialise(StreamReader) to set the input stream</remarks>
        protected Scanner(string path, IProgressBar progressbar) : this(progressbar)
        {
            if (path != null) Initialise(path);
        }

        /// <summary>
        /// Initialise the scanner. (read from buffer version)
        /// </summary>
        /// <param name="buffer">Text to parse</param>
        /// <param name="log">hook to give feedback on progress and problems.</param>
        protected Scanner(string buffer) : this()
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(buffer);
            writer.Flush();
            stream.Position = 0;
            Initialise(new StreamReader(stream));
        }

        /// <summary>
        /// Initialise the scanner. (read from stream version)
        /// </summary>
        /// <param name="input">input stream to use</param>
        /// <param name="log">hook to give feedback on progress and problems.</param>
        protected Scanner(StreamReader input, IProgressBar progressbar) : this(progressbar)
        {
            if (input != null) Initialise(input);
        }

        /// <summary>
        /// Clean up leftovers.
        /// </summary>
        ~Scanner()
        {
            Close();
        }

        /// <summary>
        /// At first appropriate moment try to use the progress bar.
        /// </summary>
        bool tryUseProgress;

        /// <summary>
        /// On all subsequent moments use the progress bar.
        /// </summary>
        bool useProgress;

        /// <summary>
        /// Instance that takes the progress data
        /// </summary>
        IProgressBar progressbar;

        /// <summary>
        /// Scanner state stack
        /// </summary>
        Stack<object> states;

        /// <summary>
        /// Current scanner state
        /// </summary>
        protected object State
        {
            get { return (states.Count == 0) ? StateNormal : states.Peek(); }
            private set
            {
                if (State != value)
                {
                    if (states.Count > 0) states.Pop();
                    PushState(value);
                    RetensionInput();
                }
            }
        }

        protected abstract object StateNormal { get; }
        protected abstract object StateError { get; }

        public virtual object TokenEOL { get { return _TokenId._EOL_; } }
        public virtual object TokenEOT { get { return _TokenId._EOT_; } }
        public virtual object TokenError { get { return _TokenId._ERROR_; } }
        public virtual object TokenComment {  get { return _TokenId._COMMENT_; } }

        /// <summary>
        /// Inform caller about the current scanner state
        /// </summary>
        /// <returns>Current parser state</returns>
        public object GetState() { return State; }

        /// <summary>
        /// Replace the current scanner state.
        /// </summary>
        /// <param name="value">New scanner state</param>
        public void SetState(object value) { State = value; }

        /// <summary>
        /// Include newlines in buffer.
        /// </summary>
        public bool IncludeNewlines { get; set; }

        /// <summary>
        /// Produce tokens for newline (true) or handle as whitespace (false).
        /// </summary>
        public bool ScanNewlines { get; set; }

        /// <summary>
        /// Produce tokens for whitespace or skip whitespace. (false).
        /// </summary>
        public bool ScanWhitespace { get; set; }

        /// <summary>
        /// Push new state on top of the current scanner state.
        /// </summary>
        /// <param name="value">New scanner state</param>
        public void PushState(object value)
        {
            states.Push(value);
            RetensionInput();
        }

        /// <summary>
        /// Remover current scanner state to reveal the previous state, no changes if this is the last state
        /// </summary>
        public void PopState()
        {
            if (states.Count > 0)
            {
                states.Pop();
                RetensionInput();
            }
        }

        /// <summary>
        /// Reset input pointer, clear lookahead buffer, rebuild current context.
        /// </summary>
        void RetensionInput()
        {
            if (lookahead != null)
            {
                Token token = lookahead;
                lookahead = null;
                if (token.Position.offset != offset)
                {
                    SetPosition(token.Position.offset, SeekOrigin.Begin);
                    column = buffer.Length;
                    line = token.Position.line;
                    Fetch();
                }
                column = token.Position.column;
            }
        }

        /// <summary>
        /// Input file if reading from file
        /// </summary>
        FileStream fs;

        /// <summary>
        /// File name associated with the input file
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Input stream if reading from file or stream
        /// </summary>
        StreamReader input;

        /// <summary>
        /// Current offset in input stream.
        /// </summary>
        protected long offset;

        /// <summary>
        /// Current line number, starting from 1
        /// </summary>
        protected int line;

        /// <summary>
        /// Current column number, starting from 0
        /// </summary>
        protected int column;

        public Position Position
        {
            get { return new Position(FileName, offset, line, column); }
        }

        /// <summary>
        /// Current line of text
        /// </summary>
        protected String buffer;

        /// <summary>
        /// Peek lookahead buffer
        /// </summary>
        TokenScanner lookahead;

        /// <summary>
        /// Initialise scanner with a new input stream
        /// </summary>
        /// <param name="input">input stream to use</param>
        public void Initialise(StreamReader input)
        {
            Close();
            this.input = input;
            State = StateNormal;
        }

        /// <summary>
        /// Initialse scanner with a new file to read
        /// </summary>
        /// <param name="fileName">Name of the file to use as input stream</param>
        public void Initialise(string fileName)
        {
            Close();
            FileName = fileName;
            useProgress = false;
            if (tryUseProgress)
            {
                try
                {
                    FileInfo fi = new FileInfo(fileName);
                    ulong len = (ulong)fi.Length;
                    if (len > 0)
                    {
                        progressbar.SetProgressLoadMax(len);
                        useProgress = true;
                    }
                }
                catch (Exception) // x)
                {
                    tryUseProgress = false;
                }
            }

            fs = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            if (fs == null) throw new FileLoadException(string.Format("Cannot open file '{0}'", FileName));
            input = new StreamReader(fs);
            if (input == null) throw new FileLoadException(string.Format("Cannot open streamreader for '{0}'", FileName));
            State = StateNormal;
        }

        /// <summary>
        /// Close up shop and leave
        /// </summary>
        public void Close()
        {
            if (input != null)
            {
                input.Close();
                input = null;
            }
            if (fs != null)
            {
                fs.Close();
                fs = null;
            }
        }

        /// <summary>
        /// Check buffer for a fixed text token.
        /// </summary>
        /// <param name="value">Value to check for</param>
        /// <param name="ignore_case">true: do case insensitieve comparison.</param>
        /// <returns>True if the next text sequence in thebuffer is containing matching</returns>
        protected bool At(string value, bool ignore_case = true)
        {
            var len = value.Length;
            var result = (column + len <= buffer.Length);
            if (result) result = (string.Compare(buffer, column, value, 0, len, ignore_case) == 0);
            return result;
        }

        /// <summary>
        /// Look at the next token in the input stream.
        /// </summary>
        /// <returns>The next token in the input stream</returns>
        public Token PeekToken()
        {
            if (lookahead == null) lookahead = (TokenScanner)Scan();
            return lookahead;
        }

        /// <summary>
        /// Consume the next token in de input stream.
        /// </summary>
        /// <returns>The next token in the input stream.</returns>
        public Token GetToken()
        {
            Token result;
            if (lookahead == null) result = Scan();
            else
            {
                result = lookahead;
                lookahead = null;
            }
            return result;
        }

        /// <summary>
        /// Interface to scanner implementation.
        /// </summary>
        /// <param name="result">The next token from the input stream (or null)</param>
        protected abstract void Scan(ref Token result);

        /// <summary>
        /// Scan for the next token in the input stream.
        /// </summary>
        /// <returns>The next token in the input stream</returns>
        Token Scan()
        {
            Token result = null;
            do { Fetch(); } while (EatWhite());
            if (buffer == null) result = new TokenScanner(State, TokenEOT, Position);
            else Scan(ref result);
            if (result == null)
            {
                if (ScanNewlines && (buffer[column] == NEWLINE))
                {
                    result = new TokenScanner(State, TokenEOT, Position);
                    column++;
                }
                else
                {
                    result = new TokenScanner(State, TokenError, Position);
                    State = StateError;
                }
            }
            return result;
        }

        // multiline scan without nesting.
        protected Token ScanMultiline(object id, string delimiter, bool ignoreCase = false)
        {
            return ScanMultiline(id, null, delimiter, ignoreCase);
        }

        // multiline scan with nesting.
        protected Token ScanMultiline(object id, string delimiter_open, string delimiter_close, bool ignoreCase = false)
        {
            Token result = null;
            if (string.IsNullOrEmpty(delimiter_close))
            {
                result = new TokenScanner(State, id, Position);
            }
            else
            {
                if (string.IsNullOrEmpty(delimiter_open)) delimiter_open = null;

                var nests = 0;
                var p = Position;
                var value = new StringBuilder();
                var start = column;
                do
                {
                    if (buffer.Length <= column)
                    {
                        if (start < column) value.Append(buffer.Substring(start));
                        if (!IncludeNewlines) value.Append(NEWLINE);
                        column = buffer.Length;
                        Fetch();
                        if (buffer == null)
                        {
                            result = new TokenScanner(State, TokenError, p, $"Multiline token: In '{id}'...; found EOT before a closing delimiter '{delimiter_close}'");
                        }
                    }
                    else if (
                        (delimiter_open != null) &&
                        (
                            !ignoreCase && (buffer[column] == delimiter_open[0]) ||
                            ignoreCase && (char.ToUpperInvariant(buffer[column]) == char.ToUpperInvariant(delimiter_open[0]))
                        ) &&
                        (delimiter_open.Length <= buffer.Length - column) &&
                        (string.Compare(buffer.Substring(column, delimiter_open.Length), delimiter_open, ignoreCase) == 0)
                    )
                    {
                        nests++;
                        column += delimiter_open.Length;
                    }
                    else if (
                        (
                            !ignoreCase && (buffer[column] == delimiter_close[0]) ||
                            ignoreCase && (char.ToUpperInvariant(buffer[column]) == char.ToUpperInvariant(delimiter_close[0]))
                        ) &&
                        (delimiter_close.Length <= buffer.Length - column) &&
                        (string.Compare(buffer.Substring(column, delimiter_close.Length), delimiter_close, ignoreCase) == 0)
                    )
                    {
                        if (0 < nests)
                        {
                            nests--;
                            column += delimiter_open.Length;
                        }
                        else
                        {
                            var l = column - start;
                            if (0 < l) value.Append(buffer.Substring(start, l));
                            column += delimiter_close.Length;
                            result = new TokenScanner(State, id, p, value.ToString());
                        }
                    }
                    else column++;
                }
                while (result == null);
            }
            return result;
        }

        /// <summary>
        /// Eat white space from current position in the buffer.
        /// </summary>
        /// <returns>True if the whitespace is at EOL, false otherwise.</returns>
        protected bool EatWhite()
        {
            if (ScanWhitespace) return false;
            bool result = false;
            if (buffer != null)
            {
                int max = buffer.Length;
                while (column < max)
                {
                    switch (buffer[column])
                    {
                    case NEWLINE:
                        if (ScanNewlines) goto Done;
                        goto EatWhite;
                    case ' ':
                    case '\f':
                    case '\r':
                    case '\t':
                    case '\v':
                    //case '\x85':   elipsis. Not ECMAScript-compliant.
                    //case '\p{Z}':  Matches any separator character. Not ECMAScript-compliant and not in UNICODE character class.
                    EatWhite:
                        column++;
                        break;
                    default:
                        goto Done;
                    }
                }
            Done:
                result = (max <= column);
            }
            return result;
        }

        /// <summary>
        /// Refill the line buffer when the buffer is null and the steam is not empty or when the current index is at or past end of line.
        /// </summary>
        protected void Fetch()
        {
            if (input == null) buffer = null;
            else if (
                (buffer == null) && !input.EndOfStream ||
                (buffer != null) && (buffer.Length <= column)
            )
            {
                if (input.EndOfStream) buffer = null;
                else
                {
                    offset = GetPosition();
                    if (useProgress)
                    {
                        try
                        {
                            progressbar.SetProgressLoad((ulong)offset);
                        }
                        catch (Exception)
                        {
                            tryUseProgress = false;
                        }
                    }
                    buffer = input.ReadLine();
                    if (IncludeNewlines) buffer += $"{NEWLINE}";
                    line++;
                    column = 0;
                }
            }
        }

        /// <summary>
        /// Get the current position from the input stream, corrected for the current line,=.
        /// </summary>
        /// <returns>Current position</returns>
        long GetPosition()
        {
            if (input == null) return 0;
            // The current buffer of decoded characters
            char[] charBuffer = (char[])input.GetType().InvokeMember(
                "charBuffer"
                , BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField
                , null, input, null
            );

            // The current position in the buffer of decoded characters
            int charPos = (int)input.GetType().InvokeMember(
                "charPos"
                , BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField
                , null, input, null
            );

            // The number of bytes that the already-read characters need when encoded.
            int numReadBytes = input.CurrentEncoding.GetByteCount(charBuffer, 0, charPos);

            // The number of encoded bytes that are in the current buffer
            int byteLen = (int)input.GetType().InvokeMember(
                "byteLen"
                , BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField
                , null, input, null
            );

            return input.BaseStream.Position - byteLen + numReadBytes;
        }

        /// <summary>
        /// Change current position to a new one.
        /// </summary>
        /// <param name="offset">New position</param>
        /// <param name="origin">Reference point of the position</param>
        void SetPosition(long offset, SeekOrigin origin)
        {
            if (input == null) return;
            input.BaseStream.Seek(offset, origin);
            input.DiscardBufferedData();
        }
    }
}

