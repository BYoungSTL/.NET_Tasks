using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using NET02._2.Entities;

namespace NET02._2.Serializers
{
    public class SerializeXml<T> : ISerializer<T>
    {
        private static readonly string FileName = Directory.GetCurrentDirectory() + "\\Config\\UserSettings.xml";
        public async Task Serialize(List<T> logins)
        {
            XmlSerializer formatterLogin = new XmlSerializer(typeof(List<Login>));
            using FileStream fileStream = new FileStream(FileName, FileMode.OpenOrCreate);
            formatterLogin.Serialize(fileStream, logins);
        }

        public async Task<List<T>> Deserialize()
        {
            XmlSerializer formatterLogin = new XmlSerializer(typeof(List<T>));
            using FileStream fileStream = new FileStream(FileName, FileMode.OpenOrCreate);
            List<T> logins = formatterLogin.Deserialize(fileStream) as List<T>;
            return logins;
        }
    }
}