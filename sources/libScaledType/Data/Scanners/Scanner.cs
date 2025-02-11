using System.Text;

namespace As.Tools.Data.Scanners
{
    /// <summary>
    /// Base class for scanner implementations.
    /// </summary>
    public abstract class Scanner : IScanner
    {
        protected const char NEWLINE = '\n';

        /// <summary>
        /// Known parser States.
        /// </summary>
        public enum ScannerState
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
        protected class ScannerToken(
            object state,
            object id,
            Position position,
            string? value = null,
            bool valueparts = false) :
            Token(state, id, position, value, valueparts)
        {
            /// <summary>
            /// Assign a _value to an token attribute.
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            public void SetValue(string key, string value)
            {
                if (Values == null) throw new AggregateException("Token without Values list");
                if (!Values.TryAdd(key, value)) Values[key] = value;
            }
        }

        /// <summary>
        /// Base class constructor for scanners.
        /// </summary>
        /// <param name="log">hook to give feedback on progress and problems.</param>
        protected Scanner(IProgressBar? progressbar)
        {
            Buffer = null;
            States = new Stack<object>();
            Lookahead = null;

            FileName = null;
            FileStream = null;
            InputStream = null;

            this.ProgressBar = progressbar;
            tryUseProgress = (progressbar is not null);

            IncludeNewlines = false; // add newlines to the Buffer.
            ScanNewlines = false; // treat newline as whitespace.
            ScanWhitespace = false; // treat whitespace not as a token.
        }

        protected Scanner()
        {
            Buffer = null;
            States = new Stack<object>();
            Lookahead = null;

            FileName = null;
            FileStream = null;
            InputStream = null;

            ProgressBar = null;
            tryUseProgress = false;

            IncludeNewlines = false; // false: add no newlines to the Buffer.
            ScanNewlines = false; // false: treat newline as whitespace.
            ScanWhitespace = false; // false: treat whitespace not as a token.
        }

        /// <summary>
        /// Initialise the scanner. (Read from file version)
        /// </summary>
        /// <param name="path">Path to text file</param>
        /// <param name="progressbar">Progress bar</param>
        /// <param name="log">hook to give feedback on progress and problems.</param>
        /// <remarks>When path==null: use Initialise(StreamReader) to set the InputStream stream</remarks>
        protected Scanner(string path, IProgressBar progressbar) : this(progressbar)
        {
            if (path != null) Initialise(path);
        }

        /// <summary>
        /// Initialise the scanner. (read from Buffer version)
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
        /// <param name="input">InputStream stream to use</param>
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
        bool UseProgress;

        /// <summary>
        /// Instance that takes the progress data
        /// </summary>
        readonly IProgressBar? ProgressBar;

        /// <summary>
        /// Scanner state stack
        /// </summary>
        readonly Stack<object> States;

        /// <summary>
        /// Current scanner state
        /// </summary>
        protected object State
        {
            get { return (States.Count == 0) ? StateNormal : States.Peek(); }
            private set
            {
                if (State != value)
                {
                    if (States.Count > 0) States.Pop();
                    PushState(value);
                    RetensionInput();
                }
            }
        }

        protected abstract object StateNormal { get; }
        protected abstract object StateError { get; }

        public virtual object TokenEOL { get { return TokenIdBase._EOL_; } }
        public virtual object TokenEOT { get { return TokenIdBase._EOT_; } }
        public virtual object TokenError { get { return TokenIdBase._ERROR_; } }
        public virtual object TokenComment {  get { return TokenIdBase._COMMENT_; } }

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
        /// Include newlines in Buffer.
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
            States.Push(value);
            RetensionInput();
        }

        /// <summary>
        /// Remover current scanner state to reveal the previous state, no changes if this is the last state
        /// </summary>
        public void PopState()
        {
            if (States.Count > 0)
            {
                States.Pop();
                RetensionInput();
            }
        }

        /// <summary>
        /// Reset InputStream pointer, clear Lookahead Buffer, rebuild current context.
        /// </summary>
        void RetensionInput()
        {
            if (Lookahead != null)
            {
                Token token = Lookahead;
                Lookahead = null;
                if (token.Position.Offset != Offset)
                {
                    SetPosition(token.Position.Offset, SeekOrigin.Begin);
                    Column = Buffer?.Length ?? 0;
                    Line = token.Position.Line;
                    Fetch();
                }
                Column = token.Position.Column;
            }
        }

        /// <summary>
        /// InputStream file if reading from file
        /// </summary>
        FileStream? FileStream;

        /// <summary>
        /// File name associated with the InputStream file
        /// </summary>
        public string? FileName { get; set; } = null;

        /// <summary>
        /// InputStream stream if reading from file or stream
        /// </summary>
        StreamReader? InputStream = null;

        /// <summary>
        /// Current Offset in InputStream stream.
        /// </summary>
        protected long Offset;

        /// <summary>
        /// Current Line number, starting from 1
        /// </summary>
        protected int Line;

