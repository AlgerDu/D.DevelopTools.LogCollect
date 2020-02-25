using System;
using System.Collections.Generic;
using System.Text;

namespace D.DevelopTools.LogCollect
{
    /// <summary>
    /// 过滤器状态
    /// </summary>
    public enum CollectFilterState
    {
        /// <summary>
        /// 停止的
        /// </summary>
        Stopped,

        /// <summary>
        /// 卡住的；不能再处理输入
        /// </summary>
        Stuck,

        /// <summary>
        /// 运行中
        /// </summary>
        Running
    }
}
