using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Calamus.Ioc;

namespace Calamus.Data
{
    /// <summary>
    /// EF Core 数据库上下文
    /// </summary>
    public class DefaultDbContext : DbContext
    {
        public DefaultDbContext(DbContextOptions<DefaultDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ITypeFinder typeFinder = new WebAppTypeFinder();
            IList<Assembly> assemblies = typeFinder.GetAssemblies();

            foreach (Assembly assembly in assemblies)
            {
                IEnumerable<Type> mapTypes = assembly.GetTypes().Where(type => !string.IsNullOrWhiteSpace(type.Namespace) && type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(DefaultEntityTypeConfiguration<>));
                foreach (Type type in mapTypes)
                {
                    dynamic configurationInstance = Activator.CreateInstance(type);
                    modelBuilder.ApplyConfiguration(configurationInstance);
                }
            }

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            EntryCheck();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            EntryCheck();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            EntryCheck();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            EntryCheck();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        void EntryCheck()
        {
            var entries = ChangeTracker.Entries();
            DateTime now = DateTime.Now;
            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        if (entry.Entity is IHasCreateTime)
                        {
                            ((IHasCreateTime)entry.Entity).CreateTime = now;
                        }
                        if (entry.Entity is IHasLastUpdateTime)
                        {
                            ((IHasLastUpdateTime)entry.Entity).LastUpdateTime = now;
                        }
                        break;
                    case EntityState.Deleted:
                        if (entry.Entity is ICanDeletable)
                        {
                            ((ICanDeletable)entry.Entity).Deleted = true;
                            entry.State = EntityState.Unchanged;
                        }
                        //else if (entry.Properties.Any(p => p.Metadata.Name == EntityFieldConstant.Deleted))
                        //{
                        //    entry.State = EntityState.Unchanged;
                        //    entry.CurrentValues[EntityFieldConstant.Deleted] = true;
                        //}
                        break;
                    case EntityState.Modified:
                        if (entry.Entity is IHasLastUpdateTime)
                        {
                            ((IHasLastUpdateTime)entry.Entity).LastUpdateTime = now;
                        }

                        foreach (PropertyEntry propertyEntry in entry.Properties)
                        {
                            if (propertyEntry.OriginalValue == propertyEntry.CurrentValue)
                            {
                                propertyEntry.IsModified = false;   // 只更新部分列
                            }
                        }
                        break;
                }
            }
        }
    }
}
