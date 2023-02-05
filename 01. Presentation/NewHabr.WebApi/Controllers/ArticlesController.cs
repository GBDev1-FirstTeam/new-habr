using Microsoft.AspNetCore.Mvc;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Dto;
using NewHabr.Domain.Exceptions;

namespace NewHabr.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ArticlesController : ControllerBase
{
    private readonly IArticleService _articleService;

    public ArticlesController(IArticleService articleService)
    {
        _articleService = articleService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ArticleDto>> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await _articleService.GetByIdAsync(id, cancellationToken));
        }
        catch (ArticleNotFoundException)
        {
            return NotFound();
        }
        catch
        {
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
        catch
        {
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
        catch
        {
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
        catch
        {
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
        catch (ArticleNotFoundException)
        {
            return NotFound();
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPut]
    public async Task<ActionResult> SetPublicationStatus([FromBody] SetArticlePublicationStatusRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await _articleService.SetPublicationStatusAsync(request, cancellationToken);
            return Ok();
        }
        catch (ArticleNotFoundException)
        {
            return NotFound();
        }
        catch
        {
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
        catch (ArticleNotFoundException)
        {
            return NotFound();
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
