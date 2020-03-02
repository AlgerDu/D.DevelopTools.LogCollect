using Autofac;
using D.DevelopTools.LogCollect.Filters;
using D.Infrastructures;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace D.DevelopTools.LogCollect
{
    public class Startup
    {
        IHostingEnvironment _env;
        IConfiguration _config;
        ILoggerFactory _loggerFactory;

        public Startup(
            IHostingEnvironment env
            , IConfiguration config
            , ILoggerFactory loggerFactory
            )
        {
            _env = env;
            _config = config;
            _loggerFactory = loggerFactory;
        }

        public void ConfigOptions(IServiceCollection services)
        {
        }

        public void ConfigServices(IServiceCollection services)
        {
            services.AddSingleton<ICollectFilterProvider, FileInputCollectFilterProvider>();
            services.AddSingleton<ICollectFilterProvider, RegexFilterProvider>();
            services.AddSingleton<ICollectFilterProvider, TidyFieldsProvider>();
            services.AddSingleton<ICollectFilterProvider, ElasticsearchOutputFilterProvider>();
        }

        public void ConfigServices(ContainerBuilder builder)
        {
            builder.AddLogCollectCore();
        }
    }
}
