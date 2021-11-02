using NLog;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace ListenerInterfaces
{
    public interface IListener
    {
        public ListenerType Type { get; set; }
        public void Listener(NLog.LogLevel logLevel, string message);
        public void DefaultLoggerOptions();
        public void CustomLoggerOptions();
        public Logger _Logger { get; set; }

    }
}
