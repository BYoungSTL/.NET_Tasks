using Sensors.Model.Data.Factory;

namespace Sensors.Model.Data.State
{
    public class SimpleState : ISensorState
    {
        public void StateSimple()
        {
            interval = 0;
            measuredValue = 0;
        }

        public void StateCalibration()
        {
            
        }

        public void StateWork()
        {
            
        }

        public void Counting(Sensor sensor)
        {
            sensor.MeasuredValue = 0;
            sensor.Interval = 0;
        }
    }
}