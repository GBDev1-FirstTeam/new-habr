using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewHabr.Domain.Models;

namespace NewHabr.DAL.EF.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.HasKey(m => m.Id);

        builder
            .HasOne(lu => lu.User)
            .WithMany(user => user.Comments)
            .HasForeignKey(k => k.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(lu => lu.Article)
            .WithMany(article => article.Comments)
            .HasForeignKey(k => k.ArticleId);

        builder.HasQueryFilter(a => !a.Deleted);
    }
}
