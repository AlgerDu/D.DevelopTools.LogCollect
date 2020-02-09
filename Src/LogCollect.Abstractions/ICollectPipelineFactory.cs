using System;
using System.Collections.Generic;
using System.Text;

namespace D.DevelopTools.LogCollect
{
    /// <summary>
    /// 管道工厂
    /// </summary>
    public interface ICollectPipelineFactory
    {
        /// <summary>
        /// 新建一个管道
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        ICollectPipeline Create(ICollectPipelineConfig config);
    }
}
