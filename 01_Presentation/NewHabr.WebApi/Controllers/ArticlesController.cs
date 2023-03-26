using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Dto;
using NewHabr.Domain.Exceptions;
using NewHabr.Domain.Models;
using NewHabr.WebApi.Extensions;

namespace NewHabr.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class ArticlesController : ControllerBase
{
    private readonly IArticleService _articleService;
    private readonly IAuthorizationService _authorizationService;
    private readonly ILogger<ArticlesController> _logger;


    public ArticlesController(IArticleService articleService, IAuthorizationService authorizationService, ILogger<ArticlesController> logger)
    {
        _articleService = articleService;
        _authorizationService = authorizationService;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves a specific Article
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ArticleDto>> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var authUserId = User.GetUserIdOrDefault();
        return Ok(await _articleService.GetByIdAsync(id, authUserId, cancellationToken));
    }

    /// <summary>
    /// Retrieves published articles with paging
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<ArticlesGetResponse>> GetPublished([FromQuery] ArticleQueryParametersDto queryParamsDto, CancellationToken cancellationToken)
    {
        var authUserId = User.GetUserIdOrDefault();
        return Ok(await _articleService.GetPublishedAsync(authUserId, queryParamsDto, cancellationToken));
    }

    /// <summary>
    /// Retrieves unpublished articles
    /// </summary>
    [HttpGet("unpublished")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize(Roles = "Moderator,Administrator")]
    public async Task<ActionResult<ArticlesGetResponse>> GetUnpublished([FromQuery] ArticleQueryParametersDto queryParamsDto, CancellationToken cancellationToken)
    {
        return Ok(await _articleService.GetUnpublishedAsync(queryParamsDto, cancellationToken));
    }

    /// <summary>
    /// Retrieves articles marked as deleted
    /// </summary>
    [HttpGet("deleted")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize(Roles = "Moderator,Administrator")]
    public async Task<ActionResult<ArticlesGetResponse>> GetDeleted([FromQuery] ArticleQueryParametersDto queryParamsDto, CancellationToken cancellationToken)
    {
        return Ok(await _articleService.GetDeletedAsync(queryParamsDto, cancellationToken));
    }

    /// <summary>
    /// Creates a new article
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [Authorize(Policy = "CanCreate")]
    public async Task<ActionResult<ArticleDto>> Create([FromBody] ArticleCreateRequest request, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        var article = await _articleService.CreateAsync(request, userId, cancellationToken);
        return new CreatedAtActionResult(nameof(GetById), "Articles", new { Id = article.Id }, article);
    }

    /// <summary>
    /// Updates a specific article
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize]
    public async Task<ActionResult> Update([FromRoute] Guid id, [FromBody] ArticleUpdateRequest request, CancellationToken cancellationToken)
    {
        var authResult = await _authorizationService
            .AuthorizeAsync(User, id, "CanManageArticle");
        if (!authResult.Succeeded)
            return new ForbidResult();

        await _articleService.UpdateAsync(id, request, cancellationToken);
        return Ok();
    }

    /// <summary>
    /// Publishes a specific article
    /// </summary>
    [HttpPut("{id}/publish")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize]
    public async Task<ActionResult> Publish([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var authResult = await _authorizationService
            .AuthorizeAsync(User, id, "CanManageArticle");
        if (!authResult.Succeeded)
            return new ForbidResult();

        await _articleService.PublishAsync(id, true, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Unpublishes a specific article
    /// </summary>
    [HttpPut("{id}/unpublish")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize]
    public async Task<ActionResult> Unpublish([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var authResult = await _authorizationService
            .AuthorizeAsync(User, id, "CanManageArticle");
        if (!authResult.Succeeded)
            return new ForbidResult();

        await _articleService.PublishAsync(id, false, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Approves a specific article
    /// </summary>
    [HttpPut("{id}/approve")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "Moderator,Administrator")]
    public async Task<ActionResult> SetApproveState([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await _articleService.SetApproveStateAsync(id, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Disaproves a specific article
    /// </summary>
    [HttpPut("{id}/disapprove")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "Moderator,Administrator")]
    public async Task<ActionResult> SetDisapproveState([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await _articleService.SetDisapproveStateAsync(id, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Marks a specific article as Liked
    /// </summary>
    [HttpPut("{articleId}/like")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Policy = "CanLike")]
    public async Task<ActionResult> SetLike([FromRoute] Guid articleId, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        await _articleService.SetLikeAsync(articleId, userId, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Clears 'Like' mark on a specific article
    /// </summary>
    [HttpPut("{articleId}/unlike")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Policy = "CanLike")]
    public async Task<ActionResult> UnsetLike([FromRoute] Guid articleId, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        await _articleService.UnsetLikeAsync(articleId, userId, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Deletes a specific article
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var authResult = await _authorizationService
            .AuthorizeAsync(User, id, "CanManageArticle");
        if (!authResult.Succeeded)
            return new ForbidResult();

        await _articleService.DeleteByIdAsync(id, cancellationToken);
        return Ok();
    }
}
