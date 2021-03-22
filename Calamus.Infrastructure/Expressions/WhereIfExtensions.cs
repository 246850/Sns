using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Calamus.Infrastructure.Expressions
{
    /// <summary>
    /// IQueryable Where条件扩展
    /// </summary>
    public static class WhereIfExtensions
    {
        /// <summary>
        /// 条件 Where
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="condition"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, bool>> predicate)
            where TSource:class
        {
            return condition ? source.Where(predicate) : source;
        }

        /// <summary>
        /// 时间判断 x=> x.PropertyName 大于等于 date
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="keySelector"></param>
        /// <param name="begin"></param>
        /// <returns></returns>
        public static IQueryable<TSource> WhereBegin<TSource, TKey>(this IQueryable<TSource> queryable, Expression<Func<TSource, TKey>> keySelector, DateTime? date)
            where TSource : class
        {
            if (!date.HasValue) return queryable;

            DateTime begin = date.Value.Date;
            var name = ExpressionHelper.GetPropertyName(keySelector);
            Expression<Func<TSource, bool>> predicate = ExpressionHelper.CreateGreaterThanOrEqual<TSource>(name, begin);
            return queryable.Where(predicate);
        }

        /// <summary>
        /// 时间判断 x=> x.PropertyName 小于 date
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="keySelector"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static IQueryable<TSource> WhereEnd<TSource, TKey>(this IQueryable<TSource> queryable, Expression<Func<TSource, TKey>> keySelector, DateTime? date)
            where TSource : class
        {
            if (!date.HasValue) return queryable;

            DateTime end = date.Value.Date.AddDays(1);
            var name = ExpressionHelper.GetPropertyName(keySelector);
            Expression<Func<TSource, bool>> predicate = ExpressionHelper.CreateLessThan<TSource>(name, end);
            return queryable.Where(predicate);
        }
    }
}
