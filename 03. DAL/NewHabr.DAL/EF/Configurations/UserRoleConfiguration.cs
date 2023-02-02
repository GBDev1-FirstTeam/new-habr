using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewHabr.Domain.Models;

namespace NewHabr.DAL.EF.Configurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.HasData(
            new UserRole
            {
                Id = Guid.Parse("00a98c8e-6a15-4447-9343-063f4f1efefc"),
                Name = "User",
                NormalizedName = "USER"
            },
            new UserRole
            {
                Id = Guid.Parse("1bfc496b-ebd2-4c5a-b3e8-4b2c1e334391"),
                Name = "Moderator",
                NormalizedName = "MODERATOR"
            },
            new UserRole
            {
                Id = Guid.Parse("aec1eede-5f3f-43ba-9ec3-454a3002c013"),
                Name = "Administrator",
                NormalizedName = "ADMINISTRATOR"
            }
        ); ;
    }
}
