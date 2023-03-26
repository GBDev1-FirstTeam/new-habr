using System;
using System.Net.Mime;
using Microsoft.AspNetCore.Diagnostics;
using NewHabr.Domain.Exceptions;

namespace NewHabr.WebApi.Extensions;

public static class WebApplicationExtensions
{
    public static void ConfigureExceptionHandler(this WebApplication app, ILogger logger)
    {
        app.UseExceptionHandler(appBuilder =>
        {
            appBuilder.Run(async context =>
            {
                context.Response.ContentType = "application/json";

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature is not null)
                {
                    context.Response.StatusCode = contextFeature.Error switch
                    {
                        NotFoundException => StatusCodes.Status404NotFound,
                        AlreadyExistsException => StatusCodes.Status400BadRequest,
                        ArticleNotApprovedException => StatusCodes.Status400BadRequest,
                        UnauthorizedAccessException => StatusCodes.Status403Forbidden,
                        InvalidOperationException => StatusCodes.Status400BadRequest,
                        _ => StatusCodes.Status500InternalServerError
                    };

                    logger.LogError($"Something went wrong: {contextFeature.Error}");
                    logger.LogError("Request Method: {method}; Path: {path}; Query: {query}", context.Request.Method, context.Request.Path, context.Request.QueryString.Value);
                    await context.Response.WriteAsJsonAsync(
                        new
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = contextFeature.Error.Message
                        });
                }
            });
        });
    }
}

