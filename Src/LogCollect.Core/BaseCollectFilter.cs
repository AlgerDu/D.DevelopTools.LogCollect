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

        protected Func<ICollectContext, bool> _output;
        protected Action<ICollectFilter> _empty;

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
            return true;
        }

        public virtual bool Pause()
        {
            return true;
        }

        public virtual bool Stop()
        {
            return true;
        }

        public virtual bool Input(ICollectContext context)
        {
            return true;
        }

        public void SetOutput(Func<ICollectContext, bool> outputAction)
        {
            _output = outputAction;
        }
        public void SetEmpty(Action<ICollectFilter> emptyAction)
        {
            _empty = emptyAction;
        }
        #endregion

        protected bool OutputContext(ICollectContext context)
        {
            return _output(context);
        }

        protected void NotiifyPipelineEmpty()
        {
            _empty(this);
        }
    }
}
