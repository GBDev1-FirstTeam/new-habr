using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewHabr.Domain.Models;

namespace NewHabr.DAL.EF.Configurations;

public class LikedArticleConfiguration : IEntityTypeConfiguration<LikedArticle>
{
    public void Configure(EntityTypeBuilder<LikedArticle> builder)
    {
        builder.HasKey(m => new { m.UserId, m.ArticleId });
    }
}
