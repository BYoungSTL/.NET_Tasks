using Logging.Entities;
using LogListener;

namespace Logging
{
    static class Program
    {
        static void Main(string[] args)
        {
            
            //LoggerLoad.LoadTxtListener();
            //LoggerLoad.LoadLogListener();
            //LoggerLoad.LoadWordListener();
            //TextListener textListener = new TextListener();
            //EventLogListener eventLogListener = new EventLogListener();
            //DocxListener docxListener = new DocxListener();
            //textListener.LoggerOptions(NLog.LogLevel.Trace);
            //textListener.Listener(NLog.LogLevel.Trace,"Student created in txt");
            //eventLogListener.Listener(NLog.LogLevel.Trace,"Student created in EventLog");
            //docxListener.Listener(NLog.LogLevel.Warn,"Student created in word");

            //Entities for test
            Student student = new Student()
            {
                Name = "A", Age = 18, Faculty = "B", University = "C"
            };
            Employee employee = new Employee()
            {
                Name = "AA", Age = 19
            };
            //Tracking<Student>.Track(student);
        }
    }
}
