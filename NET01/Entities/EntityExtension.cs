using System;

namespace NET01.Entities
{
    public static class EntityExtension
    {
        public static Guid InitGuid(this Entity entity)
        {
            entity.ID = Guid.NewGuid();
            return entity.ID;
        }
    }
}