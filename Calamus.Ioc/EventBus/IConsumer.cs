using System;
using System.Collections.Generic;
using System.Text;

namespace Calamus.Ioc.EventBus
{
    /// <summary>
    /// 事件消费者 接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IConsumer<T> where T:EventBase
    {
        /// <summary>
        /// 触发事件 - 消费
        /// </summary>
        /// <param name="eventMsg"></param>
        void HandleEvent(T eventMsg);
    }
}
