using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace Calamus.Infrastructure.Excel
{
    /// <summary>
    /// DataTable List互转 扩展类
    /// </summary>
    public static class DataTable2ListExtensions
    {
        /// <summary>
        /// List 转 DataTable
        /// </summary>
        /// <typeparam name="T">T 类型</typeparam>
        /// <param name="list">T 对象集合</param>
        /// <returns>DataTable 对象</returns>
        public static DataTable List2DataTable<T>(this IList<T> list) where T : class, new()
        {
            DataTable table = new DataTable();

            //*******************设置 Column 列名*******************
            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                //  存在忽略 特性 不映射
                if (CheckIgnore(property)) continue;

                string columnName = BuildColumnName(property);
                if (table.Columns.Contains(columnName)) throw new Exception("存在相同的列名");

                DataColumn column = new DataColumn(columnName, GetCoreType(property.PropertyType));
                table.Columns.Add(column);
            }
            //*******************遍历 添加行*******************
            foreach (T item in list)
            {
                DataRow row = table.NewRow();

                foreach (PropertyInfo property in properties)
                {
                    //  存在忽略 特性 不映射
                    if (CheckIgnore(property)) continue;

                    string columnName = BuildColumnName(property);
                    row[columnName] = property.GetValue(item, null);
                }
                table.Rows.Add(row);
            }
            return table;
        }

        /// <summary>
        /// DataTable 转 List
        /// </summary>
        /// <typeparam name="T">T 类型</typeparam>
        /// <param name="table">DataTable 对象</param>
        /// <returns>T 对象集合</returns>
        public static List<T> DataTable2List<T>(this DataTable table) where T : class, new()
        {
            List<T> list = new List<T>();
            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (DataRow row in table.Rows)
            {
                string rowValue = row[0].ToString();
                if (row.IsNull(0) || string.IsNullOrWhiteSpace(rowValue)) continue;
                T obj = new T();
                foreach (PropertyInfo property in properties)
                {
                    //  存在忽略 特性 不映射
                    if (CheckIgnore(property)) continue;

                    object value = null;
                    string columnName = BuildColumnName(property);
                    if (table.Columns.Contains(columnName))
                    {
                        value = row[columnName];
                    }
                    if (value != null && value != DBNull.Value && property.CanWrite)
                    {
                        Type type = GetCoreType(property.PropertyType);
                        property.SetValue(obj, Convert.ChangeType(value, type));
                    }
                }
                list.Add(obj);
            }
            return list;
        }

        #region - 私有方法
        /// <summary>
        /// 是否 可空类型
        /// </summary>
        /// <param name="t">类型 t</param>
        /// <returns></returns>
        static bool IsNullable(Type t)
        {
            return !t.IsValueType || (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>));
        }
        /// <summary>
        /// 获取真实 类型
        /// </summary>
        /// <param name="t">类型 t</param>
        /// <returns></returns>
        static Type GetCoreType(Type t)
        {
            if (t != null && IsNullable(t))
            {
                if (!t.IsValueType)
                {
                    return t;
                }
                return Nullable.GetUnderlyingType(t);
            }
            return t;
        }

        /// <summary>
        /// 获取属性对应的 列名 -- （列名映射存在，取映射名。反之，取属性名）
        /// </summary>
        /// <param name="property">属性</param>
        /// <returns>映射列名</returns>
        static string BuildColumnName(PropertyInfo property)
        {
            ColumnMapAttribute map = property.GetCustomAttribute<ColumnMapAttribute>();
            return map != null ? map.ColumnName : property.Name;
        }

        /// <summary>
        /// 检查是否忽略 属性
        /// </summary>
        /// <param name="property">属性</param>
        /// <returns>true | false</returns>
        static bool CheckIgnore(PropertyInfo property)
        {
            ColumnIgnoreAttribute ignore = property.GetCustomAttribute<ColumnIgnoreAttribute>();
            return ignore != null && ignore.Ignore;
        }
        #endregion
    }
}
