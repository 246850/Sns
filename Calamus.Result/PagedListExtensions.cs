using System;
using System.Collections.Generic;
using System.Linq;

namespace Calamus.Result
{
    /// <summary>
    /// 分页结果扩展方法
    /// </summary>
    public static class PagedListExtensions
    {
        /// <summary>
        /// List 构建 IPagedList
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="source"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public static IPagedList<TEntity> ToPagedList<TEntity>(this List<TEntity> source, int page, int pageSize, long total) where TEntity : class
        {
            PagedList<TEntity> pagedList = new PagedList<TEntity>(page, pageSize, total);
            pagedList.AddRange(source);
            return pagedList;
        }

        /// <summary>
        /// IPagedList 领域模型 转 业务模型
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="source"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static IPagedList<TModel> ToPagedList<TEntity, TModel>(this IPagedList<TEntity> source, Func<List<TEntity>, List<TModel>> func)
            where TEntity : class
            where TModel : class
        {
            var entities = source.ToList();
            var models = func.Invoke(entities);
            PagedList<TModel> pagedList = new PagedList<TModel>(source.Pager.Page, source.Pager.PageSize, source.Pager.Total);
            pagedList.AddRange(models);
            return pagedList;
        }

        /// <summary>
        /// IPagedList 转 Pagination - 领域实体转业务模型
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="source"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static Pagination<TModel> ToPagination<TEntity, TModel>(this IPagedList<TEntity> source, Func<List<TEntity>, List<TModel>> func) 
            where TEntity : class 
            where TModel : class
        {
            var entities = source.ToList();
            var models = func.Invoke(entities);

            Pagination<TModel> pagination = new Pagination<TModel>()
            {
                Total = source.Pager.Total,
                List = models
            };
            return pagination;
        }

        /// <summary>
        /// IPagedList 转 Pagination - 领域实体转业务模型 - 包含聚合统计
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="source"></param>
        /// <param name="func"></param>
        /// <param name="aggregates">聚合统计委托</param>
        /// <returns></returns>
        public static Pagination<TModel> ToPagination<TEntity, TModel>(this IPagedList<TEntity> source, Func<List<TEntity>, List<TModel>> func, Action<IDictionary<string, object>> aggregates) where TEntity : class where TModel : class
        {
            Pagination<TModel> pagination = ToPagination(source, func);
            if (aggregates != null)
            {
                if (pagination.Aggregates == null) pagination.Aggregates = new Dictionary<string, object>();
                aggregates.Invoke(pagination.Aggregates);
            }
            return pagination;
        }

        /// <summary>
        /// IPagedList 构建 Pagination
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Pagination<TEntity> ToPagination<TEntity>(this IPagedList<TEntity> source) where TEntity : class
        {
            Pagination<TEntity> pagination = new Pagination<TEntity>()
            {
                Total = source.Pager.Total,
                List = source.ToList()
            };
            return pagination;
        }

        /// <summary>
        /// IQueryable 构建 IPagedList
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="source"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static IPagedList<TEntity> ToPagedList<TEntity>(this IQueryable<TEntity> source, int page, int pageSize) where TEntity : class
        {
            long total = source.LongCount();
            PagedList<TEntity> pagedList = new PagedList<TEntity>(page, pageSize, total);
            pagedList.AddRange(source.Skip((page - 1) * pageSize).Take(pageSize).ToList());
            return pagedList;
        }

        /// <summary>
        /// IQueryable 转 IPagedList - 领域实体转业务模型
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="source"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static IPagedList<TModel> ToPagedList<TEntity, TModel>(this IQueryable<TEntity> source, int page, int pageSize, Func<List<TEntity>, List<TModel>> func)
            where TEntity:class
            where TModel:class
        {
            IPagedList<TEntity> pagedList = source.ToPagedList(page, pageSize);
            return pagedList.ToPagedList(func);
        }

        /// <summary>
        /// IQueryable 转 Pagination - 领域实体转业务模型
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="source"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static Pagination<TModel> ToPagination<TEntity, TModel>(this IQueryable<TEntity> source, int page, int pageSize, Func<List<TEntity>, List<TModel>> func)
            where TEntity : class
            where TModel : class
        {
            IPagedList<TEntity> pagedList = source.ToPagedList(page, pageSize);
            return pagedList.ToPagination(func);
        }

        /// <summary>
        /// IQueryable 转 Pagination - 领域实体转业务模型 - 包含聚合统计
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="source"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="func"></param>
        /// <param name="aggregates">聚合统计</param>
        /// <returns></returns>
        public static Pagination<TModel> ToPagination<TEntity, TModel>(this IQueryable<TEntity> source, int page, int pageSize, Func<List<TEntity>, List<TModel>> func, Action<IDictionary<string, object>> aggregates)
            where TEntity : class
            where TModel : class
        {
            IPagedList<TEntity> pagedList = source.ToPagedList(page, pageSize);
            return pagedList.ToPagination(func, aggregates);
        }

        /// <summary>
        /// IQueryable 转 Pagination - 分页结果
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="source"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static Pagination<TModel> ToPagination<TModel>(this IQueryable<TModel> source, int page, int pageSize) where TModel:class
        {
            long total = source.LongCount();

            Pagination<TModel> pagination = new Pagination<TModel>()
            {
                Total = total,
                List = source.Skip((page - 1) * pageSize).Take(pageSize).ToList()
            };
            return pagination;
        }
    }
}
