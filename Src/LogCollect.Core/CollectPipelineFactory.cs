using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace D.DevelopTools.LogCollect
{
    /// <summary>
    /// 管道工厂
    /// </summary>
    public class CollectPipelineFactory : ICollectPipelineFactory
    {
        ILoggerFactory _loggerFactory;
        ILogger _logger;
        ICollectFilterFactory _filterFactory;

        public CollectPipelineFactory(
            ILoggerFactory loggerFactory
            , ICollectFilterFactory filterFactory
            )
        {
            _loggerFactory = loggerFactory;
            _logger = _loggerFactory.CreateLogger<CollectPipeline>();
            _filterFactory = filterFactory;
        }

        public ICollectPipeline Create(ICollectPipelineConfig config)
        {
            Dictionary<int, ICollectFilter> filters = new Dictionary<int, ICollectFilter>();
            var index = 1;

            foreach (var options in config.FilterOptions)
            {
                var filter = _filterFactory.Create(options.FilterCode);

                filter.Init(options);
                filters.Add(index++, filter);
            }

            return new CollectPipeline(_loggerFactory.CreateLogger<CollectPipeline>(), config, filters);
        }
    }
}
