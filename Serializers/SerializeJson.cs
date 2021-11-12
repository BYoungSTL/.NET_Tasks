using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using NET02._2.Entities;

namespace NET02._2.Serializers
{
    public class SerializeJson<T> : ISerializer<T>
    {
        private readonly string _fileName = Directory.GetCurrentDirectory() + "\\Config\\UserSettings.json";
        public async Task<List<T>> Deserialize()
        {
            await using FileStream fs = new FileStream(_fileName, FileMode.OpenOrCreate);
                
            return await JsonSerializer.DeserializeAsync<List<T>>(fs);;
        }

        public async Task Serialize(List<T> logins)
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
                IgnoreNullValues = true
            };
            await using FileStream fs = new FileStream(_fileName, FileMode.OpenOrCreate);
            await JsonSerializer.SerializeAsync(fs, logins, options);
        }
    }
}