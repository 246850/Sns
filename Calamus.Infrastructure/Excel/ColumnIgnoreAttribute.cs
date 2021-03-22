using System;
using System.Collections.Generic;
using System.Text;

namespace Calamus.Infrastructure.Excel
{
    /// <summary>
    /// DataTable列名 和 实体类属性名 忽略映射
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnIgnoreAttribute : Attribute
    {
        public ColumnIgnoreAttribute() : this(true)
        {

        }

        public ColumnIgnoreAttribute(bool ignore)
        {
            Ignore = ignore;
        }
        /// <summary>
        /// 是否忽略
        /// </summary>
        public bool Ignore { get; }
    }
}
