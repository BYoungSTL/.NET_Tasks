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
        
        public bool IsLoginCorrect()
        {
            const string correctString = "main";
            if (Windows.Count == 1 && Windows[0].Title.ToLower() == correctString &&
                Windows[0].IsExistMainWindow())
            {
                return true;
            }

            if (Windows.TrueForAll(w =>
                !String.Equals(w.Title, correctString, StringComparison.CurrentCultureIgnoreCase)))
            {
                return true;
            }

            return false;
        }
    }
}