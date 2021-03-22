using System;
using System.Collections;
using System.Reflection;

namespace Calamus.Infrastructure.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// 对象初始化默认值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T DefaultValue<T>(this T source)
            where T : class, new()
        {
            if (source == null) source = new T();
            if (source is IEnumerable) return source;   // 集合不初始化

            var typeInfo = typeof(T).GetTypeInfo();
            var props = typeInfo.GetRuntimeProperties();
            foreach (var item in props)
            {
                if (!item.CanWrite) continue;
                object value = item.GetValue(source);
                if (value != null) continue;

                if (item.PropertyType == typeof(string))
                {
                    item.SetValue(source, string.Empty);
                    continue;
                }
                if(item.PropertyType  == typeof(DateTime))
                {
                    item.SetValue(source, DateTime.MinValue);
                    continue;
                }
                if(item.PropertyType.IsClass)
                {
                    object temp = Activator.CreateInstance(item.PropertyType);
                    item.SetValue(source, temp.DefaultValue());
                }
            }
            return source;
        }
        /// <summary>
        /// 对象为Null 抛异常
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static T ThrowIfNull<T>(this T source, string message = "对象未找到")
            where T:class
        {
            return source ?? throw new Exception(message);
        }
    }
}
