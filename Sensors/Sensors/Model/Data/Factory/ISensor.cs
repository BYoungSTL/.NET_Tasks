using System;

namespace Sensors.Model.Data.Factory
{
    public interface ISensor
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string MeasuredName { get; set; }
        public int MeasuredValue { get; set; }

        public int Interval { get; set; }
        public EnumMode Mode { get; set; }
    }
}