using As.Applications.Appenders;

using log4net;

namespace As.Applications.Loggers
{
    /// <summary>
    /// User Interface (UI) output as Log4Net logger and appender.
    /// </summary>
    /// <remarks>
    /// All UI output must pass Log4Net processing to allow configuration control over the actual output.
    /// So all UI.<see cref="ILog"/> -> Log4Net logger -> Log4Net appender -> UI.OnUiEventHandler.
    /// Any client hooked up to OnUiEventHandler will get the rendered loggingEvent and the raw loggingEvent.
    /// See also: <seealso cref="As.Applications.Appenders.FireEventAppender"/>
    /// </remarks>
    public static class UI
    {
        /// <summary>
        /// Root name of the log4net UI loggers subtree
        /// </summary>
        public const string LOGGER_SCREEN_ROOT = nameof(UI);

        /// <summary>
        /// Root logger of the log4net UI subtree, used for all UI.ILog actions.
        /// </summary>
        static readonly ILog Log = LogManager.GetLogger(LOGGER_SCREEN_ROOT);

        /// <summary>
        /// static .ctor, set client to log4net UI appender for loggingEvents.
        /// </summary>
        static UI()
        {
            OnUiEventHandler += OnMessageLoggedEvent;
        }

        /// <summary>
        /// Fires on receiving a UI (child) appender loggingEvent
        /// </summary>
        static public event EventHandler<MessageLoggedEventArgs>? OnUiEventHandler
        {
            add
            {
                if (FireEventAppender.Instance is not null)
                {
                    FireEventAppender.Instance.MessageLoggedEvent += value;
                }
            }
            remove
            {
                if (FireEventAppender.Instance is not null)
                {
                    FireEventAppender.Instance.MessageLoggedEvent -= value;
                }
            }
        }

        /// <summary>
        /// Check for Log4Net UI configured and available.
        /// </summary>
        static public bool IsUiEnabled => FireEventAppender.Instance is not null;

        /// <summary>
        /// Option to write all loggingEvents to the <see cref="System.Diagnostics.Trace"/> window.
        /// </summary>
        static public bool IsDiagnosticsEnabled { get; set; }
#if DEBUG
        = true;
#else
        = false;
#endif

        /// <summary>
        /// Client for Diagnostics to UI appender.
        /// </summary>
        /// <param name="sender">Originator of the event</param>
        /// <param name="e">Arguments of the event</param>
        static void OnMessageLoggedEvent(object? sender, MessageLoggedEventArgs e)
        {
            if (IsDiagnosticsEnabled)
            {
                System.Diagnostics.Trace.WriteLine(e.RenderedLoggingEvent);
            }
        }

        /// <summary>
        /// Get a logger in the UI subtree, usualy in 'UI.FullName' where FullName represents the type.
        /// </summary>
        /// <param name="type">Instance used for a logger</param>
        /// <returns><see cref="ILog"/> instance in the UI subtree</returns>
        static public ILog GetLogger(Type type)
            => GetLogger(type.FullName);

        /// <summary>
        /// Get a logger in the UI subtree, usualy in 'UI.name'.
        /// </summary>
        /// <param name="name">Name used for a logger</param>
        /// <returns>ILog instance in the UI subtree</returns>
        static public ILog GetLogger(string? name)
        {
            if (name == null) return null!;

            var l = LOGGER_SCREEN_ROOT.Length;
            if (
                (name.Length < l) ||
                (string.Compare(name, 0, LOGGER_SCREEN_ROOT, 0, l) != 0)
               )
            {
                name = LOGGER_SCREEN_ROOT + "." + name;
            }
            return LogManager.GetLogger(name);
        }

        /// <summary>
        /// <see cref="ILog"/> member
        /// </summary>
        static public bool IsDebugEnabled => Log.IsDebugEnabled;

        /// <summary>
        /// <see cref="ILog"/> member
        /// </summary>
        static public bool IsInfoEnabled => Log.IsInfoEnabled;

        /// <summary>
        /// <see cref="ILog"/> member
        /// </summary>
        static public bool IsWarnEnabled => Log.IsWarnEnabled;

        /// <summary>
        /// <see cref="ILog"/> member
        /// </summary>
        static public bool IsErrorEnabled => Log.IsErrorEnabled;

        /// <summary>
        /// <see cref="ILog"/> member
        /// </summary>
        static public bool IsFatalEnabled => Log.IsFatalEnabled;

        /// <summary>
        /// <see cref="ILog"/> member
        /// </summary>
        static public void Debug(object message) => Log.Debug(message);

        /// <summary>
        /// <see cref="ILog"/> member
        /// </summary>
        static public void Debug(object message, Exception exception)
            => Log.Debug(message, exception);

        /// <summary>
        /// <see cref="ILog"/> member
        /// </summary>
        static public void DebugFormat(string format, params object[] args)
            => Log.DebugFormat(format, args);

        /// <summary>
        /// <see cref="ILog"/> member
        /// </summary>
        static public void DebugFormat(string format, object arg0)
            => Log.DebugFormat(format, arg0);

        /// <summary>
        /// <see cref="ILog"/> member
        /// </summary>
        static public void DebugFormat(string format, object arg0, object arg1)
            => Log.DebugFormat(format, arg0, arg1);

        /// <summary>
        /// <see cref="ILog"/> member
        /// </summary>
        static public void DebugFormat(
            string format,
            object arg0,
            object arg1,
            object arg2)
            => Log.DebugFormat(format, arg0,arg1,arg2);

