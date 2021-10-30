#nullable enable
using System;
using NET01.Enums;
using NET01.Interfaces;

namespace NET01.Entities
{
    public class TrainingLesson : Entity, IVersionable, ICloneable
    {
        private byte[] _version = new byte[IVersionable.VersionSize];
        public TrainingMaterials[] MaterialsArray { get; private set; }
        public string Name { get; private set; }
        public MaterialType MaterialType { get; private set; }

        private string? Description
        {
            get => _description;
            set
            {
                if (value.Length > DescriptionLength)
                {
                    throw new ArgumentException($"Description is too long! (Max Length = {DescriptionLength})");
                }

                _description = value;
            }
        }

        //  Type of Lesson: if one of materials is Video, then the Lesson is Video Lesson
        private MaterialType MaterialsTypes()
        {
            foreach (var material in MaterialsArray)
            {
                if (material.MaterialType == MaterialType.Video)
                {
                    return MaterialType.Video;
                }
            }

            return MaterialType.Text;
        } 

        // IVersionable realization
        public byte[] GetVersion()
        {
            return _version;
        }

        public void SetVersion(byte[] version)
        {
            if (version.Length == _version.Length)
            {
                Array.Copy(_version, version, IVersionable.VersionSize);
            }
            else
            {
                throw new ArgumentException("Invalid version(length mismatch)");
            }
        }

        // ICloneable realization
        public object Clone()
        {
            byte[] newVersion = new byte[IVersionable.VersionSize];
            Array.Copy(_version, newVersion, IVersionable.VersionSize);

            TrainingMaterials[] newMaterialsArray = new TrainingMaterials[MaterialsArray.Length];
            for (int i = 0; i < MaterialsArray.Length; i++)
            {
                newMaterialsArray[i] = (MaterialsArray[i].Clone() as TrainingMaterials)!;
            }
            TrainingLesson newLesson = new()
            {
                Name = this.Name,
                _version = newVersion,
                Description = this.Description,
                MaterialsArray = newMaterialsArray,
                MaterialType = MaterialsTypes(),
                ID = this.ID
            };
            return newLesson;
        }

        public void InitLesson()
        {
            Console.WriteLine("Enter lesson name: ");
            Name = Console.ReadLine() ?? string.Empty;
            Console.WriteLine("Enter description: ");
            Description = Console.ReadLine();
            MaterialsArray = new TrainingMaterials[]
            {
                new VideoMaterial
                (
                    "Some picture",
                    "Some video",
                    VideoFormatTypes.AVI,
                    "Some description"
                ),
                new TextMaterial
                (
                    "Some text",
                    "Some description"
                ),
                new ReferenceMaterial
                (
                    "Some Content",
                    RefTypes.HTML,
                    null
                )
            };
            ID = this.InitGuid();
            MaterialType = MaterialsTypes();
            SetVersion(new byte[]{1, 0, 0, 0, 0, 0, 0, 1});
            foreach (var trainingMaterials in MaterialsArray)
            {
                if (trainingMaterials is VideoMaterial material)
                {
                    material.SetVersion(new byte[]{1, 0, 0, 0, 0, 0, 0, 1});
                }
            }
        }

        public override string? ToString()
        {
            return Description;
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