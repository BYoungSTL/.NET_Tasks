using System;
using System.Runtime.Serialization;

namespace Sensors.Model.Data.Factory
{
    [Serializable, DataContract]
    public class Sensor : ISensor
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string MeasuredName { get; set; }
        public string MeasuredValue{ get; set; }
        public int Interval { get; set; } 
        public EnumMode Mode { get; set; }

    }
}