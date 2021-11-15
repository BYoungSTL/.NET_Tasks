using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NLog;

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

        private static Task FileWatcher(UriPing ping)
        {
             
            using var watcher = new FileSystemWatcher(Directory.GetCurrentDirectory());

            var tcs = new TaskCompletionSource<bool>();
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
            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;
            return tcs.Task;
        }
        
        static async Task Main(string[] args)
        {
            //Only one instance can be run, others instance shutdown at once
            if(System.Diagnostics.Process.GetProcessesByName(System.Diagnostics.Process.GetCurrentProcess().ProcessName).Length > 1)
                return;
            UriPing ping = new UriPing();
            await ping.JsonDeserialize();
            List<Task> tasks = new List<Task>();
            if (ping.Properties != null)
            {
                tasks.AddRange(ping.Properties.Select(pingProperty => Task.Run(() => ping.Ping(pingProperty))));
            }

            await Task.WhenAll(tasks.ToArray());
            await FileWatcher(ping);
            //await ping.JsonSerialize();
        }
    }
}