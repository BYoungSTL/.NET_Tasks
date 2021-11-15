using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using ListenerInterfaces;
using NLog;
using NLog.Config;
using NLog.Targets;
using static System.Diagnostics.EventLog;
using Microsoft.Win32;

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
        private string _path;
        private readonly string _writeString = $"{DateTime.Now}, {Assembly.GetExecutingAssembly().Location} ";

        public ListenerType Type { get; set; }

        public Logger CustomLogger { get; set; }

        public void LoggerSetting(LogLevel logLevel, string message)
        {
            CustomLogger = LogManager.GetLogger(Type.ToString().ToLower());
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
                    CustomLogger.Error(message);
                    eventLog.WriteEntry(_writeString + message, EventLogEntryType.Error);
                    break;
                case "warn":
                    CustomLogger.Warn(message);
                    eventLog.WriteEntry(_writeString + message, EventLogEntryType.Warning);
                    break;
                default:
                    CustomLogger.Info(message);
                    eventLog.WriteEntry(_writeString + message, EventLogEntryType.Information);
                    break;
            }
        }

        public void DefaultLoggerOptions()
        {
            CreateRegistry();
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

        private void CreateRegistry()
        {
            RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SYSTEM", false)
                ?.OpenSubKey("CurrentControlSet", false)?.OpenSubKey("Services", true)
                ?.OpenSubKey("Application", true);
            try
            {
                registryKey?.OpenSubKey("EventLog");
            }
            catch 
            {
                if (registryKey != null)
                {
                    registryKey.CreateSubKey("EventLog")?.CreateSubKey("log");
                    registryKey.SetValue("log", RegistryValueKind.None);
                }
                return;
            }
            if (registryKey?.OpenSubKey("EventLog") != null)
            {
                registryKey.OpenSubKey("EventLog", true)?.DeleteSubKey("log");
            }

            if (registryKey != null)
            {
                registryKey.CreateSubKey("EventLog")?.CreateSubKey("log");
                registryKey.SetValue("log", RegistryValueKind.None);
            }
        }
    }
}