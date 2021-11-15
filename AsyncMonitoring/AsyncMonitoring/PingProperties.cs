using System;
using System.Collections.Generic;

namespace AsyncMonitoring
{

    /// <summary>
    /// Properties to customization Ping method
    /// </summary>
    [Serializable]
    public class PingProperties
    {
        public string UriRef { get; set; }
        public string MailTo { get; set; }
        public int Delay { get; set; }
        public int MaxWaitingTime { get; set; }
        
    }
}