using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace D.DevelopTools.LogCollect.Filters.Input.File
{
    public class FileInputFilter : BaseCollectFilter, ICollectFilter
    {
        public const string CCode = "file-input";

        IFileSystemWatcher _watcher;
        IFileAnalyser _analyser;

        public override string Code => "file-input";

        public FileInputFilter(
            ILogger<FileInputFilter> logger
            ) : base(logger)
        {
        }

        public override bool Init(ICollectFilterOptions options)
        {
            return true;
        }

        public override bool Run()
        {
            _watcher.Run();

            return true;
        }

        public override bool Stop()
        {
            _watcher.Stop();

            return true;
        }
    }
}
