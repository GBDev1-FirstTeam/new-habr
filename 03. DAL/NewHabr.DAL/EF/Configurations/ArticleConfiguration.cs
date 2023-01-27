using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NewHabr.Domain.Models;

namespace NewHabr.DAL.EF.Configurations;

public class ArticleConfiguration : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder
            .Property(m => m.ApproveState)
            .HasConversion(new EnumToStringConverter<ApproveState>());
    }
}
