using System;
using System.Collections.Generic;
using System.Text;

namespace D.DevelopTools.LogCollect
{
    /// <summary>
    /// 管道；
    /// 一个配置文件一个管道
    /// </summary>
    public interface ICollectPipeline
    {
        /// <summary>
        /// 过滤器（已排序）
        /// 怎么感觉不需要对外暴漏
        /// </summary>
        IEnumerable<ICollectFilter> Filters { get; }

        /// <summary>
        /// 开始运行
        /// </summary>
        /// <returns></returns>
        bool Run();

        /// <summary>
        /// 停止运行
        /// </summary>
        /// <returns></returns>
        bool Stop();
    }
}
