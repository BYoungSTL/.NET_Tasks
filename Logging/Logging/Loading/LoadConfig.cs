using System.IO;
using Microsoft.Extensions.Configuration;

namespace Logging.Loading
{
    public static class LoadConfig
    {
        private static readonly IConfigurationBuilder ConfigurationBuilder =
            new ConfigurationBuilder().AddJsonFile(Directory.GetCurrentDirectory() + "\\appsettings.json");
        private static readonly IConfiguration Configuration = ConfigurationBuilder.Build();
        public static readonly string LogName = Configuration["AssemblyLogName"];
        public static readonly string TxtName = Configuration["AssemblyTxtName"];
        public static readonly string WordName = Configuration["AssemblyWordName"];
        public const string Txt = "TxtListener.TextListener";
        public const string Log = "LogListener.EventLogListener";
        public const string Word = "WordListener.DocxListener";
    }

    public enum Methods
    {
        LoggerSetting,
        DefaultLoggerOptions,
        CustomLoggerOptions
    }
}