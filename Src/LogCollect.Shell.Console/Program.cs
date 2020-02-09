using D.Infrastructures;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using NLog;
using NLog.Extensions.Logging;

namespace D.DevelopTools.LogCollect
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new ApplicationBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    if (args.Length == 2 && args[0] == "-content")
                    {
                        //暂时先写在这里吧，这个
                        hostingContext.Environment.ContentRootPath = hostingContext.Environment.AppRootPath + args[1];
                    }

                    config.SetBasePath(hostingContext.Environment.AppRootPath);

                    config.AddJsonFile("appSettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    logging.AddNLog();
                })
                .UseStartupWithAutofac<Startup>()
                .Builde<LogCollectApp>();

            app.Run();

            System.Console.ReadKey();

            app.Stop();
        }
    }
}
