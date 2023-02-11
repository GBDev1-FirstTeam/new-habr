using System.Security.Claims;

namespace NewHabr.WebApi.Extensions;

public static class IdentityUserExtensions
{
    /// <exception cref="UnauthorizedAccessException"></exception>
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
