using System.Collections.Generic;
using System.Threading.Tasks;
using NET02._2.Entities;
using NET02._2.Serializers;

namespace NET02._2
{
    static class Program
    {
        
        private static async Task Operations(List<Login> logins)
        {
            SerializeJson<Login> serializeJson = new SerializeJson<Login>();
            SerializeXml<Login> serializeXml = new SerializeXml<Login>();
            
            await serializeJson.Serialize(logins);
            logins = await serializeJson.Deserialize();
            
            await serializeXml.Serialize(logins);
            logins = await serializeXml.Deserialize();
            XmlSplitter.XmlSplitting();
            
            if (logins != null)
                await serializeJson.Serialize(logins);
        }
        
        static void Main(string[] args)
        {
            
            Window first = new Window
            {
                Height = 10,
                Width = 10,
                Top = null,
                Left = 10,
                Title = "title1"
            };
            Window second = new Window
            {
                Height = 15,
                Width = 15,
                Top = 15,
                Left = 15,
                Title = "title2"
            };
            Window third = new Window
            {
                Height = 15,
                Width = 15,
                Top = 15,
                Left = 15,
                Title = "main"
            };
            Login login1 = new Login
            {
                Name = "Guf",
                Windows = new List<Window>{first, second}
            };
            Login login2 = new Login
            {
                Name = "Fuf",
                Windows = new List<Window>{first, third}
            };
            List<Login> logins = new List<Login> {login1, login2};

            Task.Run(async () => 
                await Operations(logins));
        }
    }
}