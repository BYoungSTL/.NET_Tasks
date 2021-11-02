using System;

namespace Logging.Attributes
{
    public class TrackingProperty : Attribute
    {
        public string _name { get; set; }
    }
}