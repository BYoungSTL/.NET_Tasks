using System;
using System.Diagnostics;
using NET01.Enums;
using NET01.Interfaces;

namespace NET01.Entities
{
    public class VideoMaterial : TrainingMaterials, IVersionable
    {
        private VideoFormatTypes _videoFormat;
        private byte[] _version = new byte[IVersionable.VersionSize];
        public string UriVideo { get; }
        public string UriPicture { get; }
        public VideoFormatTypes VideoFormat
        {
            get => _videoFormat;
            protected init
            {
                 bool isInitFormat = false;
                 foreach (var type in Enum.GetValues(typeof(VideoFormatTypes)))
                 {
                     if (value == (VideoFormatTypes) type)
                     {
                         _videoFormat = value;
                         isInitFormat = true;
                         break;
                     }
                 }

                 if (!isInitFormat)
                 {
                     throw new ArgumentException("Invalid Video Format");
                 }
            }
        }

        public VideoMaterial(string uriVideo, string uriPicture, VideoFormatTypes videoFormat, string description)
        {
            ID = this.InitGuid();
            UriVideo = uriVideo;
            UriPicture = uriPicture;
            VideoFormat = videoFormat;
            Description = description;
            MaterialType = MaterialType.Video;
        }
   
        public byte[] GetVersion()
        {
            return _version;
        }

        public void SetVersion(byte[] version)
        {
            if (version.Length == _version.Length)
            {
                Array.Copy(version, _version, version.Length);
            }
            else
            {
                throw new ArgumentException("Invalid version(length mismatch)");
            }
        }

        public override VideoMaterial Clone()
        {
            VideoMaterial newVideoMaterial = new VideoMaterial(UriVideo, UriPicture, VideoFormat, Description)
            {
                ID = this.ID
            };
            newVideoMaterial.SetVersion(GetVersion());
            return newVideoMaterial;
        }
    }
}