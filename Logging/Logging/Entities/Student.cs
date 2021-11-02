using Logging.Attributes;

namespace Logging.Entities
{
    [TrackingEntity]
    public class Student
    {
        [TrackingProperty(_name = "Name")]
        public string Name { get; set; }
        [TrackingProperty(_name = "Student Age")]
        public int Age { get; set; }
        public string Faculty { get; set; }
        public string University { get; set; }
    }
}