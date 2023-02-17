using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewHabr.Domain.Contracts.Services;
using NewHabr.Domain.Dto;
using NewHabr.Domain.Exceptions;
using NewHabr.WebApi.Extensions;

namespace NewHabr.WebApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class CommentsController : ControllerBase
{
    private readonly ICommentService _commentService;
    private readonly ILogger<CommentsController> _logger;

    public CommentsController(ICommentService commentService, ILogger<CommentsController> logger)
    {
        _commentService = commentService;
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

    [HttpPut("id")] //todo кто может изменять? автор
    public async Task<ActionResult> Update([FromRoute] Guid id,
        [FromBody] CommentUpdateRequest updateComment,
        CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        try
        {
            await _commentService.UpdateAsync(id, userId, updateComment, cancellationToken);
            return Ok();
        }
        catch (CommentNotFoundException ex)
        {
            _logger.LogInformation(ex, string.Concat(ex.Message, "\nid: {id}:"), id);
            return NotFound(ex.Message);
        }
        catch (UserBannedException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, string.Concat(ex.Message, "\nid: {id}:"), id);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpDelete("id")] //todo кто может удалять? автор, модератор, админ
    public async Task<ActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _commentService.DeleteAsync(id, cancellationToken);
            return Ok();
        }
        catch (CommentNotFoundException ex)
        {
            _logger.LogInformation(ex, string.Concat(ex.Message, "\nid: {id}:"), id);
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, string.Concat(ex.Message, "\nid: {id}:"), id);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [Authorize]
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

    [Authorize]
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
