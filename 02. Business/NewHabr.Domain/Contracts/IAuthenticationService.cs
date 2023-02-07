using System;
using Microsoft.AspNetCore.Identity;
using NewHabr.Domain.Dto;

namespace NewHabr.Domain.Contracts;

public interface IAuthenticationService
{
    /// <summary>
    /// Register a new user
    /// </summary>
    /// <param name="request">Parameters for user creating</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns></returns>
    Task<IdentityResult> RegisterUserAsync(RegistrationRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Check if username exist, and password matches
    /// </summary>
    /// <param name="authorizationRequest"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> ValidateUser(AuthorizationRequest authorizationRequest, CancellationToken cancellationToken);

    /// <summary>
    /// Create JWT token
    /// </summary>
    /// <returns>String representing jwt token</returns>
    Task<string> CreateTokenAsync();

    /// <summary>
    /// Register new user and create token; two in one
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Result<AuthorizationResponse>> RegisterUserAndCreateToken(RegistrationRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Create jwt token and userdto
    /// </summary>
    /// <returns></returns>
    Task<AuthorizationResponse> AuthenticateAsync();
}

