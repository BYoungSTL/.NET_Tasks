using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Sensors.Sensors
{
    [Serializable]
    public class Sensor
    {
        public Guid Id { get; set; }
        [JsonInclude] public string Type { get; set; }
        [JsonInclude] public string MeasuredName { get; set; }
        [JsonInclude] public string MeasuredValue{ get; set; }

        [JsonInclude] public int Interval { get; set; }
        [JsonInclude] public EnumMode Mode { get; set; }

        public Sensor()
        {
            Id = SensorOptions.InitGuid();
        }
    }
}