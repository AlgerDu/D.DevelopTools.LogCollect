using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace D.DevelopTools.LogCollect
{
    /// <summary>
    /// 过滤器
    /// </summary>
    public interface ICollectFilter
    {
        /// <summary>
        /// 实列唯一 ID
        /// </summary>
        string ID { get; }

        /// <summary>
        /// 类型码
        /// </summary>
        string Code { get; }

        /// <summary>
        /// 状态
        /// </summary>
        CollectFilterState State { get; }

        /// <summary>
        /// 初始化 filter
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        bool Init(ICollectFilterOptions options);

        /// <summary>
        /// 启动 filter
        /// </summary>
        /// <returns></returns>
        bool Run();

        /// <summary>
        /// 暂停 filter
        /// </summary>
        /// <returns></returns>
        bool Pause();

        /// <summary>
        /// 停止 filter
        /// </summary>
        /// <returns></returns>
        bool Stop();

        /// <summary>
        /// 输入，简单这样试试
        /// </summary>
        /// <param name="context"></param>
        /// <returns>false 代表输入失败，被卡住了，暂时这样吧</returns>
        bool Input(ICollectContext context);

        /// <summary>
        /// 设置输出，简单这样试试
        /// </summary>
        /// <param name="outputAction"></param>
        /// <returns></returns>
        void SetOutput(Func<ICollectContext, bool> outputAction);

        /// <summary>
        /// 设置已空的处理
        /// </summary>
        /// <param name="emptyAction"></param>
        [Obsolete]
        void SetEmpty(Action<ICollectFilter> emptyAction);

        /// <summary>
        /// 设置状态变更的处理；内部自行发生变化，才会发出通知；暂定
        /// </summary>
        /// <param name="stateChangedAction">filter;old state;new state</param>
        void SetStateChanged(Action<ICollectFilter, CollectFilterState, CollectFilterState> stateChangedAction);
    }
}
