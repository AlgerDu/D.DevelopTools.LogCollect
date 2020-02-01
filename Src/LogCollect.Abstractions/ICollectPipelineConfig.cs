using System;
using System.Collections.Generic;
using System.Text;

namespace D.DevelopTools.LogCollect
{
    /// <summary>
    /// 管道的配置；可以是从配置文件中读取出来的
    /// </summary>
    public interface ICollectPipelineConfig
    {
        /// <summary>
        /// 配置的所有过滤器；
        /// 按照顺序
        /// </summary>
        IList<ICollectFilterOptions> FilterOptions { get; }
    }
}
