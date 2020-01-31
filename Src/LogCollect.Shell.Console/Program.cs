using D.Infrastructures;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace D.DevelopTools.LogCollect
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new ApplicationBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(hostingContext.Environment.AppRootPath);

                    config.AddJsonFile("appSettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                })
                .UseStartupWithAutofac<Startup>()
                .Builde<LogCollectApp>();

            app.Run();

            System.Console.ReadKey();

            app.Stop();
        }
    }
}
