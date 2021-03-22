using Calamus.Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Calamus.Infrastructure.Models;

namespace Calamus.Infrastructure.Extensions
{
    /// <summary>
    /// 枚举扩展方法
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// 获取枚举 Value值
        /// </summary>
        /// <typeparam name="TEnum">枚举类型 T</typeparam>
        /// <param name="source">当前实例</param>
        /// <returns>Value值</returns>
        public static int ToValue<TEnum>(this TEnum source) where TEnum : struct
        {
            Type type = typeof(TEnum);
            if (!type.IsEnum)
                throw new ArgumentException("非枚举类型不能调用ToValue()方法", source.ToString());

            int result = Convert.ToInt32(source);
            return result;
        }

        /// <summary>
        /// 获取枚举 文本值
        /// </summary>
        /// <typeparam name="TEnum">枚举类型 T</typeparam>
        /// <param name="source">当前实例</param>
        /// <returns>文本字符串</returns>
        public static string ToText<TEnum>(this TEnum source) where TEnum : struct
        {
            Type type = typeof(TEnum);
            if (!type.IsEnum)
                throw new ArgumentException("非枚举类型不能调用ToText()方法", source.ToString());

            string name = source.ToString();

            FieldInfo field = type.GetField(name);
            if (field != null)
            {
                object[] customAttributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (customAttributes.Length > 0)
                {
                    string text = ((DescriptionAttribute)customAttributes[0]).Description;
                    return text;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// 枚举类型 转换成 TextValueItem 列表 - 不包含默认值
        /// </summary>
        /// <typeparam name="TEnum">枚举类型 T</typeparam>
        /// <param name="source">当前实例</param>
        /// <returns>TextValueItem 列表</returns>
        public static List<TextValueItem> ToTextValueList<TEnum>(this TEnum source) where TEnum : struct
        {
            return ToTextValueList(source, false);
        }

        /// <summary>
        /// 枚举类型 转换成 TextValueItem 列表
        /// </summary>
        /// <typeparam name="TEnum">枚举类型 T</typeparam>
        /// <param name="source">当前实例</param>
        /// <param name="defaulted">是否添加一条默认的数据 value = -1</param>
        /// <param name="defaultText"> text = 请选择</param>
        /// <returns>TextValueItem 列表</returns>
        public static List<TextValueItem> ToTextValueList<TEnum>(this TEnum source, bool defaulted, string defaultText = "请选择") where TEnum : struct
        {
            Type type = source.GetType();
            if (!type.IsEnum)
                throw new Exception("非枚举类型不能调用ToTextValueList()方法");

            List<TextValueItem> items = new List<TextValueItem>();
            if (defaulted)
            {
                items.Add(new TextValueItem
                {
                    Value = -1,
                    Text = defaultText
                });
            }

            Array values = Enum.GetValues(type);
            foreach (TEnum value in values)
            {
                TextValueItem item = new TextValueItem
                {
                    Text = value.ToText(),
                    Value = Convert.ToInt32(value)
                };
                items.Add(item);
            }
            return items;
        }

        /// <summary>
        /// 获取枚举值数组 [1,2,3]
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static List<int> ToValueList<TEnum>(this TEnum source)
        {
            Type type = source.GetType();
            if (!type.IsEnum)
                throw new Exception("非枚举类型不能调用ToValueList()方法");
            Array values = Enum.GetValues(type);

            List<int> results = new List<int>();
            foreach (TEnum value in values)
            {
                results.Add(Convert.ToInt32(value));
            }

            return results;
        }

        /// <summary>
        /// 枚举转成异常
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="source"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static CodeException ToException<TEnum>(this TEnum source, Exception ex = null) where TEnum : struct
        {
            return new CodeException(source.ToValue(), source.ToText(), ex);
        }
    }
}
