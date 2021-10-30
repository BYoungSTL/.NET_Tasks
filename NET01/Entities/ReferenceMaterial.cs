using System;
using NET01.Enums;

namespace NET01.Entities
{
    public class ReferenceMaterial : TrainingMaterials
    {
        private readonly RefTypes _referenceType;
        public string UriContent { get; }
        public RefTypes ReferenceType
        {
            get => _referenceType;
            protected init
            {
                bool isInitRefType = false;
                foreach (var type in Enum.GetValues(typeof(RefTypes)))
                {
                    if (value == (RefTypes) type)
                    {
                        _referenceType = value;
                        isInitRefType = true;
                        break;
                    }
                }

                if (!isInitRefType)
                {
                    throw new ArgumentException("Invalid Reference Type");
                }   
            }
            
        }

        public ReferenceMaterial(string uriContent, RefTypes referenceType, string description)
        {
            ID = this.InitGuid();
            UriContent = uriContent;
            ReferenceType = referenceType;
            Description = description;
            MaterialType = MaterialType.Reference;
        }

        public override ReferenceMaterial Clone()
        {
            return new ReferenceMaterial(UriContent, ReferenceType, Description)
            {
                ID = this.ID
            };
        }
    }
}