using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Calamus.Data
{
    /// <summary>
    /// EF Core Repository 接口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface IRepository<TEntity, in TKey> where TEntity : EntityBase<TKey>, new()
    {
        /// <summary>
        /// 追踪表达式
        /// </summary>
        IQueryable<TEntity> Table { get; }
        /// <summary>
        /// 非追踪表达式
        /// </summary>
        IQueryable<TEntity> TableAsNoTracking { get; }

        /// <summary>
        /// 根据主键Id获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity GetByKey(TKey id);

        /// <summary>
        /// 根据主键列表获取 - 批量
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        List<TEntity> GetByKey(IEnumerable<TKey> ids);

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>影响行数</returns>
        int Insert(TEntity entity);

        /// <summary>
        /// 插入 - 批量
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        int Insert(IEnumerable<TEntity> entities);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Update(TEntity entity);

        /// <summary>
        /// 更新 - 批量
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        int Update(IEnumerable<TEntity> entities);

        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int Delete(TKey id);

        /// <summary>
        /// 根据主键删除 - 批量
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        int Delete(IEnumerable<TKey> ids);

        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        int Delete(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 事务执行
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        bool ExecuteTransaction(Action func);

        #region 异步
        /// <summary>
        /// 根据主键Id获取 - 异步
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> GetByKeyAsync(TKey id);

        /// <summary>
        /// 根据主键列表获取 - 批量 - 异步
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<List<TEntity>> GetByKeyAsync(IEnumerable<TKey> ids);

        /// <summary>
        /// 插入 - 异步
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>影响行数</returns>
        Task<int> InsertAsync(TEntity entity);

        /// <summary>
        /// 插入 - 批量 - 异步
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> InsertAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// 更新 - 异步
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(TEntity entity);

        /// <summary>
        /// 更新 - 批量 - 异步
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// 根据主键删除 - 异步
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(TKey id);

        /// <summary>
        /// 根据主键删除 - 批量 - 异步
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(IEnumerable<TKey> ids);

        /// <summary>
        /// 根据条件删除 - 异步
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 事务执行 - 异步
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        Task<bool> ExecuteTransactionAsync(Action func);
        #endregion
    }
}
