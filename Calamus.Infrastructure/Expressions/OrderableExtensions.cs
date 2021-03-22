using Calamus.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Calamus.Infrastructure.Expressions
{
    /// <summary>
    /// IQueryable排序扩展
    /// </summary>
    public static class OrderableExtensions
    {
        /// <summary>
        /// 构建排序表达式
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="field">排序字段</param>
        /// <param name="direction">排序方向 ASC|DESC</param>
        /// <returns></returns>
        public static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> queryable, string field, string direction) where TSource:class
        {
            if(queryable.Expression.Type == typeof(IOrderedQueryable<TSource>))
            {
                // 多字段排序
                return ThenBy((IOrderedQueryable<TSource>)queryable, field, direction);
            }

            string command = string.Equals(direction, "ASC", StringComparison.OrdinalIgnoreCase) ? "OrderBy" : "OrderByDescending";

            var type = typeof(TSource);
            var property = type.GetProperty(field);
            if (property == null) throw new Exception($"类{type.FullName}不包含{field}属性，无法排序");
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { type, property.PropertyType }, queryable.Expression, Expression.Quote(orderByExpression));

            return (IOrderedQueryable<TSource>)queryable.Provider.CreateQuery<TSource>(resultExpression);
        }

        /// <summary>
        /// 构建排序，多个
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="orderable"></param>
        /// <param name="field">排序字段</param>
        /// <param name="direction">排序方向 ASC|DESC</param>
        /// <returns></returns>
        public static IOrderedQueryable<TSource> ThenBy<TSource>(this IOrderedQueryable<TSource> orderable, string field, string direction) where TSource : class
        {
            string command = string.Equals(direction, "ASC", StringComparison.OrdinalIgnoreCase) ? "ThenBy" : "ThenByDescending";

            var type = typeof(TSource);
            var property = type.GetProperty(field);
            if (property == null) throw new Exception($"类{type.FullName}不包含{field}属性，无法排序");
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { type, property.PropertyType }, orderable.Expression, Expression.Quote(orderByExpression));

            return (IOrderedQueryable<TSource>)orderable.Provider.CreateQuery<TSource>(resultExpression);
        }

        /// <summary>
        /// 构建排序表达式
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="sorts"></param>
        /// <returns></returns>
        public static IQueryable<TSource> ApplyOrderBy<TSource>(this IQueryable<TSource> queryable, IEnumerable<Sorting> sorts) where TSource : class
        {
            queryable = sorts.Aggregate(queryable, (current, sort) => OrderBy(current, sort.Field, sort.Direction));

            return queryable;
        }

        /// <summary>
        /// 条件 OrderBy
        /// </summary>
        /// <typeparam name="TSource">实体类型</typeparam>
        /// <typeparam name="TKey">排序字段类型</typeparam>
        /// <param name="queryable">IQueryable表达式</param>
        /// <param name="condition">true：排序，false：不排序</param>
        /// <param name="keySelector">字段筛选表达式</param>
        /// <param name="descending">true：降序，false：升序</param>
        /// <returns></returns>
        public static IQueryable<TSource> OrderByIf<TSource, TKey>(this IQueryable<TSource> queryable, bool condition, Expression<Func<TSource, TKey>> keySelector, bool descending = true)
        {
            if (!condition) return queryable;

            if(queryable is IOrderedQueryable<TSource> orderedQueryable)    // 已经排序过，后面的排序应为 ThenBy | ThenByDescending
            {
                return descending ? orderedQueryable.ThenByDescending(keySelector) : orderedQueryable.ThenBy(keySelector);
            }

            return descending ? queryable.OrderByDescending(keySelector) : queryable.OrderBy(keySelector);
        }
    }
}
