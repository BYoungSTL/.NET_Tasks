using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sensors.Sensors
{
    public class SensorOptions
    {
        public async Task<Sensor> JsonDeserialize()
        {
            var options = new JsonSerializerOptions
            {
                IncludeFields = true
            };
            await using FileStream fs = File.OpenRead(Path.GetFullPath("sensorsSpecification.json"));
            return await JsonSerializer.DeserializeAsync<Sensor>(fs, options);
        }

        public async Task JsonSerialize(Sensor sensor)
        {
            var options = new JsonSerializerOptions
            {
                IncludeFields = true
            };
            await using FileStream fs = File.OpenRead(Path.GetFullPath("sensorsSpecification.json"));
            await JsonSerializer.SerializeAsync(fs, sensor, options);
        }

        public static Guid InitGuid()
        {
            return Guid.NewGuid();
        }

    }
}