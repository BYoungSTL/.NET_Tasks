using System;
using System.Threading;
using System.Threading.Tasks;
using Sensors.Model.Data.Factory;

namespace Sensors.Model.Data.State
{
    public class WorkState : ISensorState
    {
        public void StateSimple()
        {
            
        }

        public void StateCalibration()
        {
            
        }

        public void StateWork()
        {
            
        }

        public void Counting(Sensor sensor)
        {
            Task.Run(() =>
            {
                sensor.MeasuredValue = new Random().Next(101);
                Thread.Sleep(sensor.Interval);
            });
        }
    }
}