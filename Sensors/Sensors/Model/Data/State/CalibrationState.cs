using System.Threading;
using System.Threading.Tasks;
using Sensors.Model.Data.Factory;

namespace Sensors.Model.Data.State
{
    public class CalibrationState : ISensorState
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
            sensor.Interval = 1000;
            sensor.MeasuredValue = 0;
            Task.Run(() =>
            {
                sensor.MeasuredValue += 1;
                Thread.Sleep(sensor.MeasuredValue);
            });
        }
    }
}