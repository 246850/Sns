using System;
using System.Collections.Generic;
using System.Text;

namespace Calamus.Ioc.EventBus
{
    /// <summary>
    /// IEventPublisher 扩展
    /// </summary>
    public static class EventPublisherExtensions
    {
        /// <summary>
        /// 实体添加事件方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eventPublisher"></param>
        /// <param name="entity"></param>
        public static void EntityInserted<T>(this IEventPublisher eventPublisher, T entity) where T : class
        {
            eventPublisher.Publish(new EntityInsertedEvent<T>(entity));
        }

        /// <summary>
        /// 实体更新事件方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eventPublisher"></param>
        /// <param name="entity"></param>
        public static void EntityUpdated<T>(this IEventPublisher eventPublisher, T entity) where T : class
        {
            eventPublisher.Publish(new EntityUpdatedEvent<T>(entity));
        }

        /// <summary>
        /// 实体删除事件方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eventPublisher"></param>
        /// <param name="entity"></param>
        public static void EntityDeleted<T>(this IEventPublisher eventPublisher, T entity) where T : class
        {
            eventPublisher.Publish(new EntityDeletedEvent<T>(entity));
        }
    }
}
