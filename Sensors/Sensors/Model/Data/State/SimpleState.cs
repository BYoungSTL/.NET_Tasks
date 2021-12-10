using Sensors.Model.Data.Enums;
using Sensors.Model.Data.Factory;

namespace Sensors.Model.Data.State
{
    public class SimpleState : ISensorState
    {
        private readonly ISensor _sensor;
        public SimpleState(ISensor sensor)
        {
            _sensor = sensor;
        }
        //current state
        public void StateSimple()
        {
            
        }

        public void StateCalibration()
        {
            _sensor.MeasuredValue = 0;
            _sensor.Interval = 1000;
            _sensor.Mode = EnumMode.Calibration;
            _sensor.State = new CalibrationState(_sensor);
        }

        public void StateWork()
        {
            _sensor.Mode = EnumMode.Work;
            _sensor.MeasuredValue = 0;
            _sensor.State = new WorkState(_sensor);
        }

        
    }
}