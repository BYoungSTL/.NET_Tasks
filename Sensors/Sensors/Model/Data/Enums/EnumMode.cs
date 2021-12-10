using System.Runtime.Serialization;

namespace Sensors.Model.Data.Enums
{
    [DataContract]
    public enum EnumMode
    {
        Simple,
        Calibration,
        Work
    }
}