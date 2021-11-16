using System;
using System.Runtime.Serialization;
using Sensors.Model.Data.State;

namespace Sensors.Model.Data.Factory
{
    [Serializable, DataContract]
    public class Sensor : ISensor
    {
        public ISensorState State { get; set; }
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string MeasuredName { get; set; }
        public int MeasuredValue{ get; set; }
        public int Interval { get; set; } 
        public EnumMode Mode { get; set; }

        public Sensor()
        {
            switch (Mode)
            {
                case EnumMode.Simple:
                    State = new SimpleState();
                    break;
                case EnumMode.Calibration:
                    State = new CalibrationState();
                    break;
                case EnumMode.Work:
                    State = new WorkState();
                    break;
            }
        }
    }
}