using D.DevelopTools.LogCollect.Filters.Output.Elasticsearch;
using System;
using System.Collections.Generic;
using System.Text;

namespace D.DevelopTools.LogCollect.Filters
{
    public class ElasticsearchOutputFilterProvider : ICollectFilterProvider
    {
        public IDictionary<string, Type> Get()
        {
            return new Dictionary<string, Type>
            {
                {ElasticsearchOutputFilter.CCode,typeof(ElasticsearchOutputFilter) }
            };
        }
    }
}
