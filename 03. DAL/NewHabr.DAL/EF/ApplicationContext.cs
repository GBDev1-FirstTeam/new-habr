using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace NewHabr.DAL.EF;

public class ApplicationContext : DbContext
{
    //public DbSet<Client> Clients { get; set; }

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
