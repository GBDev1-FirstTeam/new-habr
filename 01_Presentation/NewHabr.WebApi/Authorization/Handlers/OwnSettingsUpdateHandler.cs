using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using NewHabr.WebApi.Authorization.Requirements;
using NewHabr.WebApi.Extensions;

namespace NewHabr.WebApi.Authorization.Handlers;

public class OwnSettingsUpdateHandler : AuthorizationHandler<AllowedUpdateUserSettingsRequirement, Guid>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AllowedUpdateUserSettingsRequirement requirement, Guid resource)
    {
        if (context.User.GetUserId().Equals(resource))
            context.Succeed(requirement);
        return Task.CompletedTask;
    }
}

