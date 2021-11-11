using System;

namespace Sensors.Model.Data
{
    public class IdGenerator
    {
        private static readonly IdGenerator Instance = new IdGenerator();

        public static IdGenerator GetInstance()
        {
            return Instance;
        }

        public static Guid Generate()
        {
            return Guid.NewGuid();
        }

    }
}