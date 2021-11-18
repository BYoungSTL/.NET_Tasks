using Sensors.Model.Data.Factory;

namespace Sensors.Model.Data.State
{
    public class SimpleState : ISensorState
    {
        public void StateSimple(ISensor sensor)
        {
            
        }

        public void StateCalibration(ISensor sensor)
        {
            sensor.State = new CalibrationState();
        }

        public void StateWork(ISensor sensor)
        {
            sensor.State = new WorkState();
        }
    }
}