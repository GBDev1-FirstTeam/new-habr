using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NewHabr.Business.AutoMapperProfiles;
using NewHabr.Business.Configurations;
using NewHabr.Business.Services;
using NewHabr.DAL.EF;
using NewHabr.DAL.Repository;
using NewHabr.Domain.ConfigurationModels;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Contracts.Services;
using NewHabr.WebApi.Extensions;
using Serilog;

namespace NewHabr.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var services = builder.Services;

        var logPath = builder.Configuration["Log:RestAPIPath"];
        if (!string.IsNullOrEmpty(logPath))
            builder.UseSerilog("api", logPath);

        services.ConfigureDbContext(builder.Configuration);

        #region Configure Identity

        services.AddAuthentication();
        services.ConfigureIdentity();

        #endregion

        #region Configure Jwt

        services.Configure<JwtConfiguration>(builder.Configuration.GetSection(JwtConfiguration.Section));
        services.ConfigureJWT(builder.Configuration);

        #endregion

        #region Register services in DI

        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IRepositoryManager, RepositoryManager>();
        services.AddScoped<IArticleService, ArticleService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ITagService, TagService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<ISecureQuestionsService, SecureQuestionsService>();
        services.AddScoped<IRepositoryManager, RepositoryManager>();

        #endregion

        #region Configure Controllers

        services.ConfigureControllers();

        #endregion

        services.AddEndpointsApiExplorer();

        #region Configure Swagger

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "NewHabr", Version = "v1" });
            c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme()
            {
                Description = "JWT Authorization header using the Bearer scheme(Example: 'Bearer 1234abcdef')",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = JwtBearerDefaults.AuthenticationScheme
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme()
                    {
                        Reference = new OpenApiReference()
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        }
                    },Array.Empty<string>()
                }
            });
        });

        #endregion

        services.ConfigureAutoMapper(typeof(ArticleProfile).Assembly);

        var app = builder.Build();
        UpdateDatabase(app);

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }

    private static void UpdateDatabase(IApplicationBuilder app)
    {
        try
        {
            Console.WriteLine("Try update database");
            using var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
            using var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationContext>();
            context.Database.Migrate();
            Console.WriteLine("Update database successful");
        }
        catch (Exception)
        {
            Console.WriteLine("Update database failure");
            throw;
        }
    }
}
