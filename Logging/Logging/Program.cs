using System;
using System.IO;
using System.Reflection;
using Logging.Entities;
using LogListener;
using Microsoft.Extensions.Configuration;

namespace Logging
{
    static class Program
    {
        static void Main(string[] args)
        {
            //Entities for test
            Student student = new Student()
            {
                Name = "A", Age = 18, Faculty = "B", University = "C"
            };
            Employee employee = new Employee()
            {
                Name = "AA", Age = 19
            };
            Tracking<Student>.Track(student);
        }
    }
}
