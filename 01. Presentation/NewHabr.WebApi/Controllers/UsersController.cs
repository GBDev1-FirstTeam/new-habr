using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewHabr.Domain.Dto;

namespace NewHabr.WebApi.Controllers;

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
    public Task<IActionResult> GetUserArticles([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    [HttpGet("{id}/comments")]
    public Task<IActionResult> GetUserComments([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    [HttpGet("{id}/notifications")]
    public Task<IActionResult> GetUserNotifications([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
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
    public Task<IActionResult> UpdateUserProfile([FromRoute] Guid id, [FromBody] UserForManipulationDto userDto, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }


    [HttpPut("{id}/assignroles")]
    [Authorize(Roles = "Administrator")]
    public Task<IActionResult> AssignRoles([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    [HttpPut("{id}/ban")]
    [Authorize(Roles = "Moderator")]
    public Task<IActionResult> SetBanOnUser([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

