using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewHabr.Domain.Contracts.Services;
using NewHabr.Domain.Dto;
using NewHabr.Domain.Exceptions;
using NewHabr.Domain.Models;

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
    public async Task<ActionResult<List<CommentDto>>> GetAllAsync(CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await _commentService.GetAllAsync(false, cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
        
    [HttpGet("user/{userId}/comments")]
    public async Task<ActionResult<List<CommentDto>>> GetByUserIdAsync([FromRoute] Guid userId, CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await _commentService.GetByUserIdAsync(userId, false, cancellationToken));
        }
        catch (CommentNotFoundException ex)
        {
            _logger.LogInformation(ex, string.Concat(ex.Message, "\nuser id: {userId}:"), userId);
            return NotFound();
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, string.Concat(ex.Message, "\nuser id: {userId}"), userId);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("article/{articleId}/comments")]
    public async Task<ActionResult<List<CommentDto>>> GetByArticleIdAsync([FromRoute] Guid articleId, CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await _commentService.GetByArticleIdAsync(articleId, false, cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, string.Concat(ex.Message, "\narticle id: {articleId}:"), articleId);
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
            _logger.LogError(ex, string.Concat(ex.Message, "\nsuer id: {userId}:"), newComment.UserId);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPut]
    public async Task<ActionResult> UpdateAsync([FromBody] CommentDto commentDto, CancellationToken cancellationToken)
    {
        try
        {
            await _commentService.UpdateAsync(commentDto, cancellationToken);
            return Ok();
        }
        catch (CommentNotFoundException ex)
        {
            _logger.LogInformation(ex, string.Concat(ex.Message, "\nid: {id}:"), commentDto.Id);
            return NotFound(commentDto.Id);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, string.Concat(ex.Message, "\nid: {id}:"), commentDto.Id);
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
            _logger.LogInformation(ex, string.Concat(ex.Message, "\nid: {id}:"), commentDto.Id);
            return NotFound();
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, string.Concat(ex.Message, "\nid: {id}:"), commentDto.Id);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
