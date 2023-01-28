using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewHabr.Domain.Models;

namespace NewHabr.DAL.EF.Configurations;

public class LikedArticleConfiguration : IEntityTypeConfiguration<LikedArticle>
{
    public void Configure(EntityTypeBuilder<LikedArticle> builder)
    {
        builder.HasKey(m => new { m.UserId, m.ArticleId });


        builder
            .HasOne(lu => lu.User)
            .WithMany(user => user.LikedArticles)
            .HasForeignKey(k => k.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(lu => lu.Article)
            .WithMany(article => article.Likes)
            .HasForeignKey(k => k.ArticleId);
    }
}
