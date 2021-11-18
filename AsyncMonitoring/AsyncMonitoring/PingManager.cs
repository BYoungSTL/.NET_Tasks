using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncMonitoring
{
    public static class PingManager
    {
        public static async Task Start(UriPing ping)
        {
            Console.WriteLine("Start");
            CancellationTokenSource source = UriPing.TokenSource;
            //_result = new TaskFactory().StartNew(ping.JsonDeserialize, source.Token).Result;
            Console.WriteLine("After des");
            ping.PropCount = ping.Properties.Count;
            List<Task> tasks = new List<Task>();
            if (ping.Properties != null)
            {
                foreach (var pingProperty in ping.Properties)
                {
                    tasks.Add(ping.Ping(pingProperty));
                }
            }

            foreach (var task in tasks)
            {
                await task;
            }
            await Program.FileWatcher(ping);
            Task.WaitAll(tasks.ToArray());
        }
    }
}