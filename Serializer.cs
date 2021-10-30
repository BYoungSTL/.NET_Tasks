using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Xml.Linq;
using System.Xml.Serialization;
using NET02._2.Entities;

namespace NET02._2
{
    public static class Serializer
    {
        private static readonly string XmlFileName = Directory.GetCurrentDirectory() + "\\Config\\UserSettings.xml";

        private static readonly string JsonFilePath = Directory.GetCurrentDirectory() + "\\Config\\";
        private static readonly string SplitXmlFilePath = Directory.GetCurrentDirectory() + "\\Config\\";

        static Serializer()
        {
            Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Config");
        }
        public static void XmlSerialize(List<Login> logins)
        {
            XmlSerializer formatterLogin = new XmlSerializer(typeof(List<Login>));
            using FileStream fileStream = new FileStream(XmlFileName, FileMode.OpenOrCreate);
            formatterLogin.Serialize(fileStream, logins);
            Console.WriteLine("Serialize");
        }

        public static List<Login> XmlDeserialize()
        {
            XmlSerializer formatterLogin = new XmlSerializer(typeof(List<Login>));
            using FileStream fileStream = new FileStream(XmlFileName, FileMode.OpenOrCreate);
            List<Login> logins = formatterLogin.Deserialize(fileStream) as List<Login>;
            Console.WriteLine("Deserialize");
            return logins;
        }

        /// <summary>
        /// True:
        ///     - The login configuration contains the settings for exactly one window titled main,
        ///       and for this window all four nested elements (top, left, width, height) are present
        ///     - The login configuration does not have the settings for window titled main
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public static bool IsLoginCorrect(Login login)
        {
            const string correctString = "main";
            if (login.Windows.Count == 1 && login.Windows[0].Title.ToLower() == correctString &&
                login.Windows[0].IsExistMainWindow())
            {
                return true;
            }

            if (login.Windows.TrueForAll(w =>
                !String.Equals(w.Title, correctString, StringComparison.CurrentCultureIgnoreCase)))
            {
                return true;
            }

            return false;
        }
        
        public static Login JsonDeserialize(string fileName)
        {
            var jsonString = File.ReadAllText(JsonFilePath + "\\" + fileName + ".json");
            var login = JsonSerializer.Deserialize<Login>(jsonString);
            return login;
        }

        public static void JsonSerialize(Login login)
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
                IgnoreNullValues = true
            };
            
            string json = JsonSerializer.Serialize(login, options);
            File.WriteAllText(JsonFilePath + "//" + login.Name + ".json", json);
        }

        /// <summary>
        /// Split users from main XML, separate user - separate file
        ///     Name of split XML file: username.xml
        ///         username from "Name" attribute of "Login" element
        /// Set default values for null elements: width = 400, height = 150, top and left = 0;
        /// </summary>
        public static void XmlSplitting()
        {
            XDocument doc = XDocument.Load(XmlFileName);
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
