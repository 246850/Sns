using System;
using System.Collections.Generic;
using System.Text;

namespace Calamus.Ioc.EventBus
{
    /// <summary>
    /// 实体更新事件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntityUpdatedEvent<T> : EventBase where T : class
    {
        public EntityUpdatedEvent(T entity)
        {

        }
        public T Entity { get; }
    }
}
