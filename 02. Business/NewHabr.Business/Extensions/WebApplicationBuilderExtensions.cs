using Serilog.Events;
using Serilog;
using Microsoft.AspNetCore.Builder;

namespace NewHabr.Business.Configurations;

public static class WebApplicationBuilderExtensions
{
    public static void UseSerilog(this WebApplicationBuilder builder, string name, string path)
    {
        builder.Host.UseSerilog((ctx, lc) => lc
            .MinimumLevel.Debug()
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
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
