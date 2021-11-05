using System;
using System.IO;
using ListenerInterfaces;
using NLog;
using NLog.Config;
using NLog.Layouts;
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
        private const string SecondLayout = "${longdate} ${uppercase:${level}} ${message} ${newline}";
        private const string FirstLayout = "${longdate} ${callsite} ${uppercase:${level}} ${message} ${newline}";

        public ListenerType Type { get; set; }
        public Logger _Logger { get; set; }

        public void Listener(LogLevel logLevel, string message)
        {
            _Logger = LogManager.GetLogger(_loggerNamePattern);
            Type = ListenerType.Txt;
            switch (logLevel.Name.ToLower().ToLower())
            {
                case "debug":
                    _Logger.Debug(message);
                    break;
                case "error":
                    _Logger.Error(message);
                    break;
                case "info":
                    _Logger.Info(message);
                    break;
                case "warning":
                    _Logger.Warn(message);
                    break;
                case "fatal":
                    _Logger.Fatal(message);
                    break;
                default:
                    _Logger.Trace(message);
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

        public void CustomLoggerOptions()
        {
            Type = ListenerType.Txt;
            
            Console.WriteLine("Enter name of log file (enter date for yy-MM-dd format):");
            string fileName = Console.ReadLine() ?? string.Empty;
            if (fileName.ToLower() == "date")
            {
                _path = Directory.GetCurrentDirectory() + $"\\logs\\txt\\{DateTime.Now:yy-MM-dd}.txt";    
            }
            else
            {
                _path = Directory.GetCurrentDirectory() + $"\\logs\\txt\\{fileName}.txt";    
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
            NLog.LogLevel logLevel;
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
