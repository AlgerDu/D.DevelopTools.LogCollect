using System;
using System.Collections.Generic;
using System.Text;

namespace D.DevelopTools.LogCollect
{
    /// <summary>
    /// filter 可选配置
    /// </summary>
    public interface ICollectFilterOptions
    {
        /// <summary>
        /// filter 的类型码
        /// </summary>
        string FilterCode { get; }

        /// <summary>
        /// 获得一个 T 类型的模型；用于 filter 获取各自的对应的配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T ConvertTo<T>();
    }
}