        /// <summary>
        /// <see cref="ILog"/> member
        /// </summary>
        static public void DebugFormat(
            IFormatProvider provider,
            string format,
            params object[] args)
            => Log.DebugFormat(provider, format, args);

        /// <summary>
        /// <see cref="ILog"/> member
        /// </summary>
        static public void Info(object message) => Log.Info(message);

        /// <summary>
        /// <see cref="ILog"/> member
        /// </summary>
        static public void Info(
            object message,
            Exception exception)
            => Log.Info(message, exception);

        /// <summary>
        /// <see cref="ILog"/> member
        /// </summary>
        static public void InfoFormat(
            string format,
            params object[] args)
            => Log.InfoFormat(format, args);

        /// <summary>
        /// <see cref="ILog"/> member
        /// </summary>
        static public void InfoFormat(
            string format,
            object arg0)
            => Log.InfoFormat(format, arg0);

        /// <summary>
        /// <see cref="ILog"/> member
        /// </summary>
        static public void InfoFormat(
            string format,
            object arg0,
            object arg1)
            => Log.InfoFormat(format, arg0, arg1);

        /// <summary>
        /// <see cref="ILog"/> member
        /// </summary>
        static public void InfoFormat(
            string format,
            object arg0,
            object arg1,
            object arg2)
            => Log.InfoFormat(format, arg0, arg1, arg2);

        /// <summary>
        /// <see cref="ILog"/> member
        /// </summary>
        static public void InfoFormat(
            IFormatProvider provider,
            string format,
            params object[] args)
            => Log.InfoFormat(provider, format, args);

        /// <summary>
        /// <see cref="ILog"/> member
        /// </summary>
        static public void Warn(object message) => Log.Warn(message);

        /// <summary>
        /// <see cref="ILog"/> member
        /// </summary>
        static public void Warn(
            object message,
            Exception exception)
            => Log.Warn(message, exception);

        /// <summary>
        /// <see cref="ILog"/> member
        /// </summary>
        static public void WarnFormat(
            string format,
            params object[] args)
            => Log.WarnFormat(format, args);

        /// <summary>
        /// <see cref="ILog"/> member
        /// </summary>
        static public void WarnFormat(
            string format,
            object arg0)
            => Log.WarnFormat(format, arg0);

        /// <summary>
        /// <see cref="ILog"/> member
        /// </summary>
        static public void WarnFormat(
            string format,
            object arg0,
            object arg1)
            => Log.InfoFormat(format, arg0, arg1);

        /// <summary>
        /// <see cref="ILog"/> member
        /// </summary>
        static public void WarnFormat(
            string format,
            object arg0,
            object arg1,
            object arg2)
            => Log.WarnFormat(format, arg0, arg1, arg2);

        /// <summary>
        /// <see cref="ILog"/> member
        /// </summary>
        static public void WarnFormat(
            IFormatProvider provider,
            string format,
            params object[] args)
            => Log.WarnFormat(provider, format, args);

        /// <summary>
        /// <see cref="ILog"/> member
        /// </summary>
        static public void Error(object message) => Log.Error(message);

        /// <summary>
        /// <see cref="ILog"/> member
        /// </summary>
        static public void Error(
            object message,
            Exception exception)
            => Log.Error(message, exception);

        /// <summary>
        /// <see cref="ILog"/> member
        /// </summary>
        static public void ErrorFormat(
            string format,
            params object[] args)
            => Log.ErrorFormat(format, args);

        /// <summary>
        /// <see cref="ILog"/> member
        /// </summary>
        static public void ErrorFormat(
            string format,
            object arg0)
            => Log.WarnFormat(format, arg0);

        /// <summary>
        /// <see cref="ILog"/> member
        /// </summary>
        static public void ErrorFormat(
            string format,
            object arg0,
            object arg1)
            => Log.InfoFormat(format, arg0, arg1);

        /// <summary>
        /// <see cref="ILog"/> member
        /// </summary>
        static public void ErrorFormat(
            string format,
            object arg0,
            object arg1,
            object arg2)
            => Log.ErrorFormat(format, arg0, arg1, arg2);

        /// <summary>
        /// <see cref="ILog"/> member
        /// </summary>
        static public void ErrorFormat(
            IFormatProvider provider,
            string format,
            params object[] args)
            => Log.ErrorFormat(provider, format, args);

        /// <summary>
        /// <see cref="ILog"/> member
        /// </summary>
        static public void Fatal(object message) => Log.Fatal(message);

        /// <summary>
        /// <see cref="ILog"/> member
        /// </summary>
        static public void Fatal(
            object message,
            Exception exception)
            => Log.Fatal(message, exception);

        /// <summary>
        /// <see cref="ILog"/> member
        /// </summary>
        static public void FatalFormat(
            string format,
            params object[] args)
            => Log.FatalFormat(format, args);

        /// <summary>
        /// <see cref="ILog"/> member
        /// </summary>
        static public void FatalFormat(
            string format,
            object arg0)
            => Log.FatalFormat(format, arg0);

        /// <summary>
        /// <see cref="ILog"/> member
        /// </summary>
        static public void FatalFormat(
            string format,
            object arg0,
            object arg1)
            => Log.FatalFormat(format, arg0, arg1);

        /// <summary>
        /// <see cref="ILog"/> member
        /// </summary>
        static public void FatalFormat(
            string format,
            object arg0,
            object arg1,
            object arg2)
            => Log.FatalFormat(format, arg0, arg1, arg2);

        /// <summary>
        /// <see cref="ILog"/> member
        /// </summary>
        static public void FatalFormat(
            IFormatProvider provider,
            string format,
            params object[] args)
            => Log.FatalFormat(provider, format, args);
    }
}
