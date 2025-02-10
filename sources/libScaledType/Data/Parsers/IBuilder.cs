namespace As.Tools.Data.Parsers
{
    /// <summary>
    /// Optional Builder to use by the (base) parser
    /// </summary>
    /// <remarks>
    /// The actual parser implementation can extend on this interface.
    /// </remarks>
    public interface IBuilder
    {
        /// <summary>
        /// Base parser hook called when parsing is started.
        /// </summary>
        void BeginLoadData();

        /// <summary>
        /// Base parser hook called when parsing has ended.
        /// </summary>
        void EndLoadData();
    }
}
