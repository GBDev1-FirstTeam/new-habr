﻿using Microsoft.AspNetCore.Mvc;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Dto;
using NewHabr.Domain.Exceptions;
using NewHabr.Domain.Models;
using NewHabr.WebApi.Extensions;

namespace NewHabr.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ArticlesController : ControllerBase
{
    private readonly IArticleService _articleService;
    private readonly ILogger<ArticlesController> _logger;

    public ArticlesController(IArticleService articleService, ILogger<ArticlesController> logger)
    {
        _articleService = articleService;
        _logger = logger;
    }

    [HttpGet("{id}/comments")]
    public async Task<ActionResult<IEnumerable<CommentWithLikedMark>>> GetCommentsWithLikedMark(
        [FromRoute] Guid id,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await _articleService.GetCommentsWithLikedMarkAsync(id, userId, cancellationToken));
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogInformation(ex, string.Concat(ex.Message, "\nid: {id}\nuser id: {userId}"), id, userId);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, string.Concat(ex.Message, "\nid: {id}\nuser id: {userId}"), id, userId);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ArticleDto>> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var article = await _articleService.GetByIdAsync(id, cancellationToken);
            return article is not null
                ? Ok(article)
                : NoContent();
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogInformation(ex, string.Concat(ex.Message, "\nid: {id}"), id);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, string.Concat(ex.Message, "\nid: {id}"), id);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("unpublished")]
    public async Task<ActionResult<IEnumerable<ArticleDto>>> GetUnpublished(CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await _articleService.GetUnpublishedAsync(cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("deleted")]
    public async Task<ActionResult<IEnumerable<ArticleDto>>> GetDeleted(CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await _articleService.GetDeletedAsync(cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateArticleRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var createdArticle = await _articleService.CreateAsync(User.GetUserId(), request, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, createdArticle);
        }
        catch (UserBannedException ex)
        {
            _logger.LogInformation(ex, string.Concat(ex.Message, "\nrequest: {@request}"), request);
            return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogInformation(ex, string.Concat(ex.Message, "\nrequest: {@request}"), request);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, string.Concat(ex.Message, "\nrequest: {@request}"), request);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update([FromRoute] Guid id, [FromBody] UpdateArticleRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await _articleService.UpdateAsync(id, User.GetUserId(), request, cancellationToken);
            return Ok();
        }
        catch (UserBannedException ex)
        {
            _logger.LogInformation(ex, string.Concat(ex.Message, "\nrequest: {@request}\narticle id: {id}"), request, id);
            return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogInformation(ex, string.Concat(ex.Message, "\nrequest: {@request}\narticle id: {id}"), request, id);
            return NotFound(ex.Message);
        }
        catch (InteractionOutsidePermissionException<object, object> ex)
        {
            _logger.LogWarning(ex, string.Concat(ex.Message, "\nrequest: {@request}\narticle id: {id}"), request, id);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, string.Concat(ex.Message, "\nrequest: {@request}\narticle id: {id}"), request, id);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPut("{id}/publish")]
    public async Task<ActionResult> Publish([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _articleService.SetPublicationStatusAsync(id, true, cancellationToken);
            return Ok();
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogInformation(ex, string.Concat(ex.Message, "\nid: {id}"), id);
            return NotFound(ex.Message);
        }
        catch (ArticleIsNotApprovedException ex)
        {
            _logger.LogInformation(ex, string.Concat(ex.Message, "\nid: {id}"), id);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, string.Concat(ex.Message, "\nid: {id}:"), id);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPut("{id}/unpublish")]
    public async Task<ActionResult> Unpublish([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _articleService.SetPublicationStatusAsync(id, false, cancellationToken);
            return Ok();
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogInformation(ex, string.Concat(ex.Message, "\nid: {id}"), id);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, string.Concat(ex.Message, "\nid: {id}"), id);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPut("{id}/approve")]
    public async Task<ActionResult> SetApproveState(
        [FromRoute] Guid id,
        [FromQuery] ApproveState approveState,
        CancellationToken cancellationToken)
    {
        try
        {
            await _articleService.SetApproveStateAsync(id, approveState, cancellationToken);
            return Ok();
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogInformation(ex, string.Concat(ex.Message, "\nid: {id}\napprove state: {approveState}"), id, approveState);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, string.Concat(ex.Message, "\nid: {id}\napprove state: {approveState}"), id, approveState);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _articleService.DeleteByIdAsync(id, User.GetUserId(), cancellationToken);
            return Ok();
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogInformation(ex, string.Concat(ex.Message, "\nid: {id}"), id);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, string.Concat(ex.Message, "\nid: {id}"), id);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
