using System;
using System.Collections.Generic;
using System.Text;

namespace D.DevelopTools.LogCollect.Filters.Output.Elasticsearch
{
    public class ElasticsearchOutputFilterOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Index { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<string> Fields { get; set; }
    }
}
