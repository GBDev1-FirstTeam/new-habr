using System;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using NewHabr.Domain.Models;

namespace NewHabr.WebApi.Extensions;

public static class IdentityUserExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        if (!user.HasClaim(claim => claim.Type == ClaimTypes.NameIdentifier))
            throw new UnauthorizedAccessException();

        string userIdAsString = user.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdAsString, out Guid userId))
            throw new UnauthorizedAccessException();

        return userId;
    }
}

