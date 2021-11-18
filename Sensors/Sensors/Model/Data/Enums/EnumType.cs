using System.Runtime.Serialization;

namespace Sensors.Model.Data.Enums
{
    [DataContract]
    public enum EnumType
    {
        Temperature,
        Pressure,
        Moisture,
    }
}