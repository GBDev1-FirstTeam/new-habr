using System.Net;
using Microsoft.AspNetCore.Mvc;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Dto;

namespace NewHabr.WebApi.Controllers;

[Route("[controller]")]
[ApiController]
public class ArticleController : Controller
{
    private readonly IArticleService _articleService;

    public ArticleController(IArticleService articleService)
    {
        _articleService = articleService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ArticleDto>>> GetByTitleAsync(
        [FromQuery] string title,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(title))
        {
            return BadRequest();
        }

        try
        {
            return Ok(await _articleService.GetByTitleAsync(title, cancellationToken));
        }
        catch
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<IEnumerable<ArticleDto>>> GetByUserIdAsync(
    [FromRoute] Guid userId,
    CancellationToken cancellationToken = default)
    {
        try
        {
            return Ok(await _articleService.GetByUserIdAsync(userId, cancellationToken));
        }
        catch
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet("published")]
    public async Task<ActionResult<IEnumerable<ArticleDto>>> GetPublishedAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return Ok(await _articleService.GetPublishedAsync(cancellationToken));
        }
        catch
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet("deleted")]
    public async Task<ActionResult<IEnumerable<ArticleDto>>> GetDeletedAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return Ok(await _articleService.GetDeletedAsync(cancellationToken));
        }
        catch
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPost]
    public async Task<ActionResult> CreateAsync([FromQuery] CreateArticleRequest request, CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            return BadRequest();
        }

        try
        {
            await _articleService.CreateAsync(request, cancellationToken);
            return Ok();
        }
        catch (OperationCanceledException)
        {
            return Ok();
        }
        catch (ArgumentNullException)
        {
            return BadRequest();
        }
        catch
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPut]
    public async Task<ActionResult> UpdateAsync([FromQuery] ArticleDto articleToUpdate, CancellationToken cancellationToken = default)
    {
        if (articleToUpdate is null)
        {
            return BadRequest();
        }

        try
        {
            await _articleService.UpdateAsync(articleToUpdate, cancellationToken);
            return Ok();
        }
        catch (OperationCanceledException)
        {
            return Ok();
        }
        catch (ArgumentNullException)
        {
            return BadRequest();
        }
        catch
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            await _articleService.DeleteByIdAsync(id, cancellationToken);
            return Ok();
        }
        catch (OperationCanceledException)
        {
            return Ok();
        }
        catch (ArgumentNullException)
        {
            return BadRequest();
        }
        catch
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}
