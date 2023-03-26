using System;
using Microsoft.AspNetCore.Authorization;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Models;
using NewHabr.WebApi.Authorization.Requirements;
using NewHabr.WebApi.Extensions;

namespace NewHabr.WebApi.Authorization.Handlers;

public class IsArticleOwnerHandler : AuthorizationHandler<AllowedManageArticleRequirement, Guid>
{
    private readonly IArticleService _articleService;


    public IsArticleOwnerHandler(IArticleService articleService)
    {
        _articleService = articleService;
    }


    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AllowedManageArticleRequirement requirement, Guid resource)
    {
        bool isAuthor = await _articleService
            .IsAuthor(resource, context.User.GetUserId(), default);

        if (isAuthor)
        {
            context.Succeed(requirement);
        }
        return;
    }

}
