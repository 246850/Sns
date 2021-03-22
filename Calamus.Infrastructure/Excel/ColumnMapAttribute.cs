using System;
using System.Collections.Generic;
using System.Text;

namespace Calamus.Infrastructure.Excel
{
    /// <summary>
    /// DataTable列名 和 实体类属性名 映射 Attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnMapAttribute : Attribute
    {
        public ColumnMapAttribute(string columnName)
        {
            if (string.IsNullOrWhiteSpace(columnName)) throw new ArgumentNullException(columnName);
            ColumnName = columnName;
        }
        /// <summary>
        /// 映射列名
        /// </summary>
        public string ColumnName { get; }
    }
}
