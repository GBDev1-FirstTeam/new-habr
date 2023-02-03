using System.Reflection;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NewHabr.DAL.EF;
using NewHabr.Domain;

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

    public static void ConfigureAutoMapper(this IServiceCollection services, Assembly profilesAssembly)
    {
        var mapperConfigurations = new MapperConfiguration(config => config.AddMaps(profilesAssembly.GetType().Assembly));
        var mapper = mapperConfigurations.CreateMapper();
        services.AddSingleton(mapper);
    }
}

