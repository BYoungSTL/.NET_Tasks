using System;
using System.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using NLog;
using static System.Threading.Thread;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;


namespace AsyncMonitoring
{
    public class UriPing
    {
        private static readonly string Path = Directory.GetCurrentDirectory() + "//pingSettings.json";
        private const string FailedMessage = "Failed request";
        private const string SuccessfulMessage = "Successful request";
        private const string MailFrom = "babyragestl@gmail.com";

        public List<PingProperties> Properties { get; private set; }
        
        private Uri _uri;

        /// <summary>
        /// Ping some site, with delay and waiting time
        /// Logging into .txt file
        /// </summary>
        public async Task Ping(PingProperties pingProperties)
        {
            while (true)
            {
                await JsonDeserialize();
                _uri = new Uri(pingProperties.UriRef);
                Logger logger = LogManager.GetLogger("http");
                try
                {
                    HttpWebRequest webRequest = WebRequest.CreateHttp(_uri);
                    Sleep(pingProperties.MaxWaitingTime);
                    HttpWebResponse webResponse = webRequest.GetResponse() as HttpWebResponse;
                    logger.Debug(pingProperties.UriRef + " "+ SuccessfulMessage);
                }
                catch
                {
                    await SendEmailAsync(FailedMessage, "Fail Response", pingProperties.MailTo);
                    logger.Error(pingProperties.UriRef + " " + FailedMessage);
                }
                Sleep(pingProperties.Delay);
            }
        }


        /// <summary>
        /// Sends email by admin mail to user mail
        /// Sends log with Failed Response
        /// Client connected to SMTP ver. of gmail.com
        /// You can change MailFrom to other, but you need to keep your password at password.txt
        /// For gmail, you can create an application password
        /// </summary>
        /// <param name="text"></param>
        /// <param name="subject"></param>
        /// <param name="mailTo"></param>
        private async Task SendEmailAsync(string text, string subject, string mailTo)
        {
            MimeMessage emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Admin", MailFrom));
            emailMessage.To.Add(new MailboxAddress("user", mailTo));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("Plain")
            {
                Text = text
            };
        
            string str = await File.ReadAllTextAsync(Directory.GetCurrentDirectory() + "\\password.txt");
            using var client = new SmtpClient();
            await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(MailFrom, str);
            await client.SendAsync(emailMessage);
        }
        
        public async void JsonDeserialize(object sender, FileSystemEventArgs fileSystemEventArgs)
        {
            Console.WriteLine("Changed");
            await using FileStream fs = File.OpenRead(Path);
            Properties = await JsonSerializer.DeserializeAsync<List<PingProperties>>(fs);
        }
        
        /// <summary>
        /// overload of JsonDeserialize to call from Ping();
        /// </summary>
        public async Task JsonDeserialize()
        {
            await using FileStream fs = File.OpenRead(Path);
            Properties = await JsonSerializer.DeserializeAsync<List<PingProperties>>(fs);
        }

        public async Task JsonSerialize()
        {
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                WriteIndented = true,
            };
            await using FileStream fs = new FileStream(Path, FileMode.OpenOrCreate);
            await JsonSerializer.SerializeAsync(fs, Properties, options);
        }
    }
}