﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NewHabr.Business.AutoMapperProfiles;
using NewHabr.Business.Services;
using NewHabr.DAL.EF;
using NewHabr.DAL.Repository;
using NewHabr.Domain.Contracts;
using NewHabr.WebApi.Extensions;

namespace NewHabr.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var services = builder.Services;

        services.ConfigureDbContext(builder.Configuration);

        var mapperConfigurations = new MapperConfiguration(mp => mp.AddProfile<ArticleProfile>());
        var mapper = mapperConfigurations.CreateMapper();
        services.AddSingleton(mapper);

        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddScoped<IRepositoryManager, RepositoryManager>();
        services.AddTransient<IArticleService, ArticleService>();

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
