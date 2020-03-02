using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace D.DevelopTools.LogCollect.Filters.Input.File
{
    /// <summary>
    /// 文件分析
    /// </summary>
    internal interface IFileAnalyser
    {
        bool Pause();

        bool Stop();

        /// <summary>
        /// 分析文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        void Analyse(IDFile file);

        /// <summary>
        /// 设置处理分析出来的上下文的 action
        /// </summary>
        /// <param name="action"></param>
        void SetDealContextAction(Action<ICollectContext> action);
    }
}
