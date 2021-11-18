﻿using System;
using System.Text.Json.Serialization;
using Sensors.Model.Data.Enums;
using Sensors.Model.Data.State;

namespace Sensors.Model.Data.Factory.Sensors
{
    [Serializable]
    public class MoistureSensor : ISensor
    {
        [JsonIgnore] public ISensorState State { get; set; }
        public Guid Id { get; set; }
        public static EnumType Type = EnumType.Moisture;
        public string MeasuredName { get; set; }
        public int MeasuredValue { get; set; }
        public int Interval { get; set; }
        public EnumMode Mode { get; set; }

        public MoistureSensor()
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