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

        builder
            .HasOne<User>(a => a.User)
            .WithMany(u => u.AuthoredArticles)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasMany<User>(a => a.Likes)
            .WithMany(u => u.LikedArticles)
            .UsingEntity(join => join.ToTable("UserLikesArticle"));
    }
}
