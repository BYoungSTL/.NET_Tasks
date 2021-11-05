using System;
using System.IO;
using System.Reflection;
using System.Text;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using ListenerInterfaces;
using NLog;

namespace WordListener
{
    
    /// <summary>
    /// Docx Listener to logging into .docx file
    /// Has Default and Custom options to config Listener
    /// For correct work of Listener, needs to duplicate logging into txt file
    /// </summary>
    public class DocxListener : IListener
    {
        private readonly string _tempFile = Directory.GetCurrentDirectory() + $"\\logs\\word\\{DateTime.Now:yy-MM-dd}.txt";
        private string _path;
        private StringBuilder _writeString;
        public ListenerType Type { get; set; }

        public Logger _Logger { get; set; }

        public void Listener(LogLevel logLevel, string message)
        {
            string docText = null;
            if (File.Exists(_tempFile))
            {
                docText = File.ReadAllText(_tempFile);
            }

            if (docText == "")
            {
                docText = null;
            }

            using WordprocessingDocument word =
                WordprocessingDocument.Create(_path, WordprocessingDocumentType.Document);

            
            MainDocumentPart mainPart = word.AddMainDocumentPart();
            mainPart.Document = new Document();
            
            Body body = mainPart.Document.AppendChild(new Body());

            Paragraph paragraph = body.AppendChild(new Paragraph());

            Run run = paragraph.AppendChild(new Run());

            StringBuilder newString =
                _writeString.Insert(_writeString.Length, new StringBuilder($"{logLevel.Name}" + message));

            if (docText != null)
            {
                run.AppendChild(new Text(docText));
                run.AppendChild(new Paragraph());
            }
            

            run.AppendChild(new Text(newString.ToString()));
            run.AppendChild(new Paragraph());
            
            //Creating txt file for addition docx file
            using FileStream fileStream = new FileStream(_tempFile, FileMode.OpenOrCreate);
            byte[] array = Encoding.Default.GetBytes(newString.ToString());
            fileStream.Write(array, 0 , array.Length);
        }

        public void DefaultLoggerOptions()
        {
            _path = Directory.GetCurrentDirectory() +
                    $"\\logs\\word\\{DateTime.Now:yy-MM-dd}.docx"; //{DateTime.Now:yy-MM-dd}
            _writeString =
                new StringBuilder($"{DateTime.Now}, {Assembly.GetExecutingAssembly().Location} ");
            Type = ListenerType.Txt;
        }

        public void CustomLoggerOptions()
        {
            Type = ListenerType.Word;

            Console.WriteLine("Enter name of log file (enter date for yy-MM-dd format):");
            string fileName = Console.ReadLine() ?? string.Empty;
            if (fileName.ToLower() == "date")
            {
                _path = Directory.GetCurrentDirectory() + $"\\logs\\word\\{DateTime.Now:yy-MM-dd}.docx";
            }
            else
            {
                _path = Directory.GetCurrentDirectory() + $"\\logs\\txt\\{fileName}.docx";
            }

            Console.WriteLine("Choose format of log:");
            Console.WriteLine("1) Date, Location, Logging Level, message");
            Console.WriteLine("2) Date, Logging Level, message");

            int numberFormat = int.Parse(Console.ReadLine() ?? string.Empty);
            switch (numberFormat)
            {
                case 2:
                    _writeString = new StringBuilder($"{DateTime.Now}, {Assembly.GetExecutingAssembly().Location} ");
                    break;
                default:
                    _writeString = new StringBuilder($"{DateTime.Now} ");
                    break;
            }
        }
    }
}