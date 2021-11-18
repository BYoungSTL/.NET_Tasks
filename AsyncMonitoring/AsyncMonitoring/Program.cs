using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using static AsyncMonitoring.PingManager;

namespace AsyncMonitoring
{
    static class Program
    {
        //File Watcher Logger and methods
        private static readonly Logger Logger = LogManager.GetLogger("change");

        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }

            Logger.Info("Options change");
        }

        private static void OnDeleted(object sender, FileSystemEventArgs e) =>
            Logger.Info($"Deleted: {e.FullPath}");

        private static void OnRenamed(object sender, RenamedEventArgs e)
        {
            Logger.Info($"File Rename     Old: {e.OldFullPath}     New: {e.FullPath}");
        }

        private static void OnError(object sender, ErrorEventArgs e)
        {
            PrintException(e.GetException());
        }

        private static void PrintException(Exception ex)
        {
            if (ex == null) return;
            Logger.Info($"Message: {ex.Message} Stacktrace: {ex.StackTrace}");
            PrintException(ex.InnerException);
        }

        public static async Task FileWatcher(UriPing ping)
        {
            using var watcher = new FileSystemWatcher(Directory.GetCurrentDirectory());

            watcher.NotifyFilter = NotifyFilters.Attributes
                                   | NotifyFilters.CreationTime
                                   | NotifyFilters.DirectoryName
                                   | NotifyFilters.FileName
                                   | NotifyFilters.LastAccess
                                   | NotifyFilters.LastWrite
                                   | NotifyFilters.Security
                                   | NotifyFilters.Size;

            watcher.Changed += OnChanged;
            watcher.Changed += ping.JsonDeserialize;
            watcher.Deleted += OnDeleted;
            watcher.Renamed += OnRenamed;
            watcher.Error += OnError;

            watcher.Filter = "pingSettings.json";
            watcher.IncludeSubdirectories = false;
            watcher.EnableRaisingEvents = true;
            watcher.SynchronizingObject = null;
            watcher.InternalBufferSize = 65536;

            watcher.BeginInit();
        }
        

        static void Main(string[] args)
        {
            //Only one instance can be run, others instance shutdown at once
            if (System.Diagnostics.Process
                .GetProcessesByName(System.Diagnostics.Process.GetCurrentProcess().ProcessName).Length > 1)
                return;
            Console.WriteLine("Main");
            
            UriPing ping = new UriPing();
            ping.JsonDeserializeSync();
            
            Task.Run(() => FileWatcher(ping));    
            Task.Run(() => Start(ping)).Wait();
        }
    }
}