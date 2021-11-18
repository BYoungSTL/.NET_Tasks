using System;
using System.Threading;
using System.Threading.Tasks;
using Sensors.Model.Data.Factory;

namespace Sensors.Model.Data.State
{
    public class WorkState : ISensorState
    {
        public void StateSimple(ISensor sensor)
        {
            sensor.State = new SimpleState();
        }

        public void StateCalibration(ISensor sensor)
        {
            sensor.State = new CalibrationState();
        }

        //current state
        public void StateWork(ISensor sensor)
        {
            
        }
    }
}