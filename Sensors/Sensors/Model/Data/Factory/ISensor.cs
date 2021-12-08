using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Sensors.Model.Data.Enums;
using Sensors.Model.Data.State;

namespace Sensors.Model.Data.Factory
{
    public interface ISensor
    {
        [JsonIgnore]public ISensorState State { get; set; }
        [DataMember]public EnumType Type { get; set; }
        [DataMember] public Guid Id { get; set; }
        [DataMember] public string MeasuredName { get; set; }
        [DataMember] public int MeasuredValue { get; set; }
        [DataMember] public int Interval { get; set; }
        [DataMember] public EnumMode Mode { get; set; }
        [JsonIgnore]public bool isCounting { get; set; }
    }
}