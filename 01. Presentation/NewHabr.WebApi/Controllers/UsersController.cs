using System;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewHabr.Business.Services;
using NewHabr.Domain.Contracts.Services;
using NewHabr.Domain.Dto;
using NewHabr.Domain.Exceptions;
using NewHabr.WebApi.Extensions;

namespace NewHabr.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;


    public UsersController(IUserService userService)
    {
        _userService = userService;
    }


    [HttpGet]
    [Authorize(Roles = "Moderator,Administrator")]
    public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await _userService.GetUsers(cancellationToken));
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserDetails([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var authUserId = User.GetUserIdOrDefault();
        try
        {
            var response = await _userService
                .GetUserInfoAsync(id, authUserId, cancellationToken);
            return Ok(response);
        }
        catch (UserNotFoundException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}/articles")]
    public async Task<IActionResult> GetUserArticles([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var articles = await _userService.GetUserArticlesAsync(id, cancellationToken);
            return Ok(articles);
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("{id}/comments")]
    public async Task<IActionResult> GetUserComments([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var comments = await _userService.GetUserCommentsAsync(id, cancellationToken);
            return Ok(comments);
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("{id}/notifications")]
    public async Task<IActionResult> GetUserNotifications([FromRoute] Guid id, CancellationToken cancellationToken, [FromQuery] bool? unread = false)
    {
        try
        {
            var notification = await _userService.GetUserNotificationsAsync(id, unread ?? false, cancellationToken);
            return Ok(notification);
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("{id}/likedArticles")]
    public async Task<IActionResult> GetUserLikedArticles([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _userService.GetUserLikedArticlesAsync(id, cancellationToken);
            return Ok(response);
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("{id}/likedComments")]
    public async Task<IActionResult> GetUserLikedComments([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _userService.GetUserLikedCommentsAsync(id, cancellationToken);
            return Ok(response);
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("{id}/likedUsers")]
    public async Task<IActionResult> GetUserLikedUsers([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _userService.GetUserLikedUsersAsync(id, cancellationToken);
            return Ok(response);
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUserProfile([FromRoute] Guid id, [FromBody] UserForManipulationDto userDto, CancellationToken cancellationToken)
    {
        try
        {
            await _userService.UpdateUserProfileAsync(id, userDto, cancellationToken);
            return NoContent();
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }


    [HttpPut("{id}/setroles")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> SetRoles([FromRoute] Guid id, [FromBody] UserAssignRolesRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await _userService.SetUserRolesAsync(id, request, cancellationToken);
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        return NoContent();
    }

    [HttpGet("{id}/getroles")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> GetRoles([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        UserAssignRolesResponse response;
        try
        {
            response = await _userService.GetUserRolesAsync(id, cancellationToken);
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        return Ok(response);
    }

    [HttpPut("{id}/ban")]
    [Authorize(Roles = "Moderator")]
    public async Task<IActionResult> SetBanOnUser([FromRoute] Guid id, [FromBody] UserBanDto userBanDto, CancellationToken cancellationToken)
    {
        try
        {
            await _userService.SetBanOnUserAsync(id, userBanDto, cancellationToken);
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        return NoContent();
    }

    [Authorize]
    [HttpPut("{userId}/like")]
    public async Task<IActionResult> SetLike([FromRoute] Guid userId, CancellationToken cancellationToken)
    {
        var authUserId = User.GetUserId();
        try
        {
            await _userService.SetLikeAsync(userId, authUserId, cancellationToken);
            return NoContent();
        }
        catch (CommentNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (UserBannedException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
        }
    }

    [Authorize]
    [HttpPut("{userId}/unlike")]
    public async Task<IActionResult> UnsetLike([FromRoute] Guid userId, CancellationToken cancellationToken)
    {
        var authUserId = User.GetUserId();
        try
        {
            await _userService.UnsetLikeAsync(userId, authUserId, cancellationToken);
            return NoContent();
        }
        catch (CommentNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize]
    [HttpPut("{userId}/username")]
    public async Task<IActionResult> ChangeUsername([FromRoute] Guid userId, [FromBody] UsernameChangeRequest request, CancellationToken cancellationToken)
    {
        var authUserId = User.GetUserId();
        if (userId != authUserId)
            return BadRequest();

        try
        {
            var result = await _userService.ChangeUsername(authUserId, request, cancellationToken);
            if (result.Succeeded)
            {
                return NoContent();
            }
            return BadRequest(result.Errors.Select(e => e.Description));
        }
        catch (UserNotFoundException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpPut("{userId}/password")]
    public async Task<IActionResult> ChangePassword([FromRoute] Guid userId, [FromBody] UserPasswordChangeRequest request, CancellationToken cancellationToken)
    {
        var authUserId = User.GetUserId();
        if (userId != authUserId)
            return BadRequest();

        try
        {
            var result = await _userService.ChangePassword(authUserId, request, cancellationToken);
            if (result.Succeeded)
            {
                return NoContent();
            }
            return BadRequest(result.Errors.Select(e => e.Description));
        }
        catch (UserNotFoundException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
