using Microsoft.AspNetCore.Authorization;
using NewHabr.Domain.Contracts.Services;
using NewHabr.WebApi.Authorization.Requirements;
using NewHabr.WebApi.Extensions;

namespace NewHabr.WebApi.Authorization.Handlers;

public class IsCommentOwnerHandler : IAuthorizationHandler
{
    private readonly ICommentService _commentService;


    public IsCommentOwnerHandler(ICommentService commentService)
    {
        _commentService = commentService;
    }

    public async Task HandleAsync(AuthorizationHandlerContext context)
    {
        if (context.Resource is Guid commentId)
        {
            foreach (var requirement in context.PendingRequirements)
            {
                switch (requirement)
                {
                    case AllowedDeleteCommentRequirement:
                    case AllowedManageCommentRequirement:
                        {
                            bool isAuthor = await _commentService
                                .IsAuthor(commentId, context.User.GetUserId(), default);
                            if (isAuthor)
                            {
                                context.Succeed(requirement);
                            }
                            break;
                        }
                }
            }
        }
    }
}