using System;
using System.Collections.Generic;
using System.Text;

namespace Calamus.Ioc.EventBus
{
    /// <summary>
    /// 标记 Consumer 是否异步处理
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class HandleAsynchronouslyAttribute:Attribute
    {
    }
}
