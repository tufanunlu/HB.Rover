using HB.Rover.Core;
using HB.Rover.Service;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using static HB.Rover.Core.RoverController;

namespace HB.Rover.ConsoleApp
{
    class Program
    {
        public static void Main(string[] args)
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                                          .AddTransient<IRoverService, RoverService>()
                                          .AddTransient<IRoverController, RoverController>()
                                          .BuildServiceProvider();

            string defaultCmdPath = Path.Combine(new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).Directory.FullName, "cmd.txt");

            while (true)
            {
                Console.WriteLine("Komut dosyasının bulunduğu pathi giriniz.");
                Console.WriteLine("Varsayılan path ({0}) \r\niçin entera basınız.", defaultCmdPath);

                string cmdPath = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(cmdPath))
                    cmdPath = defaultCmdPath;

                if (!File.Exists(cmdPath))
                {
                    Console.WriteLine("Komut dosyası bulunamadı. {0}", cmdPath);
                    continue;
                }

                string cmdText = File.ReadAllText(cmdPath);
                var service = serviceProvider.GetService<IRoverService>();

                List<string> results = new List<string>();

                try
                {
                    results = service.RunCommands(cmdText);
                }
                catch (Exception ex)
                {
                    while (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                    }

                    Console.WriteLine("!!!Hata : {0}\r\n", ex.Message);
                }

                foreach (var result in results)
                {
                    Console.WriteLine(result);
                }
                Console.WriteLine("______________________________________");
            }

        }
    }
}
