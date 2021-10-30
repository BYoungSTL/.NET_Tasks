#nullable enable
using System;
using NET01.Enums;

namespace NET01.Entities
{
    public abstract class TrainingMaterials : Entity, ICloneable
    {
        public MaterialType MaterialType { get; protected init; }
        public string? Description
        {
            get => _description;
            protected init
            {
                if (value != null && value.Length > DescriptionLength)
                {
                    throw new ArgumentException($"Description is too long! (Max Length = {DescriptionLength})");
                }

                _description = value;
            }
        }
        
        public override string? ToString()
        {
            return Description;
        }

        public virtual object Clone()
        {
            throw new Exception("Abstract class cannot be copied");
        }

        public override int GetHashCode()
        {
            return int.Parse(ID.ToString());
        }

        public override bool Equals(object? obj)
        {
            if (obj is Entity)
            {
                return ID == (obj as Entity)!.ID;
            }

            return false;
        }
    }
}