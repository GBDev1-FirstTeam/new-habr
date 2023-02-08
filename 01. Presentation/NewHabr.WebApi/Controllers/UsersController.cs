using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewHabr.Domain.Contracts.Services;
using NewHabr.Domain.Dto;
using NewHabr.Domain.Exceptions;

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
    public Task<IActionResult> GetUsers(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    [HttpGet("{id}")]
    public Task<IActionResult> GetUserDetails([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
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
    public Task<IActionResult> GetUserLikedArticles([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    [HttpGet("{id}/likedComments")]
    public Task<IActionResult> GetUserLikedComments([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    [HttpGet("{id}/likedUsers")]
    public Task<IActionResult> GetUserLikedUsers([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUserProfile([FromRoute] Guid id, [FromBody] UserForManipulationDto userDto, CancellationToken cancellationToken)
    {
        UserProfileDto response;
        try
        {
            response = await _userService.UpdateUserProfileAsync(id, userDto, cancellationToken);
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
}

