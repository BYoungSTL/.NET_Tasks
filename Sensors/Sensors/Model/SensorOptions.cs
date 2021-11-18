using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Sensors.Model.Data;
using Sensors.Model.Data.Enums;
using Sensors.Model.Data.Factory;
using Sensors.Model.Data.Factory.Sensors;
using Sensors.Model.Data.State;

namespace Sensors.Model
{
    public class SensorOptions : ISensorFactory
    {
        private static readonly string Path = Directory.GetCurrentDirectory() + "\\sensorsSpecification.json";

        public static async Task<List<ISensor>> JsonDeserialize()
        {
            var options = new JsonSerializerOptions
            {
                IncludeFields = true
            };
            await using FileStream fs = new FileStream(Path,
                FileMode.OpenOrCreate);
            return await JsonSerializer.DeserializeAsync<List<ISensor>>(fs, options);
        }

        public static async Task<bool> JsonSerialize(ISensor sensor)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };

            List<ISensor> sensors = new List<ISensor>();
            await using (FileStream fs = new FileStream(Path,
                FileMode.OpenOrCreate))
            {
                if (new FileInfo(Path).Length != 0)
                {
                    sensors = await JsonSerializer.DeserializeAsync<List<ISensor>>(fs, options);
                }
            }

            File.Delete(Path);

            sensors ??= new List<ISensor>();
            sensors.Add(sensor);

            await using FileStream newfs = new FileStream(Path, FileMode.OpenOrCreate);
            await JsonSerializer.SerializeAsync(newfs, sensors, options);
            
            return true;
        }


        public static async Task<bool> JsonDelete(Guid id)
        {
            List<ISensor> sensors = await JsonDeserialize();
            foreach (var sensor in sensors)
            {
                if (sensor.Id == id)
                {
                    sensors.Remove(sensor);
                    File.Delete(Path);
                    break;
                }
            }

            foreach (var sensor in sensors)
            {
                await JsonSerialize(sensor);
            }
            return true;
        }

        public static async Task<bool> JsonChange(Guid id, TemperatureSensor sensor)
        {
            List<ISensor> sensors = await JsonDeserialize();
            foreach (var sens in sensors)
            {
                if (sens.Id == id)
                {
                    sens.MeasuredName = sensor.MeasuredName;
                    sens.MeasuredValue = sensor.MeasuredValue;
                    sens.Mode = sensor.Mode;
                    File.Delete(Path);
                }
            }

            foreach (var sens in sensors)
            {
                await JsonSerialize(sens);
            }
            return true;
        }


        //???????????????
        public static async Task ValueCounting(ISensor sensor)
        {
            bool isContinue = true;
            while (isContinue)
            {
                switch (sensor.Mode)
                {
                    case EnumMode.Calibration:
                        sensor.MeasuredValue += 1;
                        await Task.Delay(sensor.Interval = 1000);
                        break;
                    case EnumMode.Work:
                        sensor.MeasuredValue = new Random().Next(151);
                        await Task.Delay(sensor.Interval);
                        break;
                    case EnumMode.Simple:
                        sensor.MeasuredValue = 0;
                        sensor.Interval = 0;
                        isContinue = false;
                        break;
                }
            }
        }

        public static async Task<ISensor> JsonFind(Guid id)
        {
            List<ISensor> sensors = await JsonDeserialize();
            foreach (var sensor in sensors)
            {
                if (sensor.Id == id)
                {
                    return sensor;
                }
            }

            return null;
        }

        public ISensor Create(EnumType type)
        {
            switch (type)
            {
                case EnumType.Moisture:
                    return new MoistureSensor
                    {
                        Id = IdGenerator.Generate(),
                        MeasuredName = "Moisture",
                        MeasuredValue = 0,
                        State = new SimpleState(),
                        Mode = EnumMode.Simple
                    };
                case EnumType.Pressure:
                    return new PressureSensor
                    {
                        Id = IdGenerator.Generate(),
                        MeasuredName = "Pressure",
                        MeasuredValue = 0,
                        State = new SimpleState(),
                        Mode = EnumMode.Simple
                    };
                default:
                    return new TemperatureSensor
                    {
                        Id = IdGenerator.Generate(),
                        MeasuredName = "Temperature",
                        MeasuredValue = 0,
                        State = new SimpleState(),
                        Mode = EnumMode.Simple
                    };
            }
        }
    }
}