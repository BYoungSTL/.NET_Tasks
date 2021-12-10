using System;
using Sensors.Model.Data.Enums;
using Sensors.Model.Data.State;

namespace Sensors.Model.Data.Factory
{
    public interface ISensor
    {
        public ISensorState State { get; set; }
        public EnumType Type { get; set; }
        public Guid Id { get; set; }
        public string MeasuredName { get; set; }
        public int MeasuredValue { get; set; }
        public int Interval { get; set; }
        public EnumMode Mode { get; set; }
        public bool IsCounting { get; set; }
    }
}