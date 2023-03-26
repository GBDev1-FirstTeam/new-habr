using Microsoft.AspNetCore.Authorization;
using NewHabr.WebApi.Authorization.Requirements;

namespace NewHabr.WebApi.Authorization.Handlers;

public class IsAuthorizedHandler : IAuthorizationHandler
{
    public Task HandleAsync(AuthorizationHandlerContext context)
    {
        foreach (var requirement in context.PendingRequirements)
        {
            switch (requirement)
            {
                case AllowedSetLikeRequirement:
                case AllowedCreateRequirement:
                    {
                        if (context.User.Identity?.IsAuthenticated == true)
                        {
                            context.Succeed(requirement);
                        }
                        break;
                    }
            }
        }
        return Task.CompletedTask;
    }
}
