using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace D.DevelopTools.LogCollect.Filters.Input.File
{
    internal class FileAnalyser : IFileAnalyser
    {
        ILogger _logger;
        FileInputFilterOptions _options;

        Action<ICollectContext> _dealContext;

        Queue<IDFile> _queue;

        ManualResetEvent mre_pauseToStart;
        ManualResetEvent mre_addFile;

        bool _isPause;

        public FileAnalyser(
            ILogger<FileAnalyser> logger
            , FileInputFilterOptions options
            )
        {
            _logger = logger;
            _options = options;

            mre_pauseToStart = new ManualResetEvent(true);
            mre_addFile = new ManualResetEvent(false);

            _isPause = false;
            _queue = new Queue<IDFile>();

            AnyThresd();
        }


        public bool Pause()
        {
            lock (this)
            {
                _isPause = !_isPause;

                if (_isPause)
                {
                    mre_pauseToStart.Reset();
                }
                else
                {
                    mre_pauseToStart.Set();
                }
            }
            return true;
        }

        public bool Stop()
        {
            mre_pauseToStart.Set();
            mre_addFile.Set();

            return true;
        }

        public void Analyse(IDFile file)
        {
            lock (this)
            {
                _logger.LogInformation($"{file} 加入待处理队列");
                _queue.Enqueue(file);
                mre_addFile.Set();
            }
        }

        private void AnyThresd()
        {
            Task.Run(() =>
            {
                IDFile file = null;

                while (true)
                {
                    lock (this)
                    {
                        if (_queue.Count > 0)
                            file = _queue.Dequeue();
                    }

                    if (file != null)
                    {
                        _logger.LogInformation($"开始处理 {file}");

                        using (var sr = new StreamReader(file.FullPath))
                        {
                            var line = "";
                            var index = 1;
                            var message = "";

                            var context = new CollectContext();

                            while ((line = sr.ReadLine()) != null)
                            {
                                if (!Regex.IsMatch(line, _options.Mulitline.Pattern))
                                {
                                    context.Fields["message"] = message;
                                    _dealContext(context);

                                    context = new CollectContext();
                                    index = 1;
                                    message = "";

                                    context.Fields["path"] = file.FullPath;
                                    context.Fields["basefilename"] = file.Name.Split('.')[0].ToLower();
                                }

                                line = line.TrimStart();

                                if (index <= _options.Mulitline.SpecialLines)
                                {
                                    context.Fields[$"line{index}"] = line;

                                    index++;
                                }
                                else
                                {
                                    message += line + "\r\n";
                                }

                                mre_pauseToStart.WaitOne();
                            }
                        }

                        _logger.LogInformation($"{file} 处理完成");
                    }

                    if (file == null)
                    {
                        mre_addFile.WaitOne();
                    }

                    file = null;
                }
            });
        }

        public void SetDealContextAction(Action<ICollectContext> action)
        {
            _dealContext = action;
        }
    }
}
