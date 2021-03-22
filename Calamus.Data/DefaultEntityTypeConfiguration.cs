using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Calamus.Data
{
    /// <summary>
    /// EF Core Fluent Api 关系映射
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class DefaultEntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : EntityBase, new()
    {
        public abstract void Configure(EntityTypeBuilder<TEntity> builder);
    }
}
