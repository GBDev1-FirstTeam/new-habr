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

    [HttpGet("{title}")]
    public async Task<ActionResult<IEnumerable<ArticleDto>>> GetByTitleAsync(
        [FromRoute] string title,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(title))
        {
            return BadRequest();
        }

        try
        {
            var result = await _articleService.GetByTitleAsync(title, cancellationToken);
            return result is null ? NoContent() : Ok(result);
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
            var result = await _articleService.GetByUserIdAsync(userId, cancellationToken);
            return result is null ? NoContent() : Ok(result);
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
            var result = await _articleService.GetPublishedAsync(cancellationToken);
            return result is null ? NoContent() : Ok(result);
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
            var result = await _articleService.GetDeletedAsync(cancellationToken);
            return result is null ? NoContent() : Ok(result);
        }
        catch
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}
