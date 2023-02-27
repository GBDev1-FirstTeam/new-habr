using System;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NewHabr.Domain.ConfigurationModels;
using NewHabr.Domain.Contracts.Services;

namespace NewHabr.Business.Services;

public class BackgroundUnbanUserService : BackgroundService
{
    private readonly ILogger<BackgroundUnbanUserService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly AppSettings _appSettings;

    public BackgroundUnbanUserService(ILogger<BackgroundUnbanUserService> _logger, IServiceProvider serviceProvider, IOptions<AppSettings> options)
    {
        this._logger = _logger;
        _serviceProvider = serviceProvider;
        _appSettings = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Background service 'BackgroundUnbanUserService starting'");

        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scopedServiceProvider = _serviceProvider.CreateScope())
            {
                var userService = scopedServiceProvider
                    .ServiceProvider
                    .GetRequiredService<IUserService>();

                await userService.AutomaticUnbanUserAsync(stoppingToken);
            }

            await Task.Delay(TimeSpan.FromMinutes(_appSettings.AutoUnBanJobRunsEveryXMinutes));
        }

        _logger.LogInformation("Background service 'BackgroundUnbanUserService stopping'");
    }
}

