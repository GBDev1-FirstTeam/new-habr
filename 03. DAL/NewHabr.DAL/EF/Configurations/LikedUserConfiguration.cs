using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewHabr.Domain.Models;

namespace NewHabr.DAL.EF.Configurations;

public class LikedUserConfiguration : IEntityTypeConfiguration<LikedUser>
{
    public void Configure(EntityTypeBuilder<LikedUser> builder)
    {
        builder.HasKey(m => new { m.UserId, m.AuthorId });

        builder
            .HasOne<User>(lu => lu.User)
            .WithMany(user => user.LikedUsers)
            .HasForeignKey(k => k.UserId);

        builder
            .HasOne<User>(lu => lu.Author)
            .WithMany(user => user.ReceivedLikes)
            .HasForeignKey(k => k.AuthorId);
    }
}
