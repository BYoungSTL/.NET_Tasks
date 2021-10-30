using System;

namespace NET01.Entities
{
    public abstract class Entity
    {
        public Guid ID { get; set; }
        protected const int DescriptionLength = 256;
        protected string? _description;
    }
}