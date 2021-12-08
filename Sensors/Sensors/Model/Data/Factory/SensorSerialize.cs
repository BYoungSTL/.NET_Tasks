#nullable enable
using System;
using System.Xml.Serialization;
using Sensors.Model.Data.Enums;

namespace Sensors.Model.Data.Factory
{
    [XmlRoot("Sensor"), Serializable, XmlInclude(typeof(SensorSerialize))]
    public class SensorSerialize
    {
        [XmlAttribute] public EnumType Type { get; set; }
        [XmlAttribute] public Guid Id { get; set; }
        [XmlAttribute] public string MeasuredName { get; set; }
        [XmlAttribute] public int MeasuredValue { get; set; }
        [XmlAttribute] public int Interval { get; set; }
        [XmlAttribute] public EnumMode Mode { get; set; }
    }
}