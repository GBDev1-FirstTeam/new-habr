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
        try
        {
            return Ok(await _articleService.GetByIdAsync(id, authUserId, cancellationToken));
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

    [HttpGet]
    public async Task<ActionResult<ArticlesGetResponse>> GetPublished([FromQuery] ArticleQueryParametersDto queryParamsDto, CancellationToken cancellationToken)
    {
        var authUserId = User.GetUserIdOrDefault();
        try
        {
            return Ok(await _articleService.GetPublishedAsync(authUserId, queryParamsDto, cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("unpublished")]
    [Authorize(Roles = "Moderator,Administrator")]
    public async Task<ActionResult<ArticlesGetResponse>> GetUnpublished([FromQuery] ArticleQueryParametersDto queryParamsDto, CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await _articleService.GetUnpublishedAsync(queryParamsDto, cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("deleted")]
    [Authorize(Roles = "Moderator,Administrator")]
    public async Task<ActionResult<ArticlesGetResponse>> GetDeleted([FromQuery] ArticleQueryParametersDto queryParamsDto, CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await _articleService.GetDeletedAsync(queryParamsDto, cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost]
    [Authorize(Policy = "CanCreate")]
    public async Task<ActionResult> Create([FromBody] ArticleCreateRequest request, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        try
        {
            await _articleService.CreateAsync(request, userId, cancellationToken);
            return Ok();
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogInformation(ex, string.Concat(ex.Message, "\nrequest: {@request}"), request);
            return BadRequest(ex.Message);
        }
        catch (UserBannedException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, string.Concat(ex.Message, "\nrequest: {@request}"), request);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult> Update([FromRoute] Guid id, [FromBody] ArticleUpdateRequest request, CancellationToken cancellationToken)
    {
        var authResult = await _authorizationService
            .AuthorizeAsync(User, id, "CanManageArticle");
        if (!authResult.Succeeded)
            return new ForbidResult();
        try
        {
            await _articleService.UpdateAsync(id, request, cancellationToken);
            return Ok();
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogInformation(ex, string.Concat(ex.Message, "\nrequest: {@request}"), request);
            return NotFound(ex.Message);
        }
        catch (UserBannedException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, string.Concat(ex.Message, "\nrequest: {@request}"), request);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPut("{id}/publish")]
    [Authorize]
    public async Task<ActionResult> Publish([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var authResult = await _authorizationService
            .AuthorizeAsync(User, id, "CanManageArticle");
        if (!authResult.Succeeded)
            return new ForbidResult();

        try
        {
            await _articleService.PublishAsync(id, true, cancellationToken);
            return NoContent();
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogInformation(ex, string.Concat(ex.Message, "\nid: {id}"), id);
            return NotFound(ex.Message);
        }
        catch (ArticleIsNotApproveException ex)
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
    [Authorize]
    public async Task<ActionResult> Unpublish([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var authResult = await _authorizationService
            .AuthorizeAsync(User, id, "CanManageArticle");
        if (!authResult.Succeeded)
            return new ForbidResult();

        try
        {
            await _articleService.PublishAsync(id, false, cancellationToken);
            return NoContent();
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
    [Authorize(Roles = "Moderator,Administrator")]
    public async Task<ActionResult> SetApproveState([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _articleService.SetApproveStateAsync(id, cancellationToken);
            return NoContent();
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

    [HttpPut("{id}/disapprove")]
    [Authorize(Roles = "Moderator,Administrator")]
    public async Task<ActionResult> SetDisapproveState([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _articleService.SetDisapproveStateAsync(id, cancellationToken);
            return NoContent();
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

    [Authorize(Policy = "CanLike")]
    [HttpPut("{articleId}/like")]
    public async Task<IActionResult> SetLike([FromRoute] Guid articleId, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        try
        {
            await _articleService.SetLikeAsync(articleId, userId, cancellationToken);
            return NoContent();
        }
        catch (ArticleNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (UserBannedException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
        }
    }

    [Authorize(Policy = "CanLike")]
    [HttpPut("{articleId}/unlike")]
    public async Task<IActionResult> UnsetLike([FromRoute] Guid articleId, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        try
        {
            await _articleService.UnsetLikeAsync(articleId, userId, cancellationToken);
            return NoContent();
        }
        catch (ArticleNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var authResult = await _authorizationService
            .AuthorizeAsync(User, id, "CanManageArticle");
        if (!authResult.Succeeded)
            return new ForbidResult();

        try
        {
            await _articleService.DeleteByIdAsync(id, cancellationToken);
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
