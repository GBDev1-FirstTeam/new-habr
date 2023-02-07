﻿using Microsoft.EntityFrameworkCore;
using NewHabr.Business.Configurations;
using NewHabr.Business.AutoMapperProfiles;
using NewHabr.DAL.EF;
using NewHabr.WebApi.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Serilog;
using NewHabr.Domain.Contracts.Services;
using NewHabr.Business.Services;
using NewHabr.DAL.Repository;
using NewHabr.Domain.Contracts;

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

        services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.UseMemberCasing();
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Unspecified;
                });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.ConfigureAutoMapper(typeof(ArticleProfile).Assembly);

        services.AddScoped<IRepositoryManager, RepositoryManager>();
        services.AddScoped<ICommentService, CommentService>();

        var app = builder.Build();
        UpdateDatabase(app);

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        //app.UseAuthorization();

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
