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
        private string _path;
        private StringBuilder _writeString;
        public ListenerType Type { get; set; }

        public Logger CustomLogger { get; set; }

        public void LoggerSetting(LogLevel logLevel, string message)
        {
            if (!File.Exists(_path))
            {
                WordprocessingDocument create =
                    WordprocessingDocument.Create(_path, WordprocessingDocumentType.Document);
            }
            using WordprocessingDocument word =
                WordprocessingDocument.Open(_path, true);
            
            StringBuilder newString =
                _writeString.Insert(_writeString.Length, new StringBuilder($"{logLevel.Name}" + message));
            
            MainDocumentPart mainPart = word.MainDocumentPart;
            if (mainPart == null)
            {
                mainPart = word.AddMainDocumentPart();
            }
            mainPart.Document = new Document(
                new Body(
                    new Paragraph(
                        new Run(
                            new Text(newString.ToString()))),
                    new Paragraph()
                ));
        }

        public void DefaultLoggerOptions()
        {
            _path = Directory.GetCurrentDirectory() +
                    $"\\logs\\word\\{DateTime.Now:yy-MM-dd}.docx"; //{DateTime.Now:yy-MM-dd}
            _writeString =
                new StringBuilder($"{DateTime.Now}, {Assembly.GetExecutingAssembly().Location} ");
            Type = ListenerType.Txt;
        }
    }
}