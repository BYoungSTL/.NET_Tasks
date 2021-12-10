using System.Threading;
using System.Threading.Tasks;
using Sensors.Model.Data.Enums;
using Sensors.Model.Data.Factory;

namespace Sensors.Model.Data.State
{
    public class CalibrationState : ISensorState
    {
        private readonly ISensor _sensor;
        public CalibrationState(ISensor sensor)
        {
            _sensor = sensor;
        }
        public void StateSimple()
        {
            _sensor.MeasuredValue = 0;
            _sensor.Mode = EnumMode.Simple;
            _sensor.State = new SimpleState(_sensor);
        }

        //current state
        public void StateCalibration()
        {
            
        }

        public void StateWork()
        {
            _sensor.Mode = EnumMode.Work;
            _sensor.MeasuredValue = 0;
            _sensor.State = new WorkState(_sensor);
        }

    }
}