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

        ILoggerFactory _loggerFactory;

        IFileSystemWatcher _watcher;
        IFileAnalyser _analyser;

        FileInputFilterOptions _options;

        public override string Code => "file-input";

        public FileInputFilter(
            ILoggerFactory loggerFactory
            ) : base(loggerFactory.CreateLogger<FileInputFilter>())
        {
            _loggerFactory = loggerFactory;
        }

        public override bool Init(ICollectFilterOptions options)
        {
            _options = options.ConvertTo<FileInputFilterOptions>();

            _watcher = new DFileSystemWatcher(_loggerFactory.CreateLogger<DFileSystemWatcher>(), _options);
            _analyser = new FileAnalyser(_loggerFactory.CreateLogger<FileAnalyser>(), _options);

            _watcher.SetFileChangeAction((file) =>
            {
                _analyser.Analyse(file);
            });

            _analyser.SetDealContextAction((collectContext) =>
            {
                _output(collectContext);
            });

            return true;
        }

        protected override bool StopToRun()
        {
            _watcher.Run();
            return true;
        }

        protected override bool PauseToRun()
        {
            _analyser.Pause();
            return true;
        }

        protected override bool RunToPause()
        {
            _analyser.Pause();
            return true;
        }

        protected override bool TryToStop()
        {
            _analyser.Stop();
            _watcher.Stop();
            return true;
        }
    }
}
