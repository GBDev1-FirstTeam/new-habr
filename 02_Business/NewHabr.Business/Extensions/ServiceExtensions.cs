using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;

namespace NewHabr.WebApi.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureControllers(this IServiceCollection services)
    {
        services.AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.UseMemberCasing();
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Unspecified;
            });
    }
}
