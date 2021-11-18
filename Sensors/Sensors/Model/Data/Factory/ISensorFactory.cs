using Sensors.Model.Data.Enums;

namespace Sensors.Model.Data.Factory
{
    public interface ISensorFactory
    {
        public ISensor Create(EnumType type);
    }
}
