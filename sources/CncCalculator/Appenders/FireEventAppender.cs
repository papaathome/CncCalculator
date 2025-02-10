using log4net.Core;

namespace As.Applications.Appenders
{
    // source: https://github.com/apache/logging-log4net

    /// <summary>
    /// Appender that raises an event for each LoggingEvent received
    /// </summary>
    /// <remarks>
    /// Raises a MessageLoggedEvent for each LoggingEvent object received
    /// by this appender.
    /// </remarks>
    public class FireEventAppender : log4net.Appender.AppenderSkeleton
    {
        /// <inheritdoc/>
        public FireEventAppender() : base()
        {
            Instance = this;
        }

        /// <summary>
        /// Event handler to dispatch a loggingEvent to clients.
        /// </summary>
        public event EventHandler<MessageLoggedEventArgs>? MessageLoggedEvent;

        /// <summary>
        /// Easy singleton, gets the last instance created.
        /// </summary>
        public static FireEventAppender? Instance { get; private set; }

        /// <inheritdoc/>
        public virtual FixFlags Fix { get; set; } = FixFlags.All;

        /// <inheritdoc/>
        protected override void Append(LoggingEvent loggingEvent)
        {
            // silently ignore loggingEvent==null
            //ArgumentNullException.ThrowIfNull(loggingEvent);
            if (Instance == null) return;

            // Because the LoggingEvent may be used beyond the lifetime 
            // of the Append() method we must fix any volatile data in the event
            loggingEvent.Fix = Fix;

            // Render the loggingEvent and raise the  MessageLoggerdEvent event
            MessageLoggedEvent?.Invoke(this, new MessageLoggedEventArgs(loggingEvent, RenderLoggingEvent(loggingEvent)));
        }
    }

    /// <inheritdoc/>
    public sealed class MessageLoggedEventArgs(
        LoggingEvent logging_event,
        string rendered_logging_event) :
        EventArgs
    {
        /// <inheritdoc/>
        public LoggingEvent LoggingEvent { get; } = logging_event;

        /// <summary>
        /// Rendered loggingEvent value according to the Log4Net appender layout configuration.
        /// </summary>
        public string RenderedLoggingEvent { get; } = rendered_logging_event;
    }
}
