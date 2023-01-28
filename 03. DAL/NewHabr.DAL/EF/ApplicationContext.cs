#nullable disable
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using NewHabr.Domain.Models;

namespace NewHabr.DAL.EF;

public class ApplicationContext : DbContext
{
    public DbSet<Article> Articles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserNotification> UserNotifications { get; set; }
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
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}
