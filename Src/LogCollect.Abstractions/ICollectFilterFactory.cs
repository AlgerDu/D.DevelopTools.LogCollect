using System;
using System.Collections.Generic;
using System.Text;

namespace D.DevelopTools.LogCollect
{
    /// <summary>
    /// filter 工厂
    /// </summary>
    public interface ICollectFilterFactory
    {
        /// <summary>
        /// 通过 code 创建一个 filter
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        ICollectFilter Create(string code);
    }
}
