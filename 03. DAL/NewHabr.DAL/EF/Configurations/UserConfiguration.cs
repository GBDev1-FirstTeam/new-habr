using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewHabr.Domain.Models;

namespace NewHabr.DAL.EF.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .HasMany<Comment>(u => u.LikedComments)
            .WithMany(c => c.Likes)
            .UsingEntity(join => join.ToTable("UserLikesComment"));

        builder
            .HasMany<User>(u => u.LikedUsers)
            .WithMany(author => author.ReceivedLikes)
            .UsingEntity(join => join.ToTable("UserLikesAuthor"));
    }
}

