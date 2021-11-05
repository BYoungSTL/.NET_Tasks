using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using ListenerInterfaces;
using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;
using static System.Diagnostics.EventLog;

namespace LogListener
{
    
    /// <summary>
    /// EventLog listener to logging into Windows Event Log
    /// For convenient work, a log file is maintained
    /// Has a default and custom options
    /// Logging level to Event Log level:
    ///     Error, Fatal - Error
    ///     Warn - Warn
    ///     Others - Info
    /// </summary>
    public class EventLogListener : IListener
    {
        private string _loggerNamePattern;
        private string _path;
        private readonly string _writeString = $"{DateTime.Now}, {Assembly.GetExecutingAssembly().Location} ";
        private const string SecondLayout = "${longdate} ${uppercase:${level}} ${message} ${newline}";
        private const string FirstLayout = "${longdate} ${callsite} ${uppercase:${level}} ${message} ${newline}";

        public ListenerType Type { get; set; }

        public Logger _Logger { get; set; }

        public void Listener(LogLevel logLevel, string message)
        {
            _Logger = LogManager.GetLogger(Type.ToString().ToLower());
            if (!SourceExists("log"))
            {
                CreateEventSource("log", "Application");
            }

            if (message == string.Empty) return;
            EventLog eventLog = new EventLog("Application")
            {
                Source = "log"
            };
            switch (logLevel.Name.ToLower())
            {
                case "error":
                case "fatal":
                    _Logger.Error(message);
                    eventLog.WriteEntry(_writeString + message, EventLogEntryType.Error);
                    break;
                case "warn":
                    _Logger.Warn(message);
                    eventLog.WriteEntry(_writeString + message, EventLogEntryType.Warning);
                    break;
                default:
                    _Logger.Info(message);
                    eventLog.WriteEntry(_writeString + message, EventLogEntryType.Information);
                    break;
            }
        }

        public void DefaultLoggerOptions()
        {
            _path = Directory.GetCurrentDirectory() + $"\\logs\\log\\{DateTime.Now:yy-MM-dd}.log";
            Type = ListenerType.EventLog;
            LoggingConfiguration configuration = new LoggingConfiguration();
            FileTarget logfile = new FileTarget
            {
                FileName = _path,
                Name = Type.ToString().ToLower()
            };
            configuration.LoggingRules.Add(new LoggingRule(Type.ToString().ToLower(), LogLevel.Trace, logfile));
            LogManager.Configuration = configuration;
        }

        public void CustomLoggerOptions()
        {
            Type = ListenerType.Txt;
            
            Console.WriteLine("Enter name of log file (enter date for yy-MM-dd format):");
            string fileName = Console.ReadLine() ?? string.Empty;
            if (fileName.ToLower() == "date")
            {
                _path = Directory.GetCurrentDirectory() + $"\\logs\\log\\{DateTime.Now:yy-MM-dd}.log";    
            }
            else
            {
                _path = Directory.GetCurrentDirectory() + $"\\logs\\log\\{fileName}.log";
            }

            //LoggerNamePattern it's a Target and Rule Name
            Console.WriteLine("Enter loggerNamePattern: ");
            _loggerNamePattern = Console.ReadLine() ?? string.Empty;
            
            LoggingConfiguration configuration = new LoggingConfiguration();
            FileTarget logfile = new FileTarget
            {
                FileName = _path,
                Name = _loggerNamePattern
            };
            configuration.LoggingRules.Add(new LoggingRule(_loggerNamePattern, LogLevel.Trace, logfile));
            LogManager.Configuration = configuration;
            
            Console.WriteLine("Enter Logging Level (debug, info, error, fatal, warn, trace)");
            Console.WriteLine("If input is invalid: Logging level = trace");
            string level = Console.ReadLine() ?? string.Empty;
            
            LogLevel logLevel;
            switch (level.ToLower())
            {
                case "debug":
                    logLevel = LogLevel.Debug;
                    break;
                case "info":
                case "information":
                    logLevel = LogLevel.Info;
                    break;
                case "error":
                    logLevel = LogLevel.Error;
                    break;
                case "fatal":
                    logLevel = LogLevel.Fatal;
                    break;
                case "warning":
                case "warn":
                    logLevel = LogLevel.Warn;
                    break;
                default:
                    logLevel = LogLevel.Trace;
                    break;
            }

            Console.WriteLine("Choose format layout");
            Console.WriteLine("1) Date, callsite, logging level, message");
            Console.WriteLine("2) Date, logging level, message");
            int numberFormat = int.Parse(Console.ReadLine() ?? string.Empty);
            switch (numberFormat)
            {
                case 2:
                    logfile.Layout = new SimpleLayout
                        {Text = SecondLayout};
                    break;
                default:
                    logfile.Layout = new SimpleLayout
                        {Text = FirstLayout};
                    break;
            }
            configuration.LoggingRules.Add(new LoggingRule(_loggerNamePattern, logLevel, logfile));
            LogManager.Configuration = configuration;
        }
    }
}