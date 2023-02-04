using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NewHabr.Domain;
using NewHabr.Domain.ConfigurationModels;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Dto;
using NewHabr.Domain.Models;

namespace NewHabr.Business.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<UserRole> _roleManager;
    private readonly ILogger<AuthenticationService> _logger;
    private readonly JwtConfiguration _jwtConfiguration;
    private User? _user;


    public AuthenticationService(
        UserManager<User> userManager,
        RoleManager<UserRole> roleManager,
        IOptions<JwtConfiguration> jwtConfiguration,
        ILogger<AuthenticationService> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
        _jwtConfiguration = jwtConfiguration.Value;
    }


    public async Task<Result<AuthorizationResponse>> RegisterUserAndCreateToken(RegistrationRequest request, CancellationToken cancellationToken)
    {
        var registerUserResult = await RegisterUserAsync(request, cancellationToken);
        if (!registerUserResult.Succeeded)
        {
            var result = Result.Fail<AuthorizationResponse>(registerUserResult.Errors.Select(e => e.Description).Aggregate("", (f, s) => string.Join("", f, s)));

            return result;
        }

        var authRequest = new AuthorizationRequest
        {
            Login = request.Login,
            Password = request.Password
        };

        await ValidateUser(authRequest, cancellationToken);

        var response = await AuthenticateAsync();
        return Result.Ok(response);
    }

    public async Task<string> CreateTokenAsync()
    {
        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaims();
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }

    public async Task<IdentityResult> RegisterUserAsync(RegistrationRequest request, CancellationToken cancellationToken)
    {
        var newUser = new User()
        {
            UserName = request.Login,
            SecureQuestionId = request.SecurityQuestionId,
            SecureAnswer = request.SecurityQuestionAnswer
        };
        var result = await _userManager.CreateAsync(newUser, request.Password);

        if (result.Succeeded)
            await _userManager.AddToRoleAsync(newUser, UserRoles.User.ToString());

        return result;
    }

    public async Task<bool> ValidateUser(AuthorizationRequest authorizationRequest, CancellationToken cancellationToken)
    {
        _user = await _userManager.FindByNameAsync(authorizationRequest.Login);

        var result = (_user is not null && await _userManager.CheckPasswordAsync(_user, authorizationRequest.Password));

        if (!result)
            _logger.LogWarning($"{nameof(ValidateUser)}: Authentication failed. Wrong username or password");

        return result;
    }

    public async Task<AuthorizationResponse> AuthenticateAsync()
    {
        var tokenAsString = await CreateTokenAsync();

        var user = new UserDto
        {
            Id = _user.Id,
            Age = _user?.Age,
            Description = _user?.Description,
            FirstName = _user?.FirstName,
            LastName = _user?.LastName,
            Patronymic = _user?.Patronymic,
            Login = _user.UserName,
            Roles = await _userManager.GetRolesAsync(_user)
        };

        var response = new AuthorizationResponse
        {
            User = user,
            Token = tokenAsString
        };

        return response;
    }


    private SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(_jwtConfiguration.Secret);
        var secret = new SymmetricSecurityKey(key);
        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private async Task<List<Claim>> GetClaims()
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, _user.UserName)
        };

        var roles = await _userManager.GetRolesAsync(_user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return claims;
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var tokenOptions = new JwtSecurityToken(
            issuer: _jwtConfiguration.ValidIssuer,
            claims: claims,
            expires: DateTime.Now.AddSeconds(_jwtConfiguration.AccessTokenExpiration),
            signingCredentials: signingCredentials
            );
        return tokenOptions;
    }
}

