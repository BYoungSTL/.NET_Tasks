using System;
using System.IO;
using ListenerInterfaces;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace TxtListener
{
    /// <summary>
    /// Txt Listener to logging into .txt file
    /// Has Default and Custom options to config Listener
    /// </summary>
    public class TextListener : IListener
    {
        private string _path;
        private string _loggerNamePattern;

        public ListenerType Type { get; set; }
        public Logger CustomLogger { get; set; }

        public void LoggerSetting(LogLevel logLevel, string message)
        {
            CustomLogger = LogManager.GetLogger(_loggerNamePattern);
            Type = ListenerType.Txt;
            switch (logLevel.Name.ToLower().ToLower())
            {
                case "debug":
                    CustomLogger.Debug(message);
                    break;
                case "error":
                    CustomLogger.Error(message);
                    break;
                case "info":
                    CustomLogger.Info(message);
                    break;
                case "warning":
                    CustomLogger.Warn(message);
                    break;
                case "fatal":
                    CustomLogger.Fatal(message);
                    break;
                default:
                    CustomLogger.Trace(message);
                    break;
            }
        }

        public void DefaultLoggerOptions()
        {
            _loggerNamePattern = Type.ToString().ToLower();
            _path = Directory.GetCurrentDirectory() + $"\\logs\\txt\\{DateTime.Now:yy-MM-dd}.txt";
            Type = ListenerType.Txt;
            LoggingConfiguration configuration = new LoggingConfiguration();
            FileTarget logfile = new FileTarget
            {
                FileName = _path,
                Name = _loggerNamePattern
            };
            configuration.LoggingRules.Add(new LoggingRule(_loggerNamePattern, LogLevel.Trace, logfile));
            LogManager.Configuration = configuration;
        }
    }
}
