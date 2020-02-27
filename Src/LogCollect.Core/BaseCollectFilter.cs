using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace D.DevelopTools.LogCollect
{
    public abstract class BaseCollectFilter : ICollectFilter
    {
        private CollectFilterState _state;

        protected ILogger _logger;

        private Func<ICollectContext, bool> _output;
        private Action<ICollectFilter, CollectFilterState, CollectFilterState> _stateChanged;

        #region ICollectFilter
        public string ID { get; protected set; }

        public abstract string Code { get; }

        public CollectFilterState State
        {
            get => _state;
            private set
            {
                _logger.LogInformation($"{this} state[{_state} => {value}]");
                _state = value;
            }
        }
        #endregion

        public BaseCollectFilter(
            ILogger logger
            )
        {
            _logger = logger;

            ID = Guid.NewGuid().ToString();
            _state = CollectFilterState.Stopped;
        }

        #region ICollectFilter
        public virtual bool Init(ICollectFilterOptions options)
        {
            return true;
        }

        public bool Run()
        {
            if (State == CollectFilterState.Starting || State == CollectFilterState.Running)
            {
                LogCannotChangeState(CollectFilterState.Running);
                return true;
            }

            lock (this)
            {
                State = CollectFilterState.Starting;
                var isSuccess = false;

                try
                {
                    isSuccess = StartFilter();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"{this} 启动出现异常：[{ex}]");
                }

                State = isSuccess ? CollectFilterState.Running : CollectFilterState.Stopped;

                return isSuccess;
            }
        }

        public bool Pause()
        {
            if (State != CollectFilterState.Running)
            {
                LogCannotChangeState(CollectFilterState.Stuck);
                return false;
            }

            lock (this)
            {
                StackFilter();

                return true;
            }
        }

        public bool Stop()
        {
            lock (this)
            {
                var isSuccess = true;

                var old = State;
                State = FilterState.Stop;

                switch (old)
                {
                    case FilterState.Stop:
                        isSuccess = false;
                        break;

                    default:
                        isSuccess = TryToStop();
                        break;
                }

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

        protected virtual bool StartFilter()
        {
            return true;
        }

        protected virtual void StackFilter()
        {
        }

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

        protected void ChangeStateAndNotify(CollectFilterState newState)
        {
            var oldState = State;
            State = newState;

            if (_stateChanged != null)
            {
                _stateChanged(this, oldState, newState);
            }
        }

        public override string ToString()
        {
            return $"{Code}";
        }

        public void SetStateChanged(Action<ICollectFilter, CollectFilterState, CollectFilterState> stateChangedAction)
        {
            throw new NotImplementedException();
        }

        private void LogCannotChangeState(CollectFilterState newState)
        {
            _logger.LogWarning($"{this} 不能从 [{State}] 变更为 [{newState}] 状态");
        }
    }
}
