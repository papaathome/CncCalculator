using System;
using System.IO;

namespace As.Tools.Data.Scanners
{
    /// <summary>
    /// Scanner position details for the start of a token.
    /// </summary>
    public sealed class Position
    {
        /// <summary>
        /// Position, for internal use with the scanner providing information on location of problems.
        /// </summary>
        public Position(String filename, long offset, int line, int column)
        {
            this.line = line;
            this.column = column;
            this.offset = offset;
            this.filename = filename;
        }

        /// <summary>
        /// Limited position, for internal use with the scanner providing information on location of problems.
        /// </summary>
        public Position(int line, int column) : this(null, 0, line, column)
        { }

        /// <summary>
        /// Line number.
        /// </summary>
        public readonly int line;

        /// <summary>
        /// Column number on the current line.
        /// </summary>
        public readonly int column;

        /// <summary>
        /// Offset into the stream, start read position for the current token.
        /// </summary>
        public readonly long offset;

        /// <summary>
        /// Filename for the stream, null if stream is not associated with a file.
        /// </summary>
        public readonly String filename;

        /// <summary>
        /// Human readable representation of a position.
        /// </summary>
        /// <returns>Human readable representation of a position.</returns>
        override public string ToString()
        {
            return (String.IsNullOrWhiteSpace(filename))
                ? string.Format("(l={0}, c={1})", line, column + 1)
                : string.Format("(f='{2}', l={0}, c={1})", line, column + 1, Path.GetFileName(filename));
        }
    }
}
