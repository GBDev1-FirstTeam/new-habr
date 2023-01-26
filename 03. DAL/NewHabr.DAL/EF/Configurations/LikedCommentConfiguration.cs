using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewHabr.Domain.Models;

namespace NewHabr.DAL.EF.Configurations;

public class LikedCommentConfiguration : IEntityTypeConfiguration<LikedComment>
{
    public void Configure(EntityTypeBuilder<LikedComment> builder)
    {
        builder.HasKey(m => new { m.UserId, m.CommentId });
    }
}
