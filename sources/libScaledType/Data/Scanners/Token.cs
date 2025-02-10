using System.Text;

namespace As.Tools.Data.Scanners
{
    /// <summary>
    /// Abstract Token: representing a token from an InputStream stream.
    /// </summary>
    public abstract class Token
    {
        /// <summary>
        /// Token from a stream.
        /// </summary>
        /// <param name="scanner_state">Boxed scanner scanner_state with this token, type not disclosed.</param>
        /// <param name="symbol">Boxed token identification.</param>
        /// <param name="position">Position in the stream.</param>
        /// <param name="value">Text representation of the _value part</param>
        /// <param name="value_parts">Set true to use a list of (additional named) valuses.</param>
        #pragma warning disable IDE0290 // Use primary constructor
        public Token(
        #pragma warning restore IDE0290 // Use primary constructor
            object scanner_state,
            object symbol,
            Position position,
            string? value,
            bool value_parts)
        {
            ScannerState = scanner_state;
            Symbol = symbol;
            Position = position;
            Value = value;
            Values = (value_parts) ? [] : null;
        }

        /// <summary>
        /// Boxed scanner scanner_state with this token, type not disclosed.
        /// </summary>
        /// <remarks>ScannerState is (or inherits from) a `ScannerState'</remarks>
        public object ScannerState { get; }

        /// <summary>
        /// Boxed token identification.
        /// </summary>
        /// <remarks>Symbol is a boxed `enum TokenId : int' which is definded by the scanner user.</remarks>
        public object Symbol { get; }

        /// <summary>
        /// Token identification.
        /// </summary>
        public int Id { get { return (int)Symbol; } }

        public string Name { get { return $"{Symbol}"; } }

        /// <summary>
        /// Position in the stream.
        /// </summary>
        public readonly Position Position;

        /// <summary>
        /// Text representation of the _value part.
        /// </summary>
        public readonly string? Value;

        /// <summary>
        /// Actual values for this token (as text representation in the stream).
        /// </summary>
        protected Dictionary<string, string>? Values;

        /// <summary>
        /// Get a named _value from this token.
        /// </summary>
        /// <param name="key">name of the _value</param>
        /// <returns>text version from the stream of this _value</returns>
        public string? GetValue(string key)
        {
            if ((Values is null) || !Values.TryGetValue(key, out string? result)) result = null;
            return result;
        }

        /// <summary>
        /// Buffer for ToString(). Do not use this variable, it assumed to be local static to ToString().
        /// </summary>
        string? _tostring = null;

        /// <summary>
        /// Readable text representation of this token.
        /// </summary>
        /// <returns>Readable text representation.</returns>
        public override string? ToString()
        {
            if (_tostring is null)
            {
                StringBuilder sb = new();
                if ((int)ScannerState != (int)Scanner.ScannerState.NORMAL)
                {
                    sb.Append('<');
                    sb.Append(ScannerState.ToString());
                    sb.Append('>');
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
