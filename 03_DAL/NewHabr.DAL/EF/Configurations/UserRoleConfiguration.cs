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
                Id = new Guid("00a98c8e-6a15-4447-9343-063f4f1efefc"),
                Name = UserRoles.User.ToString(),
                NormalizedName = UserRoles.User.ToString().ToUpper().Normalize()
            },
            new UserRole
            {
                Id = new Guid("1bfc496b-ebd2-4c5a-b3e8-4b2c1e334391"),
                Name = UserRoles.Moderator.ToString(),
                NormalizedName = UserRoles.Moderator.ToString().ToUpper().Normalize()
            },
            new UserRole
            {
                Id = new Guid("aec1eede-5f3f-43ba-9ec3-454a3002c013"),
                Name = UserRoles.Administrator.ToString(),
                NormalizedName = UserRoles.Administrator.ToString().ToUpper().Normalize()
            }
        ); ;
    }
}
