namespace As.Applications.Loggers
{
    public interface ILogger :
        Caliburn.Micro.ILog,
        log4net.ILog
    { }
}
