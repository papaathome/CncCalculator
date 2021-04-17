namespace As.Tools.Data.Scanners
{
    /// <summary>
    /// Generic interface for a progress bar.
    /// </summary>
    public interface IProgressBar
    {
        /// <summary>
        /// Set the maximum value of the progress bar. (Minimum = 0, Minimum LT Maximum)
        /// </summary>
        /// <param name="max">Max value.</param>
        void SetProgressLoadMax(ulong max = 0);

        /// <summary>
        /// Change progress bar indirator.
        /// </summary>
        /// <param name="now">Current value on a scale of [0, Max]</param>
        void SetProgressLoad(ulong now);
    }
}
