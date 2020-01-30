using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace D.DevelopTools.LogCollect
{
    public abstract class BaseCollectFilter : ICollectFilter
    {
        protected ILogger _logger;

        protected Action<ICollectContext> _output;

        #region ICollectFilter
        public string ID { get; protected set; }

        public abstract string Code { get; }
        #endregion

        public BaseCollectFilter(
            ILogger logger
            )
        {
            _logger = logger;

            ID = Guid.NewGuid().ToString();
        }

        #region ICollectFilter
        public virtual bool Init(ICollectFilterOptions options)
        {
            return true;
        }

        public virtual bool Run()
        {
            throw new NotImplementedException();
        }

        public virtual bool Stop()
        {
            throw new NotImplementedException();
        }

        public virtual Task Input(ICollectContext context)
        {
            throw new NotImplementedException();
        }

        public void SetOutput(Action<ICollectContext> outputAction)
        {
            _output = outputAction;
        }
        #endregion
    }
}
