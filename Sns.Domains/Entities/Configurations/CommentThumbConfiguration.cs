﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using Sns.Domains.Entities;
using System;

#nullable disable

namespace Sns.Domains.Entities.Configurations
{
    public partial class CommentThumbConfiguration : IEntityTypeConfiguration<CommentThumb>
    {
        public void Configure(EntityTypeBuilder<CommentThumb> entity)
        {
            entity.ToTable("comment_thumb");

            entity.HasComment("评论赞表");

            entity.HasIndex(e => e.CommentId)
                .HasName("IDX_CommentThumb_CommentId");

            entity.HasIndex(e => new { e.AccountId, e.CommentId })
                .HasName("UK_CommentThumb_AccountId_CommentId")
                .IsUnique();

            entity.Property(e => e.AccountId).HasComment("用户ID");

            entity.Property(e => e.CommentId).HasComment("评论ID");

            entity.Property(e => e.CreateTime).HasColumnType("datetime");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<CommentThumb> entity);
    }
}
