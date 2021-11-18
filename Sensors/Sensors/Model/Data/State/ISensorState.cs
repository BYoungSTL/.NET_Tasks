using Sensors.Model.Data.Factory;

namespace Sensors.Model.Data.State
{
    public interface ISensorState
    {
        void StateSimple(ISensor sensor);
        void StateCalibration(ISensor sensor);
        void StateWork(ISensor sensor);
    }
}