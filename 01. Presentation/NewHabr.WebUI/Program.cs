using Microsoft.AspNetCore.SpaServices.AngularCli;
using NewHabr.Business.Configurations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Serilog;

namespace NewHabr.WebUI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var services = builder.Services;

        var logPath = builder.Configuration["Log:RestAPIPath"];
        if (!string.IsNullOrEmpty(logPath))
            builder.UseSerilog("ui", logPath);

        services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.UseMemberCasing();
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Unspecified;
                });

        services.AddSpaStaticFiles(configuration =>
        {
            configuration.RootPath = "ClientApp/dist";
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseSpaStaticFiles();
        }

        app.UseStaticFiles();
        app.UseRouting();

        app.UseCors(
               options => options.SetIsOriginAllowed(x => _ = true).AllowAnyMethod().AllowAnyHeader().AllowCredentials()
           );

        app.UseSpa(spa =>
        {
            // To learn more about options for serving an Angular SPA from ASP.NET Core,
            // see https://go.microsoft.com/fwlink/?linkid=864501

            spa.Options.SourcePath = "ClientApp";

            if (app.Environment.IsDevelopment())
            {
                spa.UseAngularCliServer(npmScript: "start");
            }
        });

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller}/{action=Index}/{id?}");

        app.MapFallbackToFile("index.html");

        app.Run();
    }
}
