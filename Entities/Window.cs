using System;
using System.Xml.Serialization;

namespace NET02._2.Entities
{
    [Serializable]
    public class Window 
    {
        [XmlAttribute]
        public string Title { get; set; }
        [XmlElement("top")]
        public int? Top { get; set; }
        [XmlElement("width")]
        public int? Width { get; set; }
        [XmlElement("left")]
        public int? Left { get; set; }
        [XmlElement("height")]
        public int? Height { get; set; }

        /// <summary>
        /// If all integer properties > 0, this Window is Exist
        /// </summary>
        /// <returns></returns>
        public bool IsExistMainWindow()
        {
            return Top != null && Width != null && Left != null && Height != null;
        }
    }
}