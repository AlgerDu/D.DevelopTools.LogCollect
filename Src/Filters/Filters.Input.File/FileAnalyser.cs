using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace D.DevelopTools.LogCollect.Filters.Input.File
{
    internal class FileAnalyser : IFileAnalyser
    {
        ILogger _logger;
        FileInputFilterOptions _options;

        Action<ICollectContext> _dealContext;

        public FileAnalyser(
            ILogger<FileAnalyser> logger
            , FileInputFilterOptions options
            )
        {
            _logger = logger;
            _options = options;
        }

        public Task Analyse(IDFile file)
        {
            return Task.Run(() =>
            {
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

                            context.Fields["path"] = file.FullPath;
                        }

                        if (index <= _options.Mulitline.SpecialLines)
                        {
                            context.Fields[$"line{index}"] = line.TrimStart();

                            index++;
                        }
                        else
                        {
                            message += line + "\r\n";
                        }
                    }
                }
            });
        }

        public void SetDealContextAction(Action<ICollectContext> action)
        {
            _dealContext = action;
        }
    }
}
