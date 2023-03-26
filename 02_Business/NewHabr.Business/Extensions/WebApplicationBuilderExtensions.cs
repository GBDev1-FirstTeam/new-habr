using Microsoft.AspNetCore.Builder;
using Serilog;
using Serilog.Events;

namespace NewHabr.Business.Configurations;

public static class WebApplicationBuilderExtensions
{
    public static void UseSerilog(this WebApplicationBuilder builder, string name, string path)
    {
        if (string.IsNullOrEmpty(path))
            return;

        builder.Host.UseSerilog((ctx, lc) => lc
        .MinimumLevel.Debug()
        .MinimumLevel.Override("System", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
        .WriteTo.Console()
        .WriteTo.File($"{path}/{name}.log",
                        rollingInterval: RollingInterval.Day,
                        rollOnFileSizeLimit: true,
                        fileSizeLimitBytes: 536870912,
                        retainedFileCountLimit: 80,
                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
        );
    }
}
