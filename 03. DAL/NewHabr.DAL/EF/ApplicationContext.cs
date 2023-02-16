#nullable disable
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NewHabr.Domain.Models;

namespace NewHabr.DAL.EF;

public class ApplicationContext : IdentityDbContext<User, UserRole, Guid>
{
    public DbSet<Article> Articles { get; set; }
    public DbSet<Notification> UserNotifications { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<SecureQuestion> SecureQuestions { get; set; }
    public DbSet<LikedUser> LikedUsers { get; set; }
    public DbSet<LikedComment> LikedComments { get; set; }
    public DbSet<LikedArticle> LikedArticles { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Category> Categories { get; set; }


    public ApplicationContext(DbContextOptions options) : base(options)
    {
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
