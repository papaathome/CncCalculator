namespace As.Tools.Data.Scanners
{
    /// <summary>
    /// Known Token types
    /// </summary>
    public enum TokenIdBase
    {
        /// <summary>
        /// End of text.
        /// </summary>
        _EOT_ = 0,

        /// <summary>
        /// Error occured, not recovered.
        /// </summary>
        _ERROR_ = -1,

        /// <summary>
        /// Comment in text
        /// </summary>
        _COMMENT_ = -2,

        /// <summary>
        /// End of Line token. (see also: property ScanNewlines)
        /// </summary>
        _EOL_ = -3,
    }
}
