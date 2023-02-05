using System.Reflection;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NewHabr.DAL.EF;
using Microsoft.AspNetCore.Identity;
using NewHabr.Domain.Models;
using NewHabr.Domain.ConfigurationModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
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

    public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtConfiguration = new JwtConfiguration();
        configuration.Bind(JwtConfiguration.Section, jwtConfiguration);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero,

                    ValidIssuer = jwtConfiguration.ValidIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration.Secret))
                };
            });
    }

    public static void ConfigureAutoMapper(this IServiceCollection services, params Assembly[] assembliesToScan)
    {
        var mapperConfigurations = new MapperConfiguration(config => config.AddMaps(assembliesToScan));
        var mapper = mapperConfigurations.CreateMapper();
        services.AddSingleton(mapper);
    }
}
