using System.Runtime.Serialization;

namespace As.Tools.Data.Parsers
{
    /// <summary>
    /// Parser exception, indicating parser problems.
    /// </summary>
    [Serializable]
    public class ParserException : Exception, ISerializable
    {
        /// <summary>
        /// Plain exception, no extra details available.
        /// </summary>
        public ParserException() : base()
        {
            token = null;
            stack = null;
        }

        /// <summary>
        /// Exception with some local details available.
        /// </summary>
        /// <param name="message">local details</param>
        public ParserException(string message) : base(message)
        {
            token = null;
            stack = null;
        }

        /// <summary>
        /// Exception with some local details and an exception at a lower level triggering this one.
        /// </summary>
        /// <param name="message">local details</param>
        /// <param name="innerException">lower level exception</param>
        public ParserException(string message, Exception innerException) : base(message, innerException)
        {
            token = null;
            stack = null;
        }

        /// <summary>
        /// Exception with some current token context details available.
        /// </summary>
        /// <param name="token">token context</param>
        public ParserException(object token) : base()
        {
            this.token = token?.ToString();
            this.stack = null;
        }

        /// <summary>
        /// Exception with some frame stack context details and current token context details available.
        /// </summary>
        /// <param name="stack">frame stack context</param>
        /// <param name="token">token context</param>
        public ParserException(object stack, object token) : base()
        {
            this.token = token?.ToString();
            this.stack = stack?.ToString();
        }

        /// <summary>
        /// Exception with some current token context details available.
        /// </summary>
        /// <param name="token">token context</param>
        /// <param name="message">local details</param>
        public ParserException(object token, string message) : base(message)
        {
            this.token = token?.ToString();
            this.stack = null;
        }

        /// <summary>
        /// Exception with some local details, frame stack context and current token context details available.
        /// </summary>
        /// <param name="stack">frame stack context</param>
        /// <param name="token">token context</param>
        /// <param name="message">local details</param>
        public ParserException(object stack, object token, string message) : base(message)
        {
            this.token = token?.ToString();
            this.stack = stack?.ToString();
        }

        /// <summary>
        /// Exception with some current token context details and an exception at a lower level triggering this one.
        /// </summary>
        /// <param name="token">token context</param>
        /// <param name="message">local details</param>
        /// <param name="innerException">exception at a lower level</param>
        public ParserException(object token, string message, Exception innerException) : base(message, innerException)
        {
            this.token = token?.ToString();
            this.stack = null;
        }

        /// <summary>
        /// Exception with some local details, frame stack context and current token context details and an exception at a lower level triggering this one.
        /// </summary>
        /// <param name="stack">frame stack context</param>
        /// <param name="token">token context</param>
        /// <param name="message">local details</param>
        /// <param name="innerException">exception at a lower level</param>
        public ParserException(object stack, object token, string message, Exception innerException) : base(message, innerException)
        {
            this.token = token?.ToString();
            this.stack = stack?.ToString();
        }

        /// <summary>
        /// Exception message
        /// </summary>
        public override string Message
        {
            get
            {
                return (string.IsNullOrWhiteSpace(stack))
                    ? string.Format(
                          string.IsNullOrWhiteSpace(base.Message) ? "Token: {1};" : "{0}; Token: {1};",
                          base.Message,
                          string.IsNullOrWhiteSpace(token) ? "-" : token
                      )
                    : string.Format(
                          string.IsNullOrWhiteSpace(base.Message) ? "Token: {1}; Stack: {2};" : "{0}; Token: {1}; Stack: {2};",
                          base.Message,
                          string.IsNullOrWhiteSpace(token) ? "-" : token,
                          stack
                      );
            }
        }

        /// <summary>
        /// Readable text representation of the token context.
        /// </summary>
        public readonly string? token;

        /// <summary>
        /// Readable text representation of the frame stack context.
        /// </summary>
        public readonly string? stack;

        public override string ToString()
        {
            return Message;
        }
    }
}
