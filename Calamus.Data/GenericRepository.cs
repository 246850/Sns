using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Calamus.Data
{
    public class GenericRepository<TEntity, TKey> : IRepository<TEntity, TKey>, IDisposable where TEntity : EntityBase<TKey>, new()
    {
        public DbContext Context { get; private set; }
        protected DbSet<TEntity> Entities => Context.Set<TEntity>();

        public IQueryable<TEntity> Table => Entities;

        public IQueryable<TEntity> TableAsNoTracking => Entities.AsNoTracking();

        public GenericRepository(DbContext context)
        {
            Context = context;
        }

        public TEntity GetByKey(TKey id)
        {
            TEntity entity = Entities.Find(id);
            return entity;
        }

        public List<TEntity> GetByKey(IEnumerable<TKey> ids)
        {
            List<TEntity> entities = Entities.Where(x => ids.Contains(x.Id)).ToList();
            return entities;
        }

        public int Insert(TEntity entity)
        {
            Entities.Add(entity);
            int row = Context.SaveChanges();
            return row;
        }

        public int Insert(IEnumerable<TEntity> entities)
        {
            Entities.AddRange(entities);
            int row = Context.SaveChanges();
            return row;
        }

        public int Update(TEntity entity)
        {
            if (!Entities.Local.Contains(entity)) Context.Entry(entity).State = EntityState.Modified;
            else if (Context.Entry(entity).State != EntityState.Modified) Context.Entry(entity).State = EntityState.Modified;
            return Context.SaveChanges();
        }

        public int Update(IEnumerable<TEntity> entities)
        {
            Entities.UpdateRange(entities);
            int row = Context.SaveChanges();
            return row;
        }

        public int Delete(TKey id)
        {
            TEntity entity = GetByKey(id);
            Entities.Remove(entity);
            int row = Context.SaveChanges();
            return row;
        }

        public int Delete(IEnumerable<TKey> ids)
        {
            List<TEntity> entities = GetByKey(ids);
            Entities.RemoveRange(entities);
            int row = Context.SaveChanges();
            return row;
        }

        public int Delete(Expression<Func<TEntity, bool>> predicate)
        {
            List<TEntity> entities = Entities.Where(predicate).ToList();
            Entities.RemoveRange(entities);
            int row = Context.SaveChanges();
            return row;
        }

        public bool ExecuteTransaction(Action func)
        {
            IDbContextTransaction transaction = null;
            try
            {
                transaction = Context.Database.BeginTransaction();
                func.Invoke();

                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                transaction?.Rollback();
                throw;
            }
            finally
            {
                transaction?.Dispose();
            }
        }

        public async Task<TEntity> GetByKeyAsync(TKey id)
        {
            TEntity entity = await Entities.FindAsync(id);
            return entity;
        }

        public async Task<List<TEntity>> GetByKeyAsync(IEnumerable<TKey> ids)
        {
            List<TEntity> entities = await Entities.Where(x => ids.Contains(x.Id)).ToListAsync();
            return entities;
        }

        public async Task<int> InsertAsync(TEntity entity)
        {
            await Entities.AddAsync(entity);
            int row = await Context.SaveChangesAsync();
            return row;
        }

        public async Task<int> InsertAsync(IEnumerable<TEntity> entities)
        {
            await Entities.AddRangeAsync(entities);
            int row = await Context.SaveChangesAsync();
            return row;
        }

        public Task<int> UpdateAsync(TEntity entity)
        {
            if (!Entities.Local.Contains(entity)) Context.Entry(entity).State = EntityState.Modified;
            else if (Context.Entry(entity).State != EntityState.Modified) Context.Entry(entity).State = EntityState.Modified;
            return Context.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(IEnumerable<TEntity> entities)
        {
            Entities.UpdateRange(entities);
            int row = await Context.SaveChangesAsync();
            return row;
        }

        public async Task<int> DeleteAsync(TKey id)
        {
            TEntity entity = await GetByKeyAsync(id);
            Entities.Remove(entity);
            int row = await Context.SaveChangesAsync();
            return row;
        }

        public async Task<int> DeleteAsync(IEnumerable<TKey> ids)
        {
            List<TEntity> entities = await GetByKeyAsync(ids);
            Entities.RemoveRange(entities);
            int row = await Context.SaveChangesAsync();
            return row;
        }

        public async Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            List<TEntity> entities = await Entities.Where(predicate).ToListAsync();
            Entities.RemoveRange(entities);
            int row = await Context.SaveChangesAsync();
            return row;
        }

        public async Task<bool> ExecuteTransactionAsync(Action func)
        {
            IDbContextTransaction transaction = null;
            try
            {
                transaction = await Context.Database.BeginTransactionAsync();
                func.Invoke();

                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                transaction?.Rollback();
                throw;
            }
            finally
            {
                transaction?.Dispose();
            }
        }

        public void Dispose()
        {
            Context?.Dispose();
            if (Context != null) Context = null;
        }
    }
}
