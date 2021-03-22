using System;
using System.Collections.Generic;
using System.Text;

namespace Calamus.Ioc.EventBus
{
    /// <summary>
    /// 实体插入事件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntityInsertedEvent<T>:EventBase where T:class
    {
        public EntityInsertedEvent(T entity)
        {
            Entity = entity;
        }
        public T Entity { get; }
    }
}
