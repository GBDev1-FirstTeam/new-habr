using Microsoft.EntityFrameworkCore;
using NewHabr.Domain;
using NewHabr.DAL.EF;
using Microsoft.AspNetCore.Identity;
using NewHabr.Domain.Models;

namespace NewHabr.WebApi.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var dbProvider = configuration.GetValue<string>(Constants.SQLProvider.ConfigurationName, Constants.SQLProvider.PostgreSQL);
        var connectionString = configuration.GetConnectionString(dbProvider);

        switch (dbProvider.ToLower())
        {
            case Constants.SQLProvider.PostgreSQL:
                services.AddDbContext<ApplicationContext>(options =>
                    options.UseNpgsql(connectionString, options => options.MigrationsAssembly("NewHabr.PostgreSQL")));
                break;
            case Constants.SQLProvider.MSSQL:
                services.AddDbContext<ApplicationContext>(options =>
                    options.UseSqlServer(connectionString, options => options.MigrationsAssembly("NewHabr.MSSQL")));
                break;
        }
    }

    public static void ConfigureIdentity(this IServiceCollection services)
    {
        services.AddIdentity<User, UserRole>(setupAction =>
        {
            setupAction.Password.RequiredLength = 5;
            setupAction.Password.RequireDigit = false;
            setupAction.Password.RequireLowercase = false;
            setupAction.Password.RequireUppercase = false;
            setupAction.Password.RequireNonAlphanumeric = false;
        })
        .AddEntityFrameworkStores<ApplicationContext>()
        .AddDefaultTokenProviders();
    }
}

