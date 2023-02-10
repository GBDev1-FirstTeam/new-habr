using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewHabr.Domain.Models;

namespace NewHabr.DAL.EF.Configurations;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        //builder
        //    .HasIndex(e => e.Name);
    }
}
