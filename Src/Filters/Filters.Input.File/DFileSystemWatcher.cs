using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace D.DevelopTools.LogCollect.Filters.Input.File
{
    /// <summary>
    /// 暂时只允许一个路径，够用了
    /// </summary>
    internal class DFileSystemWatcher : IFileSystemWatcher
    {
        ILogger _logger;
        FileInputFilterOptions _options;

        FileSystemWatcher _watcher;
        Action<IDFile> _fileChanged;

        string _fileExtension;

        public DFileSystemWatcher(
            ILogger<DFileSystemWatcher> logger
            , FileInputFilterOptions options
            )
        {
            _logger = logger;
            _options = options;

            InitWatcher();
        }

        #region IFileSystemWatcher
        public bool Run()
        {
            _watcher.EnableRaisingEvents = true;

            LoadAllFile();

            return true;
        }

        public bool Stop()
        {
            _watcher.EnableRaisingEvents = false;
            return true;
        }

        public void SetFileChangeAction(Action<IDFile> action)
        {
            _fileChanged = action;
        }
        #endregion

        private void InitWatcher()
        {
            _watcher = new FileSystemWatcher();

            _watcher.NotifyFilter =
                  NotifyFilters.LastAccess
                | NotifyFilters.LastWrite
                | NotifyFilters.FileName
                | NotifyFilters.DirectoryName;

            var pathArray = _options.Paths[0].Split('/');

            foreach (var tmp in pathArray)
            {
                if (tmp == "**")
                {
                    break;
                }

                _watcher.Path += $"{tmp}/";
            }

            _watcher.Filter = pathArray[pathArray.Length - 1];

            _watcher.Changed += new FileSystemEventHandler(HandleFileSystemEvent);
            _watcher.Created += new FileSystemEventHandler(HandleFileSystemEvent);
            _watcher.Deleted += new FileSystemEventHandler(HandleFileSystemEvent);
            _watcher.Renamed += new RenamedEventHandler(HadleRenamedEvent);
        }

        private void HandleFileSystemEvent(object sender, FileSystemEventArgs e)
        {

        }

        private void HadleRenamedEvent(object sender, RenamedEventArgs e)
        {

        }

        private async void CheckFile(IDFile file)
        {
            //判断一个文件是否需要处理等等

            _logger.LogDebug($"暂定所有的文件都处理");

            _logger.LogInformation($"{file} 的日志内容需要被处理");

            _fileChanged(file);
        }

        /// <summary>
        /// 加载路径下所有的文件，用来初始检查
        /// </summary>
        private void LoadAllFile()
        {
            _logger.LogInformation($"系统初始启动时，会加载所有的文件，判定是否需要处理");

            var root = new DirectoryInfo(_watcher.Path);

            _fileExtension = _watcher.Filter.Remove(0);

            FindFile(root);
        }

        private void FindFile(DirectoryInfo parent)
        {
            var files = parent.GetFiles(_watcher.Filter);

            foreach (var f in files)
            {
                CheckFile(new DFile(f));
            }

            var directories = parent.GetDirectories();

            foreach (var d in directories)
            {
                FindFile(d);
            }
        }
    }
}
