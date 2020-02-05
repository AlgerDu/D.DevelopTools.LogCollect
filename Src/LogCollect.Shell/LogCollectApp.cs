using D.Infrastructures;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace D.DevelopTools.LogCollect
{
    public class LogCollectApp : IApplication
    {
        ILogger _logger;
        IHostingEnvironment _environment;

        ICollectFilterFactory _filterFactory;

        public LogCollectApp(
            ILogger<LogCollectApp> logger
            , IHostingEnvironment environment
            , ICollectFilterFactory filterFactory
            )
        {
            _logger = logger;
            _environment = environment;

            _filterFactory = filterFactory;
        }

        public IApplication Run()
        {
            _logger.LogInformation($"{this} start to run");

            var json = File.ReadAllText($"{_environment.ContentRootPath}Configs/log-collect-example-config.json");

            var config = new JsonCollectPipelineConfig(json);

            var fileInput = _filterFactory.Create(config.FilterOptions[0].FilterCode);
            var regex = _filterFactory.Create(config.FilterOptions[1].FilterCode);
            var tidy = _filterFactory.Create(config.FilterOptions[2].FilterCode);

            fileInput.SetOutput((context) =>
            {
                regex.Input(context);
            });

            regex.SetOutput((context) =>
            {
                tidy.Input(context);
            });

            tidy.SetOutput((context) =>
            {
                _logger.LogDebug(context.Fields.ToString());
            });

            fileInput.Init(config.FilterOptions[0]);
            regex.Init(config.FilterOptions[1]);
            tidy.Init(config.FilterOptions[2]);

            fileInput.Run();

            _logger.LogInformation($"{this} is running");
            return this;
        }

        public IApplication Stop()
        {
            _logger.LogInformation($"{this} start to stop");


            _logger.LogInformation($"{this} is stopped");
            return this;
        }

        public override string ToString()
        {
            return "log collect tool";
        }
    }
}
