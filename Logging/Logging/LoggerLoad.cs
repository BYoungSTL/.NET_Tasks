using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace Logging
{
    /// <summary>
    /// load 3 different assemblies from different dll
    /// </summary>
    public static class LoggerLoad
    {
        private static readonly IConfigurationBuilder ConfigurationBuilder =
            new ConfigurationBuilder().AddJsonFile(Directory.GetCurrentDirectory() + "\\appsettings.json");
        private static readonly IConfiguration Configuration = ConfigurationBuilder.Build();
        private static readonly string LogName = Configuration["AssemblyLogName"];
        private static readonly string TxtName = Configuration["AssemblyTxtName"];
        private static readonly string WordName = Configuration["AssemblyWordName"];
        private const string Txt = "TxtListener.TextListener";
        private const string Log = "LogListener.EventLogListener";
        private const string Word = "WordListener.DocxListener";

        public static Type LoadTxtListener()
        {
            Type typeTextListener;
            try
            {
                Assembly assemblyTxt = Assembly.Load(new AssemblyName(TxtName));
                typeTextListener = assemblyTxt.GetType(Txt, true, true);
            }
            catch
            {
                throw new FileLoadException("Invalid Txt Assembly load");
            }

            return typeTextListener;
        }   

        public static Type LoadLogListener()
        {
            Type typeLogListener;
            try
            {
                Assembly assemblyLog = Assembly.Load(new AssemblyName(LogName));
                typeLogListener = assemblyLog.GetType(Log, true, true);
            }
            catch
            {
                throw new FileLoadException("Invalid Log Assembly load");
            }
            return typeLogListener;
        }
        
        public static Type LoadWordListener()
        {
            Type typeWordListener;
            try
            {
                Assembly assemblyWord = Assembly.Load(new AssemblyName(WordName));
                typeWordListener = assemblyWord.GetType(Word, true, true);
            }
            catch
            {
                throw new FileLoadException("Invalid Word Assembly load");
            }
            
            return typeWordListener;
        }
    }
}