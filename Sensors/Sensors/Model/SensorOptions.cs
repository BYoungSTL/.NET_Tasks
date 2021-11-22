using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media.Animation;
using Newtonsoft.Json;
using Sensors.Model.Data;
using Sensors.Model.Data.Enums;
using Sensors.Model.Data.Factory;
using Sensors.Model.Data.Factory.Sensors;
using Sensors.Model.Data.State;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Sensors.Model
{
    public class SensorOptions : ISensorFactory
    {
        private static bool _isContinue;
        public delegate Task ValueCountHandler(ISensor sensor);

        public static event ValueCountHandler Count;

        private static readonly string Path = Directory.GetCurrentDirectory() + "\\sensorsSpecification.json";

        public static async Task<List<ISensor>> JsonDeserialize()
        {
            var options = new JsonSerializerOptions
            {
                IncludeFields = true
            };

            await using FileStream fs = new FileStream(Path,
                FileMode.OpenOrCreate);
            try
            {
                return await JsonSerializer.DeserializeAsync<List<ISensor>>(fs, options);
            }
            catch (NotSupportedException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static List<ISensor> NewstonDeserialize(List<ISensor> sensors)
        {
            NewstonSerialize(sensors);
            List<ISensor> test = new List<ISensor>();
            try
            {
                test = JsonConvert.DeserializeObject<List<ISensor>>(File.ReadAllText(Path), new JsonSerializerSettings
                {
                    StringEscapeHandling = StringEscapeHandling.EscapeHtml,
                    TypeNameHandling = TypeNameHandling.Auto
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return test; // it's null (why not)
        }

        public static void NewstonSerialize(List<ISensor> sensors)
        {
            sensors.Add(new TemperatureSensor()
            {
                Id = IdGenerator.Generate(),
                Interval = 0,
                MeasuredValue = 0,
                MeasuredName = "govno",
                Type = EnumType.Temperature,
                Mode = EnumMode.Simple
            });
            var str = JsonConvert.SerializeObject(sensors, new JsonSerializerSettings()
            {
                StringEscapeHandling = StringEscapeHandling.EscapeHtml,
                TypeNameHandling = TypeNameHandling.All
            });
            File.WriteAllText(Path, str);
        }

        public static async Task<bool> JsonSerialize(ISensor sensor)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };

            List<ISensor> sensors = new List<ISensor>();
            
            if (new FileInfo(Path).Length != 0)
            {
                sensors = NewstonDeserialize(sensors);
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

                await OnCount(sens);
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
            _isContinue = true;
            Count += (sensor1) =>
            {
                if (sensor1.Mode != EnumMode.Simple)
                {
                    _isContinue = false;
                }
                return null;
            };
            if (sensor.isCounting)
            {
                return;
            }
            while (_isContinue)
            {
                sensor.isCounting = true;
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
                        _isContinue = false;
                        break;
                }
            }

            sensor.isCounting = false;
            await JsonDelete(sensor.Id);
            await JsonSerialize(sensor);
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
                        Mode = EnumMode.Simple,
                        isCounting = true
                    };
                case EnumType.Pressure:
                    return new PressureSensor
                    {
                        Id = IdGenerator.Generate(),
                        MeasuredName = "Pressure",
                        MeasuredValue = 0,
                        State = new SimpleState(),
                        Mode = EnumMode.Simple,
                        isCounting = true
                    };
                default:
                    return new TemperatureSensor
                    {
                        Id = IdGenerator.Generate(),
                        MeasuredName = "Temperature",
                        MeasuredValue = 0,
                        State = new SimpleState(),
                        Mode = EnumMode.Simple,
                        isCounting = true
                    };
            }
        }

        private static Task OnCount(ISensor sensor)
        {
            Count?.Invoke(sensor);
            return null;
        }
    }
}