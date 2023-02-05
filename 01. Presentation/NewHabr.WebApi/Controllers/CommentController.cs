using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewHabr.Domain.Contracts.Services;
using NewHabr.Domain.Dto;
using NewHabr.Domain.Exceptions;

namespace NewHabr.WebApi.Controllers;
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class CommentController : ControllerBase
{
    private readonly ICommentService _commentService;
    private readonly ILogger _logger;

    public CommentController(ICommentService commentService, ILogger logger)
    {
        _commentService = commentService;
        _logger = logger;
    }

    //[Authorize(Roles = "admin")] какие у нас в итоге роли будут?
    [HttpGet("{id}")]
    public async Task<ActionResult<CommentDto>> GetByUserIdAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await _commentService.GetByUserIdAsync(id, false, cancellationToken));
        }
        catch (CommentNotFoundException ex)
        {
            _logger.LogInformation(ex, string.Concat(ex.Message, "\nid: {id}:"), id);
            return NotFound();
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, string.Concat(ex.Message, "\nid: {id}:"), id);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("all")]
    public async Task<ActionResult<List<CommentDto>>> GetAllAsync(bool trackChanges, CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await _commentService.GetAllAsync(trackChanges, cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("user/{id}")]
    public async Task<ActionResult<List<CommentDto>>> GetByUserIdAsync([FromRoute] Guid userId, bool trackChanges, CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await _commentService.GetByUserIdAsync(userId, trackChanges, cancellationToken));
        }
        catch(CommentNotFoundException ex)
        {
            _logger.LogInformation(ex, string.Concat(ex.Message, "\nid: {id}:"), userId);
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, string.Concat(ex.Message, "\nid: {id}:"), userId);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("article/{id}")]
    public async Task<ActionResult<List<CommentDto>>> GetByArticleIdAsync([FromRoute] Guid articleId, bool trackChanges, CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await _commentService.GetByArticleIdAsync(articleId, trackChanges, cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, string.Concat(ex.Message, "\nid: {id}:"), articleId);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost]
    public async Task<ActionResult> CreateAsync([FromBody] CreateCommentRequest newComment, CancellationToken cancellationToken)
    {
        try
        {
            await _commentService.CreateAsync(newComment, cancellationToken);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, string.Concat(ex.Message, "\nid: {id}:"), newComment.UserId);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPut]
    public async Task<ActionResult> UpdateAsync([FromBody] CommentDto comment, CancellationToken cancellationToken)
    {
        try
        {
            await _commentService.UpdateAsync(comment, cancellationToken);
            return Ok();
        }
        catch (CommentNotFoundException ex)
        {
            return NotFound(comment.Id);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, string.Concat(ex.Message, "\nid: {id}:"), comment.Id);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteAsync([FromBody] CommentDto commentDto, CancellationToken cancellationToken)
    {
        try
        {
            await _commentService.DeleteAsync(commentDto, cancellationToken);
            return Ok();
        }
        catch (CommentNotFoundException ex)
        {

            return NotFound();
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, string.Concat(ex.Message, "\nid: {id}:"), commentDto.Id);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