        /// <summary>
        /// Current Column number, starting from 0
        /// </summary>
        protected int Column;

        public Position Position
        {
            get { return new Position(FileName, Offset, Line, Column); }
        }

        /// <summary>
        /// Current Line of text
        /// </summary>
        protected string? Buffer = null;

        /// <summary>
        /// Peek Lookahead Buffer
        /// </summary>
        ScannerToken? Lookahead;

        /// <summary>
        /// Initialise scanner with a new InputStream stream
        /// </summary>
        /// <param name="input">InputStream stream to use</param>
        public void Initialise(StreamReader input)
        {
            Close();
            this.InputStream = input;
            State = StateNormal;
        }

        /// <summary>
        /// Initialse scanner with a new file to read
        /// </summary>
        /// <param name="fileName">Name of the file to use as InputStream stream</param>
        public void Initialise(string fileName)
        {
            Close();
            FileName = fileName;
            UseProgress = false;
            if (tryUseProgress)
            {
                try
                {
                    FileInfo fi = new(fileName);
                    ulong len = (ulong)fi.Length;
                    if (len > 0)
                    {
                        ProgressBar?.SetProgressLoadMax(len);
                        UseProgress = true;
                    }
                }
                catch (Exception) // x)
                {
                    tryUseProgress = false;
                }
            }

            FileStream = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            if (FileStream == null) throw new FileLoadException(string.Format("Cannot open file '{0}'", FileName));
            InputStream = new StreamReader(FileStream);
            if (InputStream == null) throw new FileLoadException(string.Format("Cannot open streamreader for '{0}'", FileName));
            State = StateNormal;
        }

        /// <summary>
        /// Close up shop and leave
        /// </summary>
        public void Close()
        {
            if (InputStream != null)
            {
                InputStream.Close();
                InputStream = null;
            }
            if (FileStream != null)
            {
                FileStream.Close();
                FileStream = null;
            }
        }

        /// <summary>
        /// Check Buffer for a fixed text token.
        /// </summary>
        /// <param name="value">Value to check for</param>
        /// <param name="ignore_case">true: do case insensitieve comparison.</param>
        /// <returns>True if the next text sequence in thebuffer is containing matching</returns>
        protected bool At(string value, bool ignore_case = true)
        {
            var len = value.Length;
            var result = (Column + len <= (Buffer?.Length ?? 0));
            if (result) result = (string.Compare(Buffer, Column, value, 0, len, ignore_case) == 0);
            return result;
        }

        /// <summary>
        /// Look at the next token in the InputStream stream.
        /// </summary>
        /// <returns>The next token in the InputStream stream</returns>
        public Token PeekToken()
        {
            Lookahead ??= (ScannerToken)Scan();
            return Lookahead;
        }

        /// <summary>
        /// Consume the next token in de InputStream stream.
        /// </summary>
        /// <returns>The next token in the InputStream stream.</returns>
        public Token GetToken()
        {
            Token result;
            if (Lookahead == null) result = Scan();
            else
            {
                result = Lookahead;
                Lookahead = null;
            }
            return result;
        }

        /// <summary>
        /// Interface to scanner implementation.
        /// </summary>
        /// <param name="result">The next token from the InputStream stream (or null)</param>
        protected abstract void Scan(out Token result);

