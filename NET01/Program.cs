using System;
using System.Linq;
using NET01.Entities;

namespace NET01
{
    static class Program
    {

        static void Main(string[] args)
        {
            TrainingLesson lesson = new TrainingLesson();
            lesson.InitLesson();
            TrainingLesson newLesson = (TrainingLesson) lesson.Clone();
            Console.WriteLine(lesson.Equals(newLesson));
            Console.WriteLine(lesson.Name + " " + newLesson.Name);
            Console.WriteLine(lesson.ID + " " + newLesson.ID);
            Console.WriteLine(lesson.MaterialType + " " + newLesson.MaterialType);
            Console.WriteLine(lesson.ToString() + " " + newLesson.ToString()); //ToString() returns Description
            foreach (var material in lesson.MaterialsArray.Concat(newLesson.MaterialsArray))
            {
                Console.WriteLine(material.MaterialType + " " + material.Description);
            }
            foreach (var v in lesson.GetVersion())
            {
                Console.Write(v + " ");
            }
            Console.WriteLine();
            foreach (var v in newLesson.GetVersion())
            {
                Console.Write(v + " ");
            }
        }
    }
}
