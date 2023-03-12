using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewHabr.Domain.Contracts.Services;
using NewHabr.Domain.Dto;
using NewHabr.Domain.Exceptions;
using NewHabr.WebApi.Extensions;

namespace NewHabr.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommentsController : ControllerBase
{
    private readonly ICommentService _commentService;
    private readonly IAuthorizationService _authorizationService;
    private readonly ILogger<CommentsController> _logger;

    public CommentsController(ICommentService commentService, IAuthorizationService authorizationService, ILogger<CommentsController> logger)
    {
        _commentService = commentService;
        _authorizationService = authorizationService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<List<CommentDto>>> GetAll(CancellationToken cancellationToken)
    {
        return Ok(await _commentService.GetAllAsync(cancellationToken));
    }

    [HttpPost]
    [Authorize(Policy = "CanCreate")]
    public async Task<ActionResult> Create([FromBody] CommentCreateRequest newComment, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        await _commentService.CreateAsync(userId, newComment, cancellationToken);
        return Ok();
    }

    [HttpPut("{commentId}")]
    [Authorize]
    public async Task<ActionResult> Update([FromRoute] Guid commentId, [FromBody] CommentUpdateRequest updateComment, CancellationToken cancellationToken)
    {
        var authResult = await _authorizationService
            .AuthorizeAsync(User, commentId, "CanManageComment");
        if (!authResult.Succeeded)
            return new ForbidResult();

        await _commentService.UpdateAsync(commentId, updateComment, cancellationToken);
        return Ok();

    }

    [HttpDelete("{commentId}")]
    [Authorize]
    public async Task<ActionResult> Delete([FromRoute] Guid commentId, CancellationToken cancellationToken)
    {
        var authResult = await _authorizationService
            .AuthorizeAsync(User, commentId, "CanDeleteComment");
        if (!authResult.Succeeded)
            return new ForbidResult();

        await _commentService.DeleteAsync(commentId, cancellationToken);
        return Ok();

    }

    [Authorize(Policy = "CanLike")]
    [HttpPut("{commentId}/like")]
    public async Task<IActionResult> SetLike([FromRoute] Guid commentId, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        await _commentService.SetLikeAsync(commentId, userId, cancellationToken);
        return NoContent();

    }

    [Authorize(Policy = "CanLike")]
    [HttpPut("{commentId}/unlike")]
    public async Task<IActionResult> UnsetLike([FromRoute] Guid commentId, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        await _commentService.UnsetLikeAsync(commentId, userId, cancellationToken);
        return NoContent();

    }
}