        /// <summary>
        /// Scan for the next token in the InputStream stream.
        /// </summary>
        /// <returns>The next token in the InputStream stream</returns>
        Token Scan()
        {
            Token result;
            do { Fetch(); } while (EatWhite());
            if (Buffer is null) result = new ScannerToken(State, TokenEOT, Position);
            else Scan(out result);
            if (result is null)
            {
                if (ScanNewlines && ((Buffer?[Column] ?? '\0') == NEWLINE))
                {
                    result = new ScannerToken(State, TokenEOT, Position);
                    Column++;
                }
                else
                {
                    result = new ScannerToken(State, TokenError, Position);
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
        protected Token ScanMultiline(object id, string? delimiter_open, string delimiter_close, bool ignoreCase = false)
        {
            Token? result = null;
            if (string.IsNullOrEmpty(delimiter_close))
            {
                result = new ScannerToken(State, id, Position);
            }
            else
            {
                if (string.IsNullOrEmpty(delimiter_open)) delimiter_open = null;

                var nests = 0;
                var p = Position;
                var value = new StringBuilder();
                var start = Column;
                do
                {
                    if
                        (
                            Buffer == null
                        )
                    {
                        Fetch();
                        if (Buffer is null)
                        {
                            result = new ScannerToken(State, TokenError, p, $"Multiline token: In '{id}'...; found EOT before a closing delimiter '{delimiter_close}'");
                        }
                    }
                    else if
                        (
                            Buffer.Length <= Column
                        )
                    {
                        if (start < Column) value.Append(Buffer.AsSpan(start));
                        if (!IncludeNewlines) value.Append(NEWLINE);
                        Column = Buffer.Length;
                        Fetch();
                        if (Buffer is null)
                        {
                            result = new ScannerToken(State, TokenError, p, $"Multiline token: In '{id}'...; found EOT before a closing delimiter '{delimiter_close}'");
                        }
                    }
                    else if
                        (
                            (delimiter_open != null) &&
                            (
                                !ignoreCase && (Buffer[Column] == delimiter_open[0]) ||
                                ignoreCase && (char.ToUpperInvariant(Buffer[Column]) == char.ToUpperInvariant(delimiter_open[0]))
                            ) &&
                            (delimiter_open.Length <= Buffer.Length - Column) &&
                            (string.Compare(Buffer.Substring(Column, delimiter_open.Length), delimiter_open, ignoreCase) == 0)
                        )
                    {
                        nests++;
                        Column += delimiter_open.Length;
                    }
                    else if
                        (
                            (
                                !ignoreCase && (Buffer[Column] == delimiter_close[0]) ||
                                ignoreCase && (char.ToUpperInvariant(Buffer[Column]) == char.ToUpperInvariant(delimiter_close[0]))
                            ) &&
                            (delimiter_close.Length <= Buffer.Length - Column) &&
                            (string.Compare(Buffer.Substring(Column, delimiter_close.Length), delimiter_close, ignoreCase) == 0)
                        )
                    {
                        if (0 < nests)
                        {
                            nests--;
                            Column += delimiter_open?.Length ?? 0;
                        }
                        else
                        {
                            var l = Column - start;
                            if (0 < l) value.Append(Buffer.AsSpan(start, l));
                            Column += delimiter_close.Length;
                            result = new ScannerToken(State, id, p, value.ToString());
                        }
                    }
                    else Column++;
                }
                while (result == null);
            }
            return result;
        }

        /// <summary>
        /// Eat white space from current position in the Buffer.
        /// </summary>
        /// <returns>True if the whitespace is at EOL, false otherwise.</returns>
        protected bool EatWhite()
        {
            if (ScanWhitespace) return false;
            bool result = false;
            if (Buffer is not null)
            {
                int max = Buffer.Length;
                while (Column < max)
                {
                    switch (Buffer[Column])
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
                        Column++;
                        break;
                    default:
                        goto Done;
                    }
                }
            Done:
                result = (max <= Column);
            }
            return result;
        }

        /// <summary>
        /// Refill the Line Buffer when the Buffer is null and the steam is not empty or when the current index is at or past end of Line.
        /// </summary>
        protected void Fetch()
        {
            if (InputStream is null) Buffer = null;
            else if (
                (Buffer is null) && !InputStream.EndOfStream ||
                (Buffer is not null) && (Buffer.Length <= Column)
            )
            {
                if (InputStream.EndOfStream) Buffer = null;
                else
                {
                    Offset = GetPosition();
                    if (UseProgress)
                    {
                        try
                        {
                            ProgressBar?.SetProgressLoad((ulong)Offset);
                        }
                        catch (Exception)
                        {
                            tryUseProgress = false;
                        }
                    }
                    Buffer = InputStream.ReadLine();
                    if (IncludeNewlines) Buffer += $"{NEWLINE}";
                    Line++;
                    Column = 0;
                }
            }
        }

        /// <summary>
        /// Get the current position from the InputStream stream, corrected for the current Line,=.
        /// </summary>
        /// <returns>Current position</returns>
        long GetPosition()
        {
            if (InputStream is null) return 0;

#if false
            // TODO: find workaround, this works in .NET Framework 4.8 but not in .NET 9

            // The current Buffer of decoded characters
            char[] charBuffer = (char[]?)InputStream.GetType().InvokeMember(
                "charBuffer"
                , BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField
                , null, InputStream, null
            ) ?? [];

            // The current position in the Buffer of decoded characters
            int charPos = (int?)InputStream.GetType().InvokeMember(
                "charPos"
                , BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField
                , null, InputStream, null
            ) ?? 0;

            // The number of bytes that the already-read characters need when encoded.
            int numReadBytes = InputStream.CurrentEncoding.GetByteCount(charBuffer, 0, charPos);

            // The number of encoded bytes that are in the current Buffer
            int byteLen = (int?)InputStream.GetType().InvokeMember(
                "byteLen"
                , BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField
                , null, InputStream, null
            ) ?? 0;

            return InputStream.BaseStream.Position - byteLen + numReadBytes;
#else
            if (!InputStream.BaseStream.CanSeek) return 0;

            // TODO: correct current position for encoding and unused characters in the scanner buffer.

            return InputStream.BaseStream.Position;
#endif
        }

        /// <summary>
        /// Change current position to a new one.
        /// </summary>
        /// <param name="offset">New position</param>
        /// <param name="origin">Reference point of the position</param>
        void SetPosition(long offset, SeekOrigin origin)
        {
            if (InputStream == null) return;
            InputStream.BaseStream.Seek(offset, origin);
            InputStream.DiscardBufferedData();
        }
    }
}

