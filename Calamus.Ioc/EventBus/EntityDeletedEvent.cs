using System;
using System.Collections.Generic;
using System.Text;

namespace Calamus.Ioc.EventBus
{
    /// <summary>
    /// 实体删除事件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntityDeletedEvent<T> : EventBase where T : class
    {
        public EntityDeletedEvent(T entity)
        {

        }
        public T Entity { get; }
    }
}
