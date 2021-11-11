﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Sensors.Model.Data;
using Sensors.Model.Data.Factory;

namespace Sensors.Model
{
    public class SensorOptions : ISensorFactory
    {
        private static readonly string Path = Directory.GetCurrentDirectory() + "\\sensorsSpecification.json";
        private const string DefaultValueString = "Unknown";
        private const int DefaultValueInt = 10;

        public static async Task<List<Sensor>> JsonDeserialize()
        {
            var options = new JsonSerializerOptions
            {
                IncludeFields = true
            };
            await using FileStream fs = new FileStream(Path,
                FileMode.OpenOrCreate);
            return await JsonSerializer.DeserializeAsync<List<Sensor>>(fs, options);
        }

        public static async Task<bool> JsonSerialize(Sensor sensor)
        {
            if (sensor.Interval == 0)
            {
                sensor.Interval = DefaultValueInt;
            }

            if (sensor.Type == "")
            {
                sensor.Type = DefaultValueString;
            }
            if (sensor.MeasuredName == "")
            {
                sensor.MeasuredName = DefaultValueString;
            }
            if (sensor.MeasuredValue == "")
            {
                sensor.MeasuredValue = DefaultValueString;
            }
            sensor.Id = IdGenerator.Generate();

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };

            List<Sensor> sensors = new List<Sensor>();
            await using (FileStream fs = new FileStream(Path,
                FileMode.OpenOrCreate))
            {
                if (new FileInfo(Path).Length != 0)
                {
                    sensors = await JsonSerializer.DeserializeAsync<List<Sensor>>(fs, options);
                }
            }

            File.Delete(Path);

            sensors ??= new List<Sensor>();
            sensors.Add(sensor);

            await using FileStream newfs = new FileStream(Path, FileMode.OpenOrCreate);
            await JsonSerializer.SerializeAsync(newfs, sensors, options);
            
            return true;
        }


        public static async Task<bool> JsonDelete(Guid id)
        {
            List<Sensor> sensors = await JsonDeserialize();
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

        public static async Task<bool> JsonChange(Guid id, Sensor sensor)
        {
            List<Sensor> sensors = await JsonDeserialize();
            foreach (var sens in sensors)
            {
                if (sens.Id == id)
                {
                    sens.Type = sensor.Type;
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

        // public static async Task<Sensor> JsonFind(Guid id)
        // {
        //     
        // }

        public ISensor Create()
        {
            return new Sensor()
            {
                Id = IdGenerator.Generate(),
                Type = "Default Type",
                MeasuredName = "Default Measured Name",
                MeasuredValue = "Default Measured Value",

            };
        }
    }
}