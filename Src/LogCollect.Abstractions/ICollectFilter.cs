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
        /// 初始化 filter
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        bool Init(ICollectFilterOptions options);

        bool Run();

        bool Pause();

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
        void SetOutput(Func<bool, ICollectContext> outputAction);

        /// <summary>
        /// 设置已空的处理
        /// </summary>
        /// <param name="emptyAction"></param>
        void SetEmpty(Action<ICollectFilter> emptyAction);
    }
}
