using System;
using System.Collections.Generic;
using System.Text;

namespace D.DevelopTools.LogCollect.Filters.DRegex
{
    /// <summary>
    /// 字段处理选项
    /// </summary>
    internal class FieldOptions
    {
        public string SrcField { get; set; }

        public string DstField { get; set; }

        public string Pattern { get; set; }
    }
}
