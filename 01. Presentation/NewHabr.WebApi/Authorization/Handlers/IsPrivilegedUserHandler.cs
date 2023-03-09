using Microsoft.AspNetCore.Authorization;
using NewHabr.WebApi.Authorization.Requirements;

namespace NewHabr.WebApi.Authorization.Handlers;

public class IsPrivilegedUserHandler : AuthorizationHandler<AllowedDeleteCommentRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AllowedDeleteCommentRequirement requirement)
    {
        bool isPrivileged = context.User.IsInRole("Moderator") || context.User.IsInRole("Administrator");
        if (isPrivileged)
            context.Succeed(requirement);
        return Task.CompletedTask;
    }
}
