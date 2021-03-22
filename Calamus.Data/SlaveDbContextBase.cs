using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Calamus.Data
{
    /// <summary>
    /// 从库EF Core 数据库上下文
    /// </summary>
    public abstract class SlaveDbContextBase : DbContext
    {
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            throw new InvalidOperationException("从库不允许执行保存操作!");
        }
        public override int SaveChanges()
        {
            return SaveChanges(false);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            throw new InvalidOperationException("从库不允许执行保存操作!");
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return SaveChangesAsync(false, cancellationToken);
        }
    }
}
