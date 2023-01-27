using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewHabr.Domain.Models;

namespace NewHabr.DAL.EF.Configurations;

public class LikedCommentConfiguration : IEntityTypeConfiguration<LikedComment>
{
    public void Configure(EntityTypeBuilder<LikedComment> builder)
    {
        builder.HasKey(m => new { m.UserId, m.CommentId });


        builder
            .HasOne(lu => lu.User)
            .WithMany(user => user.LikedComments)
            .HasForeignKey(k => k.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(lu => lu.Comment)
            .WithMany(article => article.Likes)
            .HasForeignKey(k => k.CommentId);
    }
}
