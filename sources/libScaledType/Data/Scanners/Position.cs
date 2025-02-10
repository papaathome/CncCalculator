namespace As.Tools.Data.Scanners
{
    /// <summary>
    /// Scanner details for the start position of a token.
    /// </summary>
    /// <remarks>
    /// Position in file, for internal use with the scanner providing information on location of problems.
    /// </remarks>
    /// <param name="filename">Name of file used as InputStream stream</param>
    /// <param name="offset">Offset into the InputStream file to start of Line</param>
    /// <param name="line">Line number of the Line with this position, counting from 1</param>
    /// <param name="column">Column number within the Line with this position, counting from 0</param>
    public sealed class Position(
        string? filename,
        long offset,
        int line,
        int column)
    {
        /// <summary>
        /// Position in stream, for internal use with the scanner providing information on location of problems.
        /// </summary>
        /// <param name="line">Line number of the Line with this position, counting from 1</param>
        /// <param name="column">Column number within the Line with this position, counting from 0</param>
        public Position(int line, int column) : this("", -1, line, column) { }

        /// <summary>
        /// Line number of the Line with this position, counting from 1.
        /// </summary>
        public int Line { get; } = line;

        /// <summary>
        /// Column number within the Line with this position, counting from 0.
        /// </summary>
        public int Column { get; } = column;

        /// <summary>
        /// Offset into the InputStream file to start of Line, unpositional streams use an Offset of -1.
        /// </summary>
        public long Offset { get; } = offset;

        /// <summary>
        /// >Name of file used as InputStream stream.
        /// </summary>
        public string? FileName { get; } = filename;

        /// <summary>
        /// Readable representation of a position.
        /// </summary>
        /// <returns>Human readable representation of a position.</returns>
        override public string ToString()
        {
            return (string.IsNullOrWhiteSpace(FileName))
                ? string.Format("(l={0}, c={1})", Line, Column + 1)
                : string.Format("(f='{2}', l={0}, c={1})", Line, Column + 1, Path.GetFileName(FileName));
        }
    }
}
