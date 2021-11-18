using System;
using System.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using NLog;
using Org.BouncyCastle.Tsp;
using static System.Threading.Thread;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;


namespace AsyncMonitoring
{
    public class UriPing
    {
        public readonly string Path = Directory.GetCurrentDirectory() + "\\pingSettings.json";
        private const string FailedMessage = "Failed request";
        private const string SuccessfulMessage = "Successful request";
        private const string MailFrom = "babyragestl@gmail.com";
        private bool _isNotEqualCount = false;
        public static readonly CancellationTokenSource TokenSource = new CancellationTokenSource();
        private Uri _uri;


        public bool IsNotEqualCount
        {
            get => _isNotEqualCount;
            private set
            {
                _isNotEqualCount = value;
                if (value)
                {
                    Task.Run(()=>PingManager.Start(this));
                }
            }
            
        }

        public int PropCount { get; set; }
        public List<PingProperties> Properties { get; private set; }


        /// <summary>
        /// Ping some site, with delay and waiting time
        /// Logging into .txt file
        /// </summary>
        public async Task Ping(PingProperties pingProperties)
        {
            while (true)
            {
                Logger logger = LogManager.GetLogger("http");
                // if (IsNotEqualCount)
                // {
                //     logger.Info("IsNotEqualCount");
                //     return;
                // }
                await JsonDeserialize();
                _uri = new Uri(pingProperties.UriRef);
                try
                {
                    HttpClient client = new HttpClient();
                    var x = new TaskFactory().StartNew(() => client.GetAsync(_uri));
                    //Console.WriteLine(x.ToString());
                    Sleep(pingProperties.MaxWaitingTime);
                    // HttpWebRequest webRequest = WebRequest.CreateHttp(_uri);
                    // HttpWebResponse webResponse = webRequest.GetResponse() as HttpWebResponse;
                    logger.Debug(pingProperties.UriRef + " " + SuccessfulMessage);
                    Console.WriteLine(pingProperties.UriRef + " " + SuccessfulMessage);
                }
                catch
                {
                    // var shit = new TaskFactory().StartNew(() =>
                    //     SendEmailAsync(FailedMessage, "Fail Response", pingProperties.MailTo));
                    await SendEmailAsync(FailedMessage, "Fail Response", pingProperties.MailTo);
                    logger.Error(pingProperties.UriRef + " " + FailedMessage);
                    Console.WriteLine(pingProperties.UriRef + " " + FailedMessage);
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
            await using FileStream fs = File.OpenRead(Path);
            Properties = await JsonSerializer.DeserializeAsync<List<PingProperties>>(fs);
            // Properties = JsonSerializer.Deserialize<List<PingProperties>>(File.ReadAllText(Path));
            if (PropCount != Properties?.Count)
            {
                IsNotEqualCount = true;
            }
        }
        
        /// <summary>
        /// overload of JsonDeserialize to call from Ping();
        /// </summary>
        private async Task JsonDeserialize()
        {
            //Properties = JsonSerializer.Deserialize<List<PingProperties>>(File.ReadAllText(Path));

            await using FileStream fs = File.OpenRead(Path);
            Properties = await JsonSerializer.DeserializeAsync<List<PingProperties>>(fs);
        }
        
        public void JsonDeserializeSync()
        {
            Properties = JsonSerializer.Deserialize<List<PingProperties>>(File.ReadAllText(Path));

            // await using FileStream fs = File.OpenRead(Path);
            // Properties = await JsonSerializer.DeserializeAsync<List<PingProperties>>(fs);
        }

        public async Task JsonSerialize()
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            await using FileStream fs = new FileStream(Path, FileMode.OpenOrCreate);
            await JsonSerializer.SerializeAsync(fs, Properties, options);
        }
    }
}