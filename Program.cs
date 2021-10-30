using System;
using System.Collections.Generic;
using NET02._2.Entities;

namespace NET02._2
{
    class Program
    {
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
            Login login1 = new Login()
            {
                Name = "Guf",
                Windows = new List<Window>{first, second}
            };
            Login login2 = new Login()
            {
                Name = "Fuf",
                Windows = new List<Window>{first, third}
            };
            List<Login> logins = new List<Login> {login1, login2};
            Serializer.XmlSerialize(logins);
            logins = Serializer.XmlDeserialize();
            if (logins != null)
            {
                foreach (var login in logins)
                {
                    if (!Serializer.IsLoginCorrect(login))
                    {
                        Console.WriteLine($"{login.Name} : Isn't correct user");
                        foreach (var window in login.Windows)
                        {
                            Console.WriteLine($"Title: {window.Title}\nTop: {window.Top} " +
                                              $"Bottom: {window.Width} " +
                                              $"Left: {window.Left} " +
                                              $"Right: {window.Height} ");
                        }
                    }
                }
            }

            Serializer.XmlSplitting();
            if (logins != null)
                foreach (var login in logins)
                {
                    Serializer.JsonSerialize(login);
                }
            
        }
    }
}