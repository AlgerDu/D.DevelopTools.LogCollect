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

        Task Input(ICollectContext context);

        bool SetOutput(Action<ICollectContext> outputAction);
    }
}
