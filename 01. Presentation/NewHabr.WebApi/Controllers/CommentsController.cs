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
    public async Task<ActionResult> Createc([FromBody] CreateCommentRequest newComment, CancellationToken cancellationToken)
    {
        try
        {
            await _commentService.CreateAsync(newComment, cancellationToken);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, string.Concat(ex.Message, "\nuser id: {id}:"), newComment.UserId);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPut("id")]
    public async Task<ActionResult> Update([FromRoute] Guid id,
        [FromBody] UpdateCommentRequest updateComment,
        CancellationToken cancellationToken)
    {
        try
        {
            await _commentService.UpdateAsync(id, updateComment, cancellationToken);
            return Ok();
        }
        catch (CommentNotFoundException ex)
        {
            _logger.LogInformation(ex, string.Concat(ex.Message, "\nid: {id}:"), id);
            return NotFound(ex.Message);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, string.Concat(ex.Message, "\nid: {id}:"), id);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpDelete("id")]
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
        catch(Exception ex)
        {
            _logger.LogError(ex, string.Concat(ex.Message, "\nid: {id}:"), id);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
