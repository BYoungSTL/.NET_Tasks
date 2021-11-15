using NLog;

namespace ListenerInterfaces
{
    public interface IListener
    {
        public ListenerType Type { get; set; }
        public void LoggerSetting(LogLevel logLevel, string message);
        public void DefaultLoggerOptions();
        public Logger CustomLogger { get; set; }

    }
}
