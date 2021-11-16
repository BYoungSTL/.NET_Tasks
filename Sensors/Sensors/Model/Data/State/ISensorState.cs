using Sensors.Model.Data.Factory;

namespace Sensors.Model.Data.State
{
    public interface ISensorState
    {
        void StateSimple();
        void StateCalibration();
        void StateWork();

        void Counting(Sensor sensor);
    }
}