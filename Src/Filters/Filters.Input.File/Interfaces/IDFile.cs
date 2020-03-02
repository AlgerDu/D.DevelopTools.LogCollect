using System;
using System.Collections.Generic;
using System.Text;

namespace D.DevelopTools.LogCollect.Filters.Input.File
{
    /// <summary>
    /// 文件
    /// </summary>
    internal interface IDFile
    {
        string FullPath { get; }

        string Name { get; }
    }
}
