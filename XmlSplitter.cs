using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace NET02._2
{
    public static class XmlSplitter
    {
        private static readonly string FileName = Directory.GetCurrentDirectory() + "\\Config\\UserSettings.xml";
        private static readonly string SplitXmlFilePath = Directory.GetCurrentDirectory() + "\\Config\\";

        /// <summary>
        /// Split users from main XML, separate user - separate file
        ///     Name of split XML file: username.xml
        ///         username from "Name" attribute of "Login" element
        /// Set default values for null elements: width = 400, height = 150, top and left = 0;
        /// </summary>
        public static void XmlSplitting()
        {
            XDocument doc = XDocument.Load(FileName);
            var newDocs = doc.Descendants("Login")
                .Select(d => new XDocument(d));
            foreach (var newDoc in newDocs)
            {
                var newElement = newDoc.Descendants("Window");
                foreach (var vElement in newElement)
                {
                    foreach (var element in Enum.GetNames(typeof(WindowElements)))
                    {
                        if (vElement.Element(element)!.IsEmpty)
                        {
                            vElement.Element(element)!.RemoveAll();
                            if (element == Enum.GetName(typeof(WindowElements), 2)) // width = 400;
                            {
                                vElement.Element(element)!.Value = "400";
                            }
                            if (element == Enum.GetName(typeof(WindowElements), 4)) // height = 150;
                            {
                                vElement.Element(element)!.Value = "150";
                            }
                            vElement.Element(element)!.Value = "0"; // top = 0, left = 0;
                        }
                    }
                }
                newDoc.Save(SplitXmlFilePath + newDoc.Element("Login")!.Attribute("Name")!.Value + ".xml");
            }
        }
    }
}