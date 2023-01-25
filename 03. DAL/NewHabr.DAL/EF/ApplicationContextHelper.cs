using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NewHabr.Domain;

namespace NewHabr.DAL.EF;

public static class ApplicationContextHelper
{
    public static IConfiguration GetConfiguration()
    {
        var builder = new ConfigurationBuilder();
        builder.SetBasePath(Directory.GetCurrentDirectory());
        builder.AddJsonFile(Constants.AppSettingsJSON, true);

        var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        if (File.Exists($"appsettings.{environmentName}.json"))
            builder.AddJsonFile($"appsettings.{environmentName}.json", true);

        builder.AddEnvironmentVariables();
        return builder.Build();
    }

    public static string GetProvider(IConfiguration configuration) => configuration[Constants.SQLProvider.ConfigurationName];

    public static string GetConnectionString(IConfiguration configuration, string provider)
    {
        if (configuration == null) return string.Empty;

        var connectionString = configuration.GetConnectionString(provider);
        var connectionBuilder = new SqlConnectionStringBuilder(connectionString);
        return connectionBuilder.ToString();
    }

    public static DbContextOptionsBuilder ConfigureDbContextOptions(DbContextOptionsBuilder optionsBuilder, IConfiguration configuration)
    {
        var provider = GetProvider(configuration);
        var connectionString = GetConnectionString(configuration, provider);
        return ConfigureDbContextOptions(optionsBuilder, connectionString, provider);
    }

    public static DbContextOptionsBuilder ConfigureDbContextOptions(DbContextOptionsBuilder optionsBuilder, string connectionString, string provider)
    {
        Console.WriteLine("ApplicationContextHelper.ConfigureDbContextOptions start");
        Console.WriteLine($"SQL Provider = {provider}");
        Console.WriteLine($"Connection string = {connectionString}");

        switch (provider?.ToLower())
        {
            case Constants.SQLProvider.PostgreSQL:
                Console.WriteLine("Cofigure PostgreSQL Provider");
                optionsBuilder.UseNpgsql(connectionString, bo => bo.MigrationsAssembly("NewHabr.PostgreSQL"));
                break;
            case Constants.SQLProvider.MSSQL:
            default:
                Console.WriteLine("Cofigure MSSQL Provider");
                optionsBuilder.UseSqlServer(connectionString, bo => bo.MigrationsAssembly("NewHabr.MSSQL"));
                break;
        }
        Console.WriteLine("ApplicationContextHelper.ConfigureDbContextOptions stop");
        return optionsBuilder;
    }
}
