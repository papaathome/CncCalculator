namespace As.Tools.Data.Scanners
{
    /// <summary>
    /// Public part of the scanner interface.
    /// </summary>
    public interface IScanner
    {
        /// <summary>
        /// InputStream file (null if not nusing a file)
        /// </summary>
        string? FileName { get; }

        /// <summary>
        /// Include newlines in Buffer.
        /// </summary>
        bool IncludeNewlines { get; set; }

        /// <summary>
        /// Produce tokens for newline (true) or handle as whitespace (false).
        /// </summary>
        bool ScanNewlines { get; set; }

        /// <summary>
        /// Produce tokens for whitespace or skip whitespace. (false).
        /// </summary>
        bool ScanWhitespace { get; set; }

        /// <summary>
        /// Current parser state, top of States stack.
        /// </summary>
        /// <returns></returns>
        object GetState();

        /// <summary>
        /// Change current parser state, replace top of States stack.
        /// </summary>
        /// <param name="value">New parster state</param>
        /// <remarks>Lookahead Buffer is flushed</remarks>
        void SetState(object value);

        /// <summary>
        /// Change current parser state, push new on top of States stack.
        /// </summary>
        /// <param name="value">New parser state</param>
        /// <remarks>Lookahead Buffer is flushed</remarks>
        void PushState(object value);

        /// <summary>
        /// Change current parser state, pop current state from top of States stack.
        /// No changes if the stack has only one state left.
        /// </summary>
        /// <remarks>Lookahead Buffer is flushed</remarks>
        void PopState();

        /// <summary>
        /// Get the next available token from the InputStream stream.
        /// </summary>
        /// <returns>The next token which might be ERROR or EOS or any other one defined.</returns>
        Token GetToken();

        /// <summary>
        /// Look ahead at the next available token from the InputStream stream
        /// </summary>
        /// <returns>The next available token which might be ERROR or EOS or any other one defined.</returns>
        Token PeekToken();

        object TokenEOL { get; }
        object TokenEOT { get; }
        object TokenError { get; }
        object TokenComment { get; }

        Position Position { get; }
    }
}
