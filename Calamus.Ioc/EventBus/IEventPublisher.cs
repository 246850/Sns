using System;
using System.Collections.Generic;
using System.Text;

namespace Calamus.Ioc.EventBus
{
    /// <summary>
    /// 事件发布者接口
    /// </summary>
    public interface IEventPublisher
    {
        /// <summary>
        /// 发布事件
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="eventMsg"></param>
        void Publish<TEvent>(TEvent eventMsg) where TEvent : EventBase;
    }
}
