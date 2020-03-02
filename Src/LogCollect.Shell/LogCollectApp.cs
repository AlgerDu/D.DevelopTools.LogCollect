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

        ICollectPipelineFactory _pipelineFactory;

        public LogCollectApp(
            ILogger<LogCollectApp> logger
            , IHostingEnvironment environment
            , ICollectPipelineFactory pipelineFactory
            )
        {
            _logger = logger;
            _environment = environment;

            _pipelineFactory = pipelineFactory;
        }

        public IApplication Run()
        {
            _logger.LogInformation($"{this} start to run");

            var json = File.ReadAllText($"{_environment.ContentRootPath}Configs/log-collect-example-config.json");
            var config = new JsonCollectPipelineConfig(json);

            var pipeline = _pipelineFactory.Create(config);

            pipeline.Run();

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
