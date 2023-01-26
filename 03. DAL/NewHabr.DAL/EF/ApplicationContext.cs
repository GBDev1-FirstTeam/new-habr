using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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

    private IConfiguration _configuration;

    private IConfiguration Configuration => _configuration ??= ApplicationContextHelper.GetConfiguration();

    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        // Для миграции, если connectionString не был ранее использован из настроек, то берем закодированнй жестко в классе
        if (!optionsBuilder.IsConfigured)
        {
            ApplicationContextHelper.ConfigureDbContextOptions(optionsBuilder, Configuration);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        //modelBuilder.ApplyConfiguration(new ClientConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}
