using System.Runtime.Serialization;

namespace Sensors.Model.Data.Enums
{
    [DataContract]
    public enum EnumMode
    {
        [EnumMember(Value = "simple")]
        Simple,
        [EnumMember(Value = "calibration")]
        Calibration,
        [EnumMember(Value = "work")]
        Work
    }
}