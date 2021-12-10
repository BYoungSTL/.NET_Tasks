﻿using System;
using System.Text.Json.Serialization;
using Sensors.Model.Data.Enums;
using Sensors.Model.Data.State;

namespace Sensors.Model.Data.Factory.Sensors
{
    public class PressureSensor : ISensor
    {
        public ISensorState State { get; set; }
        public EnumType Type { get; set; }
        public Guid Id { get; set; }
        public string MeasuredName { get; set; }
        public int MeasuredValue { get; set; }
        public int Interval { get; set; }
        public EnumMode Mode { get; set; }
        public bool IsCounting { get; set; }

        public PressureSensor()
        {
            IsCounting = false;
            Type = EnumType.Pressure;
            switch (Mode)
            {
                case EnumMode.Simple:
                    State = new SimpleState(this);
                    break;
                case EnumMode.Calibration:
                    State = new CalibrationState(this);
                    break;
                case EnumMode.Work:
                    State = new WorkState(this);
                    break;
            }
        }
    }
}