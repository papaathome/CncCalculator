using System.ComponentModel;

namespace As.Applications.Validation
{
    /// <summary>
    /// IChanged : INotifyPropertyChanged
    /// </summary>
    public interface IChanged : INotifyPropertyChanged
    {
        /// <summary>
        /// Flag indicating that at least one change was notified since the last ResetChanged.
        /// </summary>
        bool IsChanged { get; }

        /// <summary>
        /// Reset the IsChanged flag.
        /// </summary>
        void ResetChanged();
    }
}
