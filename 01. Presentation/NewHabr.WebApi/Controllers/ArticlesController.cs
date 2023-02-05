﻿using Microsoft.AspNetCore.Mvc;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Dto;
using NewHabr.Domain.Exceptions;

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

    [HttpGet("{id}")]
    public async Task<ActionResult<ArticleDto>> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await _articleService.GetByIdAsync(id, cancellationToken));
        }
        catch (ArticleNotFoundException ex)
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

    [HttpGet("unpublished")]
    public async Task<ActionResult<IEnumerable<ArticleDto>>> GetUnpublished(CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await _articleService.GetUnpublishedAsync(cancellationToken));
        }
        catch(Exception ex)
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
            await _articleService.CreateAsync(request, cancellationToken);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, string.Concat(ex.Message, "\nrequest: {@request}:"), request);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPut("update")]
    public async Task<ActionResult> Update([FromBody] UpdateArticleRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await _articleService.UpdateAsync(request, cancellationToken);
            return Ok();
        }
        catch (ArticleNotFoundException ex)
        {
            _logger.LogInformation(ex, string.Concat(ex.Message, "\nrequest: {@request}:"), request);
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, string.Concat(ex.Message, "\nrequest: {@request}:"), request);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _articleService.DeleteByIdAsync(id, cancellationToken);
            return Ok();
        }
        catch (ArticleNotFoundException ex)
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

    [HttpPut("publish/{id}")]
    public async Task<ActionResult> Publish([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _articleService.SetPublicationStatusAsync(id, true, cancellationToken);
            return Ok();
        }
        catch (ArticleNotFoundException ex)
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

    [HttpPut("unpublish/{id}")]
    public async Task<ActionResult> Unpublish([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _articleService.SetPublicationStatusAsync(id, false, cancellationToken);
            return Ok();
        }
        catch (ArticleNotFoundException ex)
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
}
