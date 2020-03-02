using System;
using System.Collections.Generic;
using System.Text;

namespace D.DevelopTools.LogCollect
{
    /// <summary>
    /// 收集的上下文
    /// </summary>
    public interface ICollectContext
    {
        /// <summary>
        /// 字段
        /// </summary>
        ICollectContextFields Fields { get; }
    }
}
