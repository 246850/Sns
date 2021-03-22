using System;
using System.Collections.Generic;
using System.Text;

namespace Calamus.Ioc.EventBus
{
    /// <summary>
    /// 事件消息 基类
    /// </summary>
    [Serializable]
    public abstract class EventBase
    {
        public EventBase()
        {
            Guid = Guid.NewGuid();
        }
        public Guid Guid { get; set; }
    }
}
