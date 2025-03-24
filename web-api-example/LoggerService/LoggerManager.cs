using Contracts;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace LoggerService;

public class LoggerManager : ILoggerManager
{
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

    public LoggerManager()
    {
        var logDirectory = Path.Combine(Directory.GetCurrentDirectory(), "logs");
        Directory.CreateDirectory(logDirectory);
        // Настройка NLog
        var logConfig = new LoggingConfiguration();

        // Файловый таргет
        var fileTarget = new FileTarget("logfile")
        {
            FileName = Path.Combine(logDirectory, "${shortdate}_logfile.txt"),
            Layout = "${longdate} | ${level:uppercase=true} | ${message} ${exception:format=ToString}",
            ArchiveEvery = FileArchivePeriod.Day,
            MaxArchiveFiles = 7,
            ConcurrentWrites = true
        };
        
        var consoleTarget = new ConsoleTarget("logconsole")
        {
            Layout = "${longdate} | ${level:uppercase=true} | ${message}"
        };
        
        logConfig.AddRule(LogLevel.Debug, LogLevel.Fatal, fileTarget);
        logConfig.AddRule(LogLevel.Info, LogLevel.Fatal, consoleTarget);

        LogManager.Configuration = logConfig;
        LogManager.KeepVariablesOnReload = true;
        LogManager.ReconfigExistingLoggers();
    }
    
    public void LogDebug(string message) => Logger.Debug(message);
    
    public void LogError(string message) => Logger.Error(message);
    
    public void LogInfo(string message) => Logger.Info(message);
    
    public void LogWarn(string message) => Logger.Warn(message);
}