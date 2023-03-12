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


    [HttpGet("{id}")]
    public async Task<ActionResult<ArticleDto>> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var authUserId = User.GetUserIdOrDefault();
        return Ok(await _articleService.GetByIdAsync(id, authUserId, cancellationToken));

    }

    [HttpGet]
    public async Task<ActionResult<ArticlesGetResponse>> GetPublished([FromQuery] ArticleQueryParametersDto queryParamsDto, CancellationToken cancellationToken)
    {
        var authUserId = User.GetUserIdOrDefault();
        return Ok(await _articleService.GetPublishedAsync(authUserId, queryParamsDto, cancellationToken));
    }

    [HttpGet("unpublished")]
    [Authorize(Roles = "Moderator,Administrator")]
    public async Task<ActionResult<ArticlesGetResponse>> GetUnpublished([FromQuery] ArticleQueryParametersDto queryParamsDto, CancellationToken cancellationToken)
    {
        return Ok(await _articleService.GetUnpublishedAsync(queryParamsDto, cancellationToken));
    }

    [HttpGet("deleted")]
    [Authorize(Roles = "Moderator,Administrator")]
    public async Task<ActionResult<ArticlesGetResponse>> GetDeleted([FromQuery] ArticleQueryParametersDto queryParamsDto, CancellationToken cancellationToken)
    {
        return Ok(await _articleService.GetDeletedAsync(queryParamsDto, cancellationToken));
    }

    [HttpPost]
    [Authorize(Policy = "CanCreate")]
    public async Task<ActionResult> Create([FromBody] ArticleCreateRequest request, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        await _articleService.CreateAsync(request, userId, cancellationToken);
        return Ok();
    }

    [HttpPut("{id}")]
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

    [HttpPut("{id}/publish")]
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

    [HttpPut("{id}/unpublish")]
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

    [HttpPut("{id}/approve")]
    [Authorize(Roles = "Moderator,Administrator")]
    public async Task<ActionResult> SetApproveState([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await _articleService.SetApproveStateAsync(id, cancellationToken);
        return NoContent();
    }

    [HttpPut("{id}/disapprove")]
    [Authorize(Roles = "Moderator,Administrator")]
    public async Task<ActionResult> SetDisapproveState([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await _articleService.SetDisapproveStateAsync(id, cancellationToken);
        return NoContent();
    }

    [Authorize(Policy = "CanLike")]
    [HttpPut("{articleId}/like")]
    public async Task<IActionResult> SetLike([FromRoute] Guid articleId, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        await _articleService.SetLikeAsync(articleId, userId, cancellationToken);
        return NoContent();
    }

    [Authorize(Policy = "CanLike")]
    [HttpPut("{articleId}/unlike")]
    public async Task<IActionResult> UnsetLike([FromRoute] Guid articleId, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        await _articleService.UnsetLikeAsync(articleId, userId, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id}")]
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
