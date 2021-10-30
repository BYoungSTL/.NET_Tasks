using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace NET02._2.Entities
{
    [Serializable, XmlRoot("config")]
    public class Login
    {
        [XmlAttribute]
        public string Name { get; set; }
        public List<Window> Windows { get; set; }
    }
}