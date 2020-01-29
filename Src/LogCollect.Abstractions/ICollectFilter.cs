using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace D.DevelopTools.LogCollect
{
    /// <summary>
    /// 过滤器
    /// </summary>
    public interface ICollectFilter
    {
        /// <summary>
        /// 实列唯一 ID
        /// </summary>
        string ID { get; }

        /// <summary>
        /// 类型码
        /// </summary>
        string Code { get; }

        /// <summary>
        /// 初始化 filter
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        bool Init(ICollectFilterOptions options);

        /// <summary>
        /// 输入，简单这样试试
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task Input(ICollectContext context);

        /// <summary>
        /// 设置输出，简单这样试试
        /// </summary>
        /// <param name="outputAction"></param>
        /// <returns></returns>
        bool SetOutput(Action<ICollectContext> outputAction);
    }
}
