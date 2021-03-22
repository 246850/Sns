using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Calamus.Ioc.EventBus
{
    public class EventPublisher : IEventPublisher
    {
        public void Publish<TEvent>(TEvent eventMsg) where TEvent : EventBase
        {
            var consumers = EngineContext.Current.GetServices<IConsumer<TEvent>>();
            foreach (var consumer in consumers)
            {
                PublishToConsumer(consumer, eventMsg);
            }
        }

        void PublishToConsumer<TEvent>(IConsumer<TEvent> consumer, TEvent eventMsg) where TEvent : EventBase
        {
            try
            {
                if(consumer.GetType().IsDefined(typeof(HandleAsynchronouslyAttribute), false))
                {
                    Task.Run(() => consumer.HandleEvent(eventMsg));    // 异步执行
                }
                else
                {
                    consumer.HandleEvent(eventMsg);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
