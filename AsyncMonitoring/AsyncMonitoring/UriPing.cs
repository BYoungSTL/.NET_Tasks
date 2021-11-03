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
        private const string FailedMessage = "Failed request";
        private const string SuccessfulMessage = "Successful request";
        private const string MailFrom = "babyragestl@gmail.com";

        public PingProperties Properties { get; set; }
        
        private Uri _uri;

        /// <summary>
        /// Ping some site, with delay and waiting time
        /// Logging into .txt file
        /// </summary>
        public async Task Ping()
        {
            while (true)
            {
                await JsonDeserialize();
                _uri = new Uri(Properties.UriRef);
                Logger logger = LogManager.GetLogger("http");
                try
                {
                    HttpWebRequest webRequest = WebRequest.CreateHttp(_uri);
                    Sleep(Properties.MaxWaitingTime);
                    HttpWebResponse webResponse = webRequest.GetResponse() as HttpWebResponse;
                    Console.WriteLine(SuccessfulMessage);
                    logger.Debug(SuccessfulMessage);
                }
                catch
                {
                    await SendEmailAsync(FailedMessage, "Fail Response");
                    Console.WriteLine(FailedMessage);
                    logger.Error(FailedMessage);
                }
                Sleep(Properties.Delay);
            }
        }

        

        /// <summary>
        /// Sends email by admin mail to user mail
        /// Sends log with Failed Response
        /// Client connected to SMTP ver. of gmail.com
        /// </summary>
        /// <param name="text"></param>
        /// <param name="subject"></param>
        private async Task SendEmailAsync(string text, string subject)
        {
            MimeMessage emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Admin", MailFrom));
            emailMessage.To.Add(new MailboxAddress("user", Properties.MailTo));
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
            await using FileStream fs = File.OpenRead(Directory.GetCurrentDirectory() + "\\pingSettings.json");
            Properties = await JsonSerializer.DeserializeAsync<PingProperties>(fs);
        }
        
        /// <summary>
        /// overload of JsonDeserialize to call from Ping();
        /// </summary>
        private async Task JsonDeserialize()
        {
            await using FileStream fs = File.OpenRead(Directory.GetCurrentDirectory() + "\\pingSettings.json");
            Properties = await JsonSerializer.DeserializeAsync<PingProperties>(fs);
        }
    }
}