using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewHabr.Business.Services;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Dto;

namespace NewHabr.WebApi.Controllers;

[ApiController, Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ILogger<AuthController> _logger;


    public AuthController(
        IAuthenticationService authenticationService,
        ILogger<AuthController> logger)
    {
        _authenticationService = authenticationService;
        _logger = logger;
    }


    [HttpPost]
    public async Task<IActionResult> RegisterUser([FromBody] RegistrationRequest request, CancellationToken cancellationToken)
    {
        var result = await _authenticationService.RegisterUserAndCreateToken(request, cancellationToken);

        if (result.Failure)
        {
            return BadRequest(result.Error);
        }
        return Ok(result.Value);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Authenticate([FromBody] AuthorizationRequest request, CancellationToken cancellationToken)
    {
        if (!await _authenticationService.ValidateUser(request, cancellationToken))
        {
            return Unauthorized();
        }

        var response = await _authenticationService.AuthenticateAsync();

        return Ok(response);
    }
}

