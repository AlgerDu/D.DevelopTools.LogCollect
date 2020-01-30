using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace D.DevelopTools.LogCollect.Filters.Input.File
{
    public class FileInputFilter : ICollectFilter
    {
        ILogger _logger;

        public string ID { get; private set; }

        public string Code => "file-input";

        public FileInputFilter(
            ILogger<FileInputFilter> logger
            )
        {
            _logger = logger;
        }

        #region ICollectFilter
        public bool Init(ICollectFilterOptions options)
        {
            throw new NotImplementedException();
        }

        public Task Input(ICollectContext context)
        {
            throw new NotImplementedException();
        }

        public bool SetOutput(Action<ICollectContext> outputAction)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
