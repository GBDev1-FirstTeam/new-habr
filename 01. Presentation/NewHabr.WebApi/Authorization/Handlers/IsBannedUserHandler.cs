using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using NewHabr.Domain.Contracts.Services;
using NewHabr.Domain.Models;
using NewHabr.WebApi.Authorization.Requirements;
using NewHabr.WebApi.Extensions;

namespace NewHabr.WebApi.Authorization.Handlers;

public class IsBannedUserHandler : IAuthorizationHandler
{
    private readonly IUserService _userService;


    public IsBannedUserHandler(IUserService userService)
    {
        _userService = userService;
    }


    public async Task HandleAsync(AuthorizationHandlerContext context)
    {
        foreach (var requirement in context.PendingRequirements)
        {
            switch (requirement)
            {
                case AllowedManageArticleRequirement:
                case AllowedManageCommentRequirement:
                case AllowedSetLikeRequirement:
                case AllowedCreateRequirement:
                    {
                        var banned = await _userService.IsBanned(context.User.GetUserId(), default);
                        if (banned)
                        {
                            context.Fail();
                        }

                        break;
                    }
            }
        }
    }
}
