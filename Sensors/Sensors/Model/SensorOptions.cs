using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Sensors.Model.Data;
using Sensors.Model.Data.Enums;
using Sensors.Model.Data.Factory;
using Sensors.Model.Data.Factory.Sensors;
using Sensors.Model.Data.State;

namespace Sensors.Model
{
    public delegate void ValueCountHandler();
    public class SensorOptions : ISensorFactory
    {
        public static bool IsContinue;
        public static event ValueCountHandler Count;

        public static readonly string JsonPath = Directory.GetCurrentDirectory() + "\\sensorsSpecification.json";
        public static readonly string XmlPath = Directory.GetCurrentDirectory() + "\\sensorsSpecification.xml";
        public static async Task<List<ISensor>> JsonDeserializeAsync()
        {
            ExistingFile(JsonPath);
            if (new FileInfo(JsonPath).Length == 0)
            {
                return null;
            }
            List<SensorSerialize> sensorsSerialize = new List<SensorSerialize>();
            await Task.Run(() =>
            {
                sensorsSerialize = JsonConvert.DeserializeObject<List<SensorSerialize>>(File.ReadAllText(JsonPath),
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });
            });
            List<ISensor> sensors = new List<ISensor>();
            foreach (var sensor in sensorsSerialize)
            {
                ISensor sens = new SensorOptions().Create(sensor.Type);
                sens.Id = sensor.Id;
                sens.MeasuredValue = sensor.MeasuredValue;
                sens.Mode = sensor.Mode;
                sens.Interval = sensor.Interval;
                sens.MeasuredName = sensor.MeasuredName;
                sensors.Add(sens);
            }
            return sensors;
        }

        public static async Task<List<ISensor>> XmlDeserializeAsync()
        {
            ExistingFile(XmlPath);
            if (new FileInfo(XmlPath).Length == 0)
            {
                return null;
            }
            XmlSerializer serializer = new XmlSerializer(typeof(List<SensorSerialize>));
            await using FileStream fileStream = new FileStream(XmlPath, FileMode.OpenOrCreate);
            var xmlSensors = serializer.Deserialize(fileStream) as List<SensorSerialize>;
            List<ISensor> sensors = new List<ISensor>();
            if (xmlSensors != null)
                foreach (var xmlSensor in xmlSensors)
                {
                    ISensor sensor = new SensorOptions().Create(xmlSensor.Type);
                    sensor.Id = xmlSensor.Id;
                    sensor.Mode = xmlSensor.Mode;
                    sensor.Interval = xmlSensor.Interval;
                    sensor.MeasuredValue = xmlSensor.MeasuredValue;
                    sensor.MeasuredName = xmlSensor.MeasuredName;
                    sensors.Add(sensor);
                }
            else
            {
                return null;
            }
            return sensors;
        }

