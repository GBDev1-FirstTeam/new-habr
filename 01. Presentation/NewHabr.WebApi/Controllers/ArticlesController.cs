using System.Net;
using Microsoft.AspNetCore.Mvc;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Dto;

namespace NewHabr.WebApi.Controllers;

[Route("[controller]")]
[ApiController]
public class ArticlesController : ControllerBase
{
    private readonly IArticleService _articleService;

    public ArticlesController(IArticleService articleService)
    {
        _articleService = articleService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ArticleDto>>> GetByTitle([FromBody] string title, CancellationToken cancellationToken)
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
    public async Task<ActionResult<IEnumerable<ArticleDto>>> GetByUserId([FromRoute] Guid userId, CancellationToken cancellationToken)
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
    public async Task<ActionResult<IEnumerable<ArticleDto>>> GetPublished(CancellationToken cancellationToken)
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
    public async Task<ActionResult<IEnumerable<ArticleDto>>> GetDeleted(CancellationToken cancellationToken)
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
    public async Task<ActionResult> Create([FromBody] CreateArticleRequest request, CancellationToken cancellationToken)
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
    public async Task<ActionResult> Update([FromBody] ArticleDto articleToUpdate, CancellationToken cancellationToken)
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
    public async Task<ActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
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
