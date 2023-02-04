#nullable disable

using System;
namespace NewHabr.Domain.ConfigurationModels;

public class JwtConfiguration
{
    public string? ValidIssuer { get; set; }
    public int AccessTokenExpiration { get; set; }
    public string Secret { get; set; }

    public static string Section => "JwtSettings";
}
