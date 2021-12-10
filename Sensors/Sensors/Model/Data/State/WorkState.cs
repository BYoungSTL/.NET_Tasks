using Sensors.Model.Data.Enums;
using Sensors.Model.Data.Factory;

namespace Sensors.Model.Data.State
{
    public class WorkState : ISensorState
    {
        private readonly ISensor _sensor;
        public WorkState(ISensor sensor)
        {
            _sensor = sensor;
        }
        public void StateSimple()
        {
            _sensor.MeasuredValue = 0;
            _sensor.Mode = EnumMode.Simple;
            _sensor.State = new SimpleState(_sensor);
        }

        public void StateCalibration()
        {
            _sensor.MeasuredValue = 0;
            _sensor.Interval = 0;
            _sensor.Mode = EnumMode.Calibration;
            _sensor.State = new CalibrationState(_sensor);
        }

        //current state
        public void StateWork()
        {
            _sensor.MeasuredValue = 0;
        }
    }
}