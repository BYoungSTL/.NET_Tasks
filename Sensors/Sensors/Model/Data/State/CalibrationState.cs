using System.Threading;
using System.Threading.Tasks;
using Sensors.Model.Data.Factory;

namespace Sensors.Model.Data.State
{
    public class CalibrationState : ISensorState
    {
        public void StateSimple(ISensor sensor)
        {
            sensor.State = new SimpleState();
        }

        public void StateCalibration(ISensor sensor)
        {
            
        }

        public void StateWork(ISensor sensor)
        {
            sensor.State = new WorkState();
        }
    }
}