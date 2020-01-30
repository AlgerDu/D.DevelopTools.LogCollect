using System;
using System.Collections.Generic;
using System.Text;

namespace D.DevelopTools.LogCollect.Filters.Input.File
{
    /// <summary>
    /// 文件系统监听器
    /// </summary>
    internal interface IFileSystemWatcher
    {
        /// <summary>
        /// 启动时，要获取所有的符合条件的文件，去进行分析
        /// </summary>
        /// <returns></returns>
        bool Run();

        bool Stop();

        void SetFileChangeAction(Action<IDFile> action);
    }
}
