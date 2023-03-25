using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewHabr.Domain.Contracts.Services;
using NewHabr.Domain.Dto;
using NewHabr.WebApi.Extensions;

namespace NewHabr.WebApi.Controllers;

[Route("api/[controller]")]
[Produces("application/json")]
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

    /// <summary>
    /// Retrieves a specific Comment
    /// </summary>
    [HttpGet("{commentId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CommentDto>> GetById([FromRoute] Guid commentId, CancellationToken cancellationToken)
    {
        var comment = await _commentService
            .GetByIdAsync(commentId, cancellationToken);
        return Ok(comment);
    }

    /// <summary>
    /// Retrieves all comments
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<CommentDto>>> GetAll(CancellationToken cancellationToken)
    {
        return Ok(await _commentService.GetAllAsync(cancellationToken));
    }

    /// <summary>
    /// Creates a new comment on article
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [Authorize(Policy = "CanCreate")]
    public async Task<ActionResult<CommentDto>> Create([FromBody] CommentCreateRequest newComment, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        var comment = await _commentService.CreateAsync(userId, newComment, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { commentId = comment.Id }, comment);
    }

    /// <summary>
    /// Updates a specific comment
    /// </summary>
    [HttpPut("{commentId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
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

    /// <summary>
    /// Deletes a specific comment
    /// </summary>
    [HttpDelete("{commentId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
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

    /// <summary>
    /// Mark a specific comment as Liked
    /// </summary>
    [HttpPut("{commentId}/like")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Policy = "CanLike")]
    public async Task<ActionResult> SetLike([FromRoute] Guid commentId, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        await _commentService.SetLikeAsync(commentId, userId, cancellationToken);
        return NoContent();

    }

    /// <summary>
    /// Clears 'Liked' mark on specific comment
    /// </summary>
    [Authorize(Policy = "CanLike")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut("{commentId}/unlike")]
    public async Task<ActionResult> UnsetLike([FromRoute] Guid commentId, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        await _commentService.UnsetLikeAsync(commentId, userId, cancellationToken);
        return NoContent();

    }
}
