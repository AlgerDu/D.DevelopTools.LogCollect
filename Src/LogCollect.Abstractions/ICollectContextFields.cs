using System;
using System.Collections.Generic;
using System.Text;

namespace D.DevelopTools.LogCollect
{
    /// <summary>
    /// 上下文中的一些自定义字段
    /// </summary>
    public interface ICollectContextFields
    {
        string this[string key] { get; set; }

        bool Remove(string key);
    }
}