        public static async Task XmlSerializeAsync(ISensor sensor)
        {
            List<SensorSerialize> xmlSensors = new List<SensorSerialize>();
            Count?.Invoke();
            ExistingFile(XmlPath);
            sensor.Id = IdGenerator.Generate();
            List<ISensor> sensors = new List<ISensor>();
            if (new FileInfo(XmlPath).Length != 0)
            {
                sensors = await XmlDeserializeAsync();
            }
            sensors ??= new List<ISensor>();
            sensors.Add(sensor);
            foreach (var sens in sensors)
            {
                xmlSensors.Add(new SensorSerialize()
                {
                    Id = sens.Id,
                    Interval = sens.Interval,
                    MeasuredValue = sens.MeasuredValue,
                    MeasuredName = sens.MeasuredName,
                    Type = sens.Type
                });
            }
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<SensorSerialize>));
                await using FileStream fileStream = new FileStream(XmlPath, FileMode.OpenOrCreate);
                serializer.Serialize(fileStream, xmlSensors);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static async Task JsonSerializeAsync(ISensor sensor)
        {
            Count?.Invoke();
            ExistingFile(JsonPath);
            sensor.Id = IdGenerator.Generate();
            List<SensorSerialize> sensorsSerialize = new List<SensorSerialize>();
            List<ISensor> sensors = new List<ISensor>();
            if (new FileInfo(JsonPath).Length != 0)
            {
                sensors = await JsonDeserializeAsync();
            }
            sensors ??= new List<ISensor>();
            sensors.Add(sensor);
            foreach (var sens in sensors)
            {
                sensorsSerialize.Add(new SensorSerialize
                {
                    Id = sens.Id,
                    Interval = sens.Interval,
                    MeasuredValue = sens.MeasuredValue,
                    MeasuredName = sens.MeasuredName,
                    Type = sens.Type
                });
            }
            string str = null;
            await Task.Run(() =>
            {
                str = JsonConvert.SerializeObject(sensorsSerialize, new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.All
                });
            });
            
            try
            {
                await File.WriteAllTextAsync(JsonPath, str);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static void ExistingFile(string path)
        {
            if (!File.Exists(path))
            {
                File.Create(path).Close();
            }
        }

        public static async Task<bool> DeleteAsync(Guid id, bool isXml)
        {
            IsContinue = false;
            List<ISensor> sensors;
            if (isXml)
            {
                sensors = await XmlDeserializeAsync();
            }
            else
            {
                sensors = await JsonDeserializeAsync();
            }
            foreach (var sensor in sensors)
            {
                if (sensor.Id == id)
                {
                    sensors.Remove(sensor);
                    File.Delete(isXml ? XmlPath : JsonPath);
                    break;
                }
            }

            foreach (var sensor in sensors)
            {
                if (isXml)
                {
                    await XmlSerializeAsync(sensor);
                }
                else
                {
                    await JsonSerializeAsync(sensor);
                }
            }
            return true;
        }

        public static async Task<bool> ChangeAsync(Guid id, ISensor sensor, bool isXml)
        {
            IsContinue = false;
            List<ISensor> sensors;
            if (isXml)
            {
                sensors = await XmlDeserializeAsync();
            }
            else
            {
                sensors = await JsonDeserializeAsync();
            }
            foreach (var sens in sensors)
            {
                if (sens.Id == id)
                {
                    sens.MeasuredName = sensor.MeasuredName;
                    sens.MeasuredValue = sensor.MeasuredValue;
                    sens.Mode = sensor.Mode;
                    File.Delete(isXml ? XmlPath : JsonPath);
                }

            }

            foreach (var sens in sensors)
            {
                if (isXml)
                {
                    await XmlSerializeAsync(sens);
                }
                else
                {
                    await JsonSerializeAsync(sens);
                }
            }
            return true;
        }

        //IsContinue = false, maybe serialize calls StopCount
        public static async Task ValueCountingAsync(List<ISensor> sensors)
        {
            Thread.CurrentThread.Name = "Value Count";
            Count += StopCount;
            IsContinue = true;
            foreach (var sensor in sensors)
            {
                sensor.isCounting = true;
            }
            while (IsContinue)
            {
                foreach (var sensor in sensors)
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
                            break;
                    }
                }
            }

            File.Delete(SensorOptions.JsonPath);
            foreach (var sensor in sensors)
            {
                sensor.isCounting = false;
                await JsonSerializeAsync(sensor);
            }
            //await ChangeAsync(sensor.Id, sensor, false); // don't use ChangeAsync
        }

        public static async Task<ISensor> FindAsync(Guid id, bool isXml)
        {
            List<ISensor> sensors;
            if (isXml)
            {
                sensors = await XmlDeserializeAsync();
            }
            else
            {
                sensors = await JsonDeserializeAsync();
            }
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
                        isCounting = false
                    };
                case EnumType.Pressure:
                    return new PressureSensor
                    {
                        Id = IdGenerator.Generate(),
                        MeasuredName = "Pressure",
                        MeasuredValue = 0,
                        State = new SimpleState(),
                        Mode = EnumMode.Simple,
                        isCounting = false
                    };
                default:
                    return new TemperatureSensor
                    {
                        Id = IdGenerator.Generate(),
                        MeasuredName = "Temperature",
                        MeasuredValue = 0,
                        State = new SimpleState(),
                        Mode = EnumMode.Simple,
                        isCounting = false
                    };
            }
        }

        public static void StopCount()
        {
            IsContinue = false;
        }
    }
}