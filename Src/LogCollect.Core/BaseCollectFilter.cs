using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace D.DevelopTools.LogCollect
{
    public abstract class BaseCollectFilter : ICollectFilter
    {
        private FilterState _state;

        protected ILogger _logger;

        protected Func<ICollectContext, bool> _output;
        protected Action<ICollectFilter> _empty;

        protected FilterState State => _state;

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
            _state = FilterState.Stop;
        }

        #region ICollectFilter
        public virtual bool Init(ICollectFilterOptions options)
        {
            return true;
        }

        public bool Run()
        {
            lock (this)
            {
                var isSuccess = true;

                switch (_state)
                {
                    case FilterState.Stop:
                        isSuccess = StopToRun();
                        break;

                    case FilterState.Pause:
                        isSuccess = PauseToRun();
                        break;

                    default:
                        break;
                }

                if (isSuccess) _state = FilterState.Running;

                return isSuccess;
            }
        }

        public bool Pause()
        {
            lock (this)
            {
                var isSuccess = true;

                switch (_state)
                {
                    case FilterState.Running:
                        isSuccess = RunToPause();
                        break;

                    case FilterState.Stop:
                        isSuccess = false;
                        break;

                    default:
                        break;
                }

                if (isSuccess) _state = FilterState.Pause;

                return isSuccess;
            }
        }

        public bool Stop()
        {
            lock (this)
            {
                var isSuccess = true;

                switch (_state)
                {
                    case FilterState.Stop:
                        isSuccess = false;
                        break;

                    default:
                        isSuccess = TryToStop();
                        break;
                }

                _state = FilterState.Stop;

                return isSuccess;
            }
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

        protected virtual bool StopToRun()
        {
            return true;
        }

        protected virtual bool TryToStop()
        {
            return true;
        }

        protected virtual bool RunToPause()
        {
            return true;
        }

        protected virtual bool PauseToRun()
        {
            return true;
        }

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
