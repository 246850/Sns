using Calamus.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sns.Domains.Entities
{
    public partial class SnsdbContext : DbContext
    {
        protected DateTime Timestamp => DateTime.Now;
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
            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        foreach(PropertyEntry propertyEntry in entry.Properties)
                        {
                            if (propertyEntry.Metadata.Name == EntityFieldDefaults.CreateTime)
                            {
                                propertyEntry.CurrentValue = Timestamp;
                                continue;
                            }
                            if (propertyEntry.Metadata.Name == EntityFieldDefaults.LastUpdateTime)
                            {
                                propertyEntry.CurrentValue = Timestamp;
                                continue;
                            }
                        }
                        //if (entry.Properties.Any(p => p.Metadata.Name == EntityFieldDefaults.CreateTime))
                        //{
                        //    entry.CurrentValues[EntityFieldDefaults.CreateTime] = now;
                        //}
                        //if (entry.Properties.Any(p => p.Metadata.Name == EntityFieldDefaults.LastUpdateTime))
                        //{
                        //    entry.CurrentValues[EntityFieldDefaults.LastUpdateTime] = now;
                        //}
                        break;
                    case EntityState.Deleted:
                        if (entry.Properties.Any(p => p.Metadata.Name == EntityFieldDefaults.Deleted))
                        {
                            entry.State = EntityState.Unchanged;
                            entry.CurrentValues[EntityFieldDefaults.Deleted] = true;
                        }
                        break;
                    case EntityState.Modified:
                        foreach (PropertyEntry propertyEntry in entry.Properties)
                        {
                            if(propertyEntry.Metadata.Name == EntityFieldDefaults.LastUpdateTime)
                            {
                                propertyEntry.CurrentValue = Timestamp;
                                continue;
                            }
                            if (propertyEntry.IsModified && propertyEntry.OriginalValue == propertyEntry.CurrentValue)
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
