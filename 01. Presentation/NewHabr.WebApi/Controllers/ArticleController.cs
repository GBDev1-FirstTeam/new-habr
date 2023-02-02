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
    public async Task<ActionResult<IReadOnlyCollection<ArticleDto>>> GetByTitleAsync(
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
}
