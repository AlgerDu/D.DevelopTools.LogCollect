using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace D.DevelopTools.LogCollect
{
    public class CollectPipeline : ICollectPipeline
    {
        ILogger _logger;

        ICollectPipelineConfig _config;
        IDictionary<int, ICollectFilter> _filters;

        public IEnumerable<ICollectFilter> Filters => _filters.Values;

        public CollectPipeline(
            ILogger<CollectPipeline> logger
            , ICollectPipelineConfig config
            , IDictionary<int, ICollectFilter> filters
            )
        {
            if (filters == null)
            {
                throw new Exception("CollectPipeline filters 参数不能为 null");
            }

            _logger = logger;
            _config = config;
            _filters = filters;

            Init();
        }

        public bool Run()
        {
            var count = _filters.Count;

            for (var i = count; i > 0; i--)
            {
                _filters[i].Run();
            }

            return true;
        }

        public bool Stop()
        {
            var count = _filters.Count;

            for (var i = 0; i < count; i++)
            {
                _filters[i].Stop();
            }

            return true;
        }

        private void Init()
        {
            for (var i = 1; i < _filters.Count; i++)
            {
                var p = _filters[i];
                var n = _filters[i + 1];

                p.SetOutput((context) =>
                {
                    return n.Input(context);
                });
            }

            //TODO 下面先这样写着吧
            var last = _filters[_filters.Count];
            var first = _filters[1];

            last.SetEmpty((filter) =>
            {
                first.Run();
            });
        }
    }
}
