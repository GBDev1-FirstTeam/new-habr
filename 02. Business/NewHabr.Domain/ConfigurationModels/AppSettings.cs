namespace NewHabr.Domain.ConfigurationModels;

public class AppSettings
{
    public int UserBanExpiresInDays { get; set; }

    public static string Section => "JwtSettings";
}
