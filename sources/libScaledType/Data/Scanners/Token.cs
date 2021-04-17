using System.Collections.Generic;
using System.Text;

namespace As.Tools.Data.Scanners
{
    /// <summary>
    /// Abstract Token: representing a token from an input stream.
    /// </summary>
    public abstract class Token
    {
        /// <summary>
        /// Token from a stream.
        /// </summary>
        /// <param name="position">Position in the stream.</param>
        /// <param name="id">Token identification.</param>
        /// <param name="symbol">Object of the id part.</param>
        /// <param name="value">Text representation of the value part</param>
        /// <param name="value_parts">Set true to use a list of (additional named) valuses.</param>
        public Token(object state, object symbol, Position position, string value, bool value_parts)
        {
            State = state;
            Symbol = symbol;
            Position = position;
            Value = value;
            Values = (value_parts)
                ? new Dictionary<string, string>()
                : null;
        }

        /// <summary>
        /// Scanner state with this token.
        /// </summary>
        public readonly object State;

        /// <summary>
        /// Token representation.
        /// </summary>
        public readonly object Symbol;

        /// <summary>
        /// Token identification.
        /// </summary>
        public int Id { get { return (int)Symbol; } }

        public string Name { get { return Symbol.ToString(); } }

        /// <summary>
        /// Position in the stream.
        /// </summary>
        public readonly Position Position;

        /// <summary>
        /// Text representation of the value part.
        /// </summary>
        public readonly string Value;

        /// <summary>
        /// Actual values for this token (as text representation in the stream).
        /// </summary>
        protected Dictionary<string, string> Values;

        /// <summary>
        /// Get a named value from this token.
        /// </summary>
        /// <param name="key">name of the value</param>
        /// <returns>text version from the stream of this value</returns>
        public string GetValue(string key)
        {
            if ((Values == null) || !Values.TryGetValue(key, out string result)) result = null;
            return result;
        }

        /// <summary>
        /// Buffer for ToString(). Do not use this variable, it assumed to be local static to ToString().
        /// </summary>
        string _tostring = null;

        /// <summary>
        /// Readable text representation of this token.
        /// </summary>
        /// <returns>Readable text representation.</returns>
        public override string ToString()
        {
            if (_tostring == null)
            {
                StringBuilder sb = new StringBuilder();
                if ((int)State != (int)Scanner._State.NORMAL)
                {
                    sb.Append("<");
                    sb.Append(State.ToString());
                    sb.Append(">");
                }
                sb.Append("(Id='");
                sb.Append(Symbol.ToString());
                sb.Append("', ");
                sb.Append(Position.ToString());
                if (Value != null)
                {
                    sb.Append(", val='");
                    sb.Append(Value);
                    sb.Append('\'');
                }
                if (Values != null)
                {
                    foreach (var kv in Values)
                    {
                        sb.Append(", ");
                        sb.Append(kv.Key);
                        sb.Append("='");
                        sb.Append(kv.Value);
                        sb.Append('\'');
                    }
                }
                sb.Append(')');
                _tostring = sb.ToString();
            }
            return _tostring;
        }
    }
}
