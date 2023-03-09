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
        try
        {
            return Ok(await _commentService.GetAllAsync(cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost]
    [Authorize(Policy = "CanCreate")]
    public async Task<ActionResult> Create([FromBody] CommentCreateRequest newComment, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        try
        {
            await _commentService.CreateAsync(userId, newComment, cancellationToken);
            return Ok();
        }
        catch (UserBannedException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, string.Concat(ex.Message, "\nuser id: {id}:"), userId);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPut("{commentId}")]
    [Authorize]
    public async Task<ActionResult> Update([FromRoute] Guid commentId, [FromBody] CommentUpdateRequest updateComment, CancellationToken cancellationToken)
    {
        var authResult = await _authorizationService
            .AuthorizeAsync(User, commentId, "CanManageComment");
        if (!authResult.Succeeded)
            return new ForbidResult();

        try
        {
            await _commentService.UpdateAsync(commentId, updateComment, cancellationToken);
            return Ok();
        }
        catch (CommentNotFoundException ex)
        {
            _logger.LogInformation(ex, string.Concat(ex.Message, "\ncommentId: {commentId}:"), commentId);
            return NotFound(ex.Message);
        }
        catch (UserBannedException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, string.Concat(ex.Message, "\ncommentId: {commentId}:"), commentId);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpDelete("{commentId}")]
    [Authorize]
    public async Task<ActionResult> Delete([FromRoute] Guid commentId, CancellationToken cancellationToken)
    {
        var authResult = await _authorizationService
            .AuthorizeAsync(User, commentId, "CanDeleteComment");
        if (!authResult.Succeeded)
            return new ForbidResult();

        try
        {
            await _commentService.DeleteAsync(commentId, cancellationToken);
            return Ok();
        }
        catch (CommentNotFoundException ex)
        {
            _logger.LogInformation(ex, string.Concat(ex.Message, "\nid: {id}:"), commentId);
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, string.Concat(ex.Message, "\nid: {id}:"), commentId);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [Authorize(Policy = "CanLike")]
    [HttpPut("{commentId}/like")]
    public async Task<IActionResult> SetLike([FromRoute] Guid commentId, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        try
        {
            await _commentService.SetLikeAsync(commentId, userId, cancellationToken);
            return NoContent();
        }
        catch (CommentNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (UserBannedException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
        }
    }

    [Authorize(Policy = "CanLike")]
    [HttpPut("{commentId}/unlike")]
    public async Task<IActionResult> UnsetLike([FromRoute] Guid commentId, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        try
        {
            await _commentService.UnsetLikeAsync(commentId, userId, cancellationToken);
            return NoContent();
        }
        catch (CommentNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
