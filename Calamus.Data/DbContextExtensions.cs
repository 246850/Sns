using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Calamus.Data
{
    /// <summary>
    /// DbContext 实用扩展方法
    /// </summary>
    public static class DbContextExtensions
    {
        #region Transaction
        /// <summary>
        /// 事务执行
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <param name="context"></param>
        /// <param name="action"></param>
        /// <param name="isolationLevel"></param>
        public static void ExecuteTransaction<TDbContext>(this TDbContext context, Action action, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
            where TDbContext : DbContext
            => ExecuteTransaction(context, context => action(), isolationLevel);

        /// <summary>
        /// 事务执行
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <param name="context"></param>
        /// <param name="action"></param>
        /// <param name="isolationLevel"></param>
        public static void ExecuteTransaction<TDbContext>(this TDbContext context, Action<TDbContext> action, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
            where TDbContext : DbContext
            => ExecuteTransaction(context, (context, connection, transaction) => action(context), isolationLevel);
        /// <summary>
        /// 事务执行
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <param name="context"></param>
        /// <param name="action"></param>
        /// <param name="isolationLevel"></param>
        public static void ExecuteTransaction<TDbContext>(this TDbContext context, Action<TDbContext, IDbConnection, IDbTransaction> action, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
            where TDbContext : DbContext
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            IDbContextTransaction tran = context.Database.BeginTransaction(isolationLevel);
            try
            {
                DbConnection connection = context.Database.GetDbConnection();
                DbTransaction transaction = tran.GetDbTransaction();
                action(context, connection, transaction);
                tran.Commit();
            }
            catch (Exception ex)
            {
                tran?.Rollback();
                throw;
            }
            finally
            {
                tran?.Dispose();
            }
        }

        /// <summary>
        /// 事务执行 - Async
        /// </summary>
        /// <param name="context"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static async Task ExecuteTransactionAsync<TDbContext>(this TDbContext context, Func<Task> func, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
            where TDbContext : DbContext
            => await ExecuteTransactionAsync(context, (context) => func(), isolationLevel);

        /// <summary>
        /// 事务执行 - Async
        /// </summary>
        /// <param name="context"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static async Task ExecuteTransactionAsync<TDbContext>(this TDbContext context, Func<TDbContext, Task> func, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
            where TDbContext : DbContext
            => await ExecuteTransactionAsync(context, (context, connection, transaction) => func(context), isolationLevel);

        /// <summary>
        /// 事务执行 - Async
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static async Task ExecuteTransactionAsync<TDbContext>(this TDbContext context, Func<TDbContext, IDbConnection, IDbTransaction, Task> func, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
            where TDbContext : DbContext
        {
            if (func == null) throw new ArgumentNullException(nameof(func));

            IDbContextTransaction tran = await context.Database.BeginTransactionAsync(isolationLevel);
            try
            {
                DbConnection connection = context.Database.GetDbConnection();
                DbTransaction transaction = tran.GetDbTransaction();
                await func(context, connection, transaction);
                await tran.CommitAsync();
            }
            catch (Exception ex)
            {
                await tran?.RollbackAsync();
                throw;
            }
            finally
            {
                tran?.Dispose();
            }

        }
        #endregion

        #region Update
        /// <summary>
        /// 更新操作 - 可部分更新
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="context">EFCore 上下文</param>
        /// <param name="primaryKey">实体主键</param>
        /// <param name="parameters">更新参数，属性名称/或键名应和实体属性名称一致，支持对象、匿名类、Dictionary<string, object></param>
        /// <returns>实体的跟踪状态 EntityEntry</returns>
        public static EntityEntry Update<TEntity>(this DbContext context, object primaryKey, object parameters)
            where TEntity:class
        {
            TEntity entity = context.Find<TEntity>(primaryKey);
            return Update(context, entity, parameters);
        }

        /// <summary>
        /// 更新操作 - 可部分更新
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="context">EFCore 上下文</param>
        /// <param name="predicate">单个实体筛选表达式</param>
        /// <param name="parameters">更新参数，属性名称/或键名应和实体属性名称一致，支持对象、匿名类、Dictionary<string, object></param>
        /// <returns>实体的跟踪状态 EntityEntry</returns>
        public static EntityEntry Update<TEntity>(this DbContext context, Expression<Func<TEntity, bool>> predicate, object parameters)
            where TEntity : class
        {
            TEntity entity = context.Set<TEntity>().SingleOrDefault(predicate);
            return Update(context, entity, parameters);
        }

        static EntityEntry Update<TEntity>(DbContext context, TEntity entity, object parameters)
            where TEntity:class
        {
            EntityEntry entry = context.Entry(entity);
            entry.CurrentValues.SetValues(parameters);
            return entry;
        }
        #endregion

        #region Remove
        /// <summary>
        /// 主键移除操作
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TKey">实体主键类型</typeparam>
        /// <param name="context">EFCore 上下文</param>
        /// <param name="primaryKey">实体主键</param>
        /// <returns></returns>
        public static EntityEntry Remove<TEntity, TKey>(this DbContext context, TKey primaryKey)
            where TEntity : class
        {
            TEntity entity = context.Find<TEntity>(primaryKey);
            EntityEntry entry = context.Remove(entity);
            return entry;
        }

        /// <summary>
        /// 条件移除操作
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="context">EFCore 上下文</param>
        /// <param name="predicate">条件筛选表达式</param>
        public static void RemoveRange<TEntity>(this DbContext context, Expression<Func<TEntity, bool>> predicate)
            where TEntity : class
        {
            List<TEntity> entities = context.Set<TEntity>().Where(predicate).ToList();
            context.RemoveRange(entities);
        }
        #endregion
    }
}
