namespace As.Applications.Loggers
{
    /// <summary>
    /// Caliburn Micro Log4Net logger.
    /// </summary>
    internal class CmLog4NetLogger : ILogger
    {
        public static ILogger GetLogger(string name)
            => new CmLog4NetLogger(name);

        public static ILogger GetLogger(Type type)
            => new CmLog4NetLogger(type);

        public CmLog4NetLogger(string name)
        {
            Log = log4net.LogManager.GetLogger(name);
        }

        public CmLog4NetLogger(Type type)
        {
            Log = log4net.LogManager.GetLogger(type);
        }

        readonly log4net.ILog Log;

        #region Caliburn.Micro.ILog
        void Caliburn.Micro.ILog.Error(Exception exception)
            => Log.Error(exception.Message, exception);

        void Caliburn.Micro.ILog.Info(string format, params object[] args)
            => Log.InfoFormat(format, args);

        void Caliburn.Micro.ILog.Warn(string format, params object[] args)
            => Log.WarnFormat(format, args);
        #endregion Caliburn.Micro.ILog

        #region log4net.ILog
        public bool IsDebugEnabled => Log.IsDebugEnabled;

        public bool IsInfoEnabled => Log.IsInfoEnabled;

        public bool IsWarnEnabled => Log.IsWarnEnabled;

        public bool IsErrorEnabled => Log.IsErrorEnabled;

        public bool IsFatalEnabled => Log.IsFatalEnabled;

        public log4net.Core.ILogger Logger => Log.Logger;

        public void Debug(object? message)
            => Log.Debug(message);

        public void Debug(object? message, Exception? exception)
            => Log.Debug(message, exception);

        public void DebugFormat(string format, params object?[]? args)
            => Log.DebugFormat(format, args);

        public void DebugFormat(string format, object? arg0)
            => Log.DebugFormat(format, arg0);

        public void DebugFormat(string format, object? arg0, object? arg1)
            => Log.DebugFormat(format, arg0, arg1);

        public void DebugFormat(string format, object? arg0, object? arg1, object? arg2)
            => Log.DebugFormat(format, arg0, arg1, arg2);

        public void DebugFormat(IFormatProvider? provider, string format, params object?[]? args)
            => Log.DebugFormat(provider, format, args);

        public void Error(object? message)
            => Log.Error(message);

        public void Error(object? message, Exception? exception)
            => Log.Error(message, exception);

        public void ErrorFormat(string format, params object?[]? args)
            => Log.ErrorFormat(format, args);
        public void ErrorFormat(string format, object? arg0)
            => Log.ErrorFormat(format, arg0);

        public void ErrorFormat(string format, object? arg0, object? arg1)
            => Log.ErrorFormat(format, arg0, arg1);

        public void ErrorFormat(string format, object? arg0, object? arg1, object? arg2)
            => Log.ErrorFormat(format, arg0, arg1, arg2);

        public void ErrorFormat(IFormatProvider? provider, string format, params object?[]? args)
            => Log.ErrorFormat(provider, format, args);

        public void Fatal(object? message)
            => Log.Fatal(message);

        public void Fatal(object? message, Exception? exception)
            => Log.Fatal(message, exception);

        public void FatalFormat(string format, params object?[]? args)
            => Log.FatalFormat(format, args);

        public void FatalFormat(string format, object? arg0)
            => Log.FatalFormat(format, arg0);

        public void FatalFormat(string format, object? arg0, object? arg1)
            => Log.FatalFormat(format, arg0, arg1);

        public void FatalFormat(string format, object? arg0, object? arg1, object? arg2)
            => Log.FatalFormat(format, arg0, arg1, arg2);

        public void FatalFormat(IFormatProvider? provider, string format, params object?[]? args)
            => Log.FatalFormat(format, args);

        public void Info(object? message)
            => Log.Info(message);

        public void Info(object? message, Exception? exception)
            => Log.Info(message, exception);

        public void InfoFormat(string format, params object?[]? args)
            => Log.InfoFormat(format, args);

        public void InfoFormat(string format, object? arg0)
            => Log.InfoFormat(format, arg0);

        public void InfoFormat(string format, object? arg0, object? arg1)
            => Log.InfoFormat(format, arg0, arg1);

        public void InfoFormat(string format, object? arg0, object? arg1, object? arg2)
            => Log.InfoFormat(format, arg0, arg1, arg2);

        public void InfoFormat(IFormatProvider? provider, string format, params object?[]? args)
            => Log.InfoFormat(provider, format, args);

        public void Warn(object? message)
            => Log.Warn(message);

        public void Warn(object? message, Exception? exception)
            => Log.Warn(message, exception);

        public void WarnFormat(string format, params object?[]? args)
            => Log.WarnFormat(format, args);

        public void WarnFormat(string format, object? arg0)
            => Log.WarnFormat(format, arg0);

        public void WarnFormat(string format, object? arg0, object? arg1)
            => Log.WarnFormat(format, arg0, arg1);

        public void WarnFormat(string format, object? arg0, object? arg1, object? arg2)
            => Log.WarnFormat(format, arg0, arg1, arg2);

        public void WarnFormat(IFormatProvider? provider, string format, params object?[]? args)
            => Log.WarnFormat(provider, format, args);
        #endregion log4net.ILog
    }
}
