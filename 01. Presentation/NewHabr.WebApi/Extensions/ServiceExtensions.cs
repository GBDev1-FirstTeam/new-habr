using System;
using Microsoft.EntityFrameworkCore;
using NewHabr.DAL.EF;

namespace NewHabr.WebApi.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var dbProvider = configuration.GetValue<string>("SQLServerProvider", "PostgreSQL");
        var connectionString = configuration.GetConnectionString(dbProvider);

        switch (dbProvider)
        {
            case "PostgreSQL":
                services.AddDbContext<ApplicationContext>(options =>
                    options.UseNpgsql(connectionString, options => options.MigrationsAssembly("NewHabr.PostgreSQL")));
                break;
            case "MSSQL":
                services.AddDbContext<ApplicationContext>(options =>
                    options.UseSqlServer(connectionString, options => options.MigrationsAssembly("NewHabr.MSSQL")));
                break;
        }
    }
}

