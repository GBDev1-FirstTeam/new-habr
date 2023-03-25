using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewHabr.Domain.Contracts.Services;
using NewHabr.Domain.Dto;
using NewHabr.Domain.Exceptions;
using NewHabr.WebApi.Extensions;

namespace NewHabr.WebApi.Controllers;

[ApiController]
[Produces("application/json")]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IAuthorizationService _authorizationService;


    public UsersController(IUserService userService, IAuthorizationService authorizationService)
    {
        _userService = userService;
        _authorizationService = authorizationService;
    }


    /// <summary>
    /// Retrieves reset password token
    /// </summary>
    [HttpPost("requestRecovery")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [AllowAnonymous]
    public async Task<ActionResult<RecoveryResponse>> ForgotPassword([FromBody] RecoveryRequest recoveryRequest, CancellationToken cancellationToken)
    {
        return Ok(await _userService.ForgotPasswordAsync(recoveryRequest, cancellationToken));
    }

    /// <summary>
    /// Resets user's password
    /// </summary>
    [HttpPut("resetPassword")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [AllowAnonymous]
    public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordRequest resetPasswordRequest, CancellationToken cancellationToken)
    {
        var result = await _userService.ResetPasswordAsync(resetPasswordRequest, cancellationToken);
        if (result.Succeeded)
        {
            return NoContent();
        }
        return BadRequest(result.Errors.Select(e => e.Description));
    }

    /// <summary>
    /// Retrieves all users
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize(Roles = "Moderator,Administrator")]
    public async Task<ActionResult<ICollection<UserProfileDto>>> GetUsers(CancellationToken cancellationToken)
    {
        return Ok(await _userService.GetUsers(cancellationToken));
    }

    /// <summary>
    /// Retrieves a specific user
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserProfileDto>> GetUserDetails([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var authUserId = User.GetUserIdOrDefault();
        var response = await _userService
            .GetUserInfoAsync(id, authUserId, cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Retrieves specific author's articles
    /// </summary>
    [HttpGet("{id}/articles")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ArticlesGetResponse>> GetUserArticles([FromRoute] Guid id, [FromQuery] ArticleQueryParametersDto queryParametersDto, CancellationToken cancellationToken)
    {
        var authUserId = User.GetUserIdOrDefault();
        var articles = await _userService.GetUserArticlesAsync(id, authUserId, queryParametersDto, cancellationToken);
        return Ok(articles);
    }

    /// <summary>
    /// Retrieves specific author's comments
    /// </summary>
    [HttpGet("{id}/comments")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ICollection<UserCommentDto>>> GetUserComments([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var comments = await _userService.GetUserCommentsAsync(id, cancellationToken);
        return Ok(comments);
    }

    /// <summary>
    /// Retrieves specific author's notifications
    /// </summary>
    [HttpGet("{id}/notifications")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ICollection<NotificationDto>>> GetUserNotifications([FromRoute] Guid id, CancellationToken cancellationToken, [FromQuery] bool? unread = false)
    {
        var notification = await _userService.GetUserNotificationsAsync(id, unread ?? false, cancellationToken);
        return Ok(notification);
    }

    /// <summary>
    /// Retrieves articles liked by specific user
    /// </summary>
    [HttpGet("{id}/likedArticles")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ArticlesGetResponse>> GetUserLikedArticles([FromRoute] Guid id, [FromQuery] ArticleQueryParametersDto queryParametersDto, CancellationToken cancellationToken)
    {
        var response = await _userService.GetUserLikedArticlesAsync(id, queryParametersDto, cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Retrieves comments liked by specific user
    /// </summary>
    [HttpGet("{id}/likedComments")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ICollection<LikedCommentDto>>> GetUserLikedComments([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var response = await _userService.GetUserLikedCommentsAsync(id, cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Retrieves authors liked by specific user
    /// </summary>
    [HttpGet("{id}/likedUsers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ICollection<LikedUserDto>>> GetUserLikedUsers([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var response = await _userService.GetUserLikedUsersAsync(id, cancellationToken);
        return Ok(response);
    }


    /// <summary>
    /// Updates user details
    /// </summary>
    [HttpPut("{userId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize]
    public async Task<ActionResult> UpdateUserProfile([FromRoute] Guid userId, [FromBody] UserForManipulationDto userDto, CancellationToken cancellationToken)
    {
        var authResult = await _authorizationService.AuthorizeAsync(User, userId, "CanUpdateUserProfile");
        if (!authResult.Succeeded)
            return new ForbidResult();

        await _userService.UpdateUserProfileAsync(userId, userDto, cancellationToken);
        return NoContent();
    }


    /// <summary>
    /// Assign roles to specific user
    /// </summary>
    [HttpPut("{id}/setroles")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "Administrator")]
    public async Task<ActionResult> SetRoles([FromRoute] Guid id, [FromBody] UserAssignRolesRequest request, CancellationToken cancellationToken)
    {
        await _userService.SetUserRolesAsync(id, request, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Retrieves a specific user's roles
    /// </summary>
    [HttpGet("{id}/getroles")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "Administrator")]
    public async Task<ActionResult<UserAssignRolesResponse>> GetRoles([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var response = await _userService.GetUserRolesAsync(id, cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Mark a specific user as Banned
    /// </summary>
    [HttpPut("{id}/ban")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "Moderator,Administrator")]
    public async Task<ActionResult> SetBanOnUser([FromRoute] Guid id, [FromBody] UserBanDto userBanDto, CancellationToken cancellationToken)
    {
        await _userService.SetBanOnUserAsync(id, userBanDto, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Mark a specific user as Unbanned
    /// </summary>
    [HttpPut("{id}/unban")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "Moderator,Administrator")]
    public async Task<ActionResult> UnBanOnUser([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await _userService.UnBanUserAsync(id, cancellationToken);
        return NoContent();
    }


    /// <summary>
    /// Mark a specific author as liked by specific user
    /// </summary>
    [Authorize(Policy = "CanLike")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut("{userId}/like")]
    public async Task<ActionResult> SetLike([FromRoute] Guid userId, CancellationToken cancellationToken)
    {
        var authUserId = User.GetUserId();
        await _userService.SetLikeAsync(userId, authUserId, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Clears 'liked' mark of specific user on specific author
    /// </summary>
    [Authorize(Policy = "CanLike")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut("{userId}/unlike")]
    public async Task<ActionResult> UnsetLike([FromRoute] Guid userId, CancellationToken cancellationToken)
    {
        var authUserId = User.GetUserId();
        await _userService.UnsetLikeAsync(userId, authUserId, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Changes specific user's username
    /// </summary>
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut("{userId}/username")]
    public async Task<ActionResult> ChangeUsername([FromRoute] Guid userId, [FromBody] UsernameChangeRequest request, CancellationToken cancellationToken)
    {
        var authUserId = User.GetUserId();
        if (userId != authUserId)
            return BadRequest();

        var result = await _userService.ChangeUsername(authUserId, request, cancellationToken);
        if (result.Succeeded)
        {
            return NoContent();
        }
        return BadRequest(result.Errors.Select(e => e.Description));
    }

    /// <summary>
    /// Changes specific user's password
    /// </summary>
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut("{userId}/password")]
    public async Task<IActionResult> ChangePassword([FromRoute] Guid userId, [FromBody] UserPasswordChangeRequest request, CancellationToken cancellationToken)
    {
        var authUserId = User.GetUserId();
        if (userId != authUserId)
            return BadRequest();

        var result = await _userService.ChangePassword(authUserId, request, cancellationToken);
        if (result.Succeeded)
        {
            return NoContent();
        }
        return BadRequest(result.Errors.Select(e => e.Description));
    }
}
