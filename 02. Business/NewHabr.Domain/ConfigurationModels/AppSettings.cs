namespace NewHabr.Domain.ConfigurationModels;

public class AppSettings
{
    public int UserBanExpiresInDays { get; set; }
    public int AutoUnBanJobRunsEveryXMinutes { get; set; }

    public static string Section => "AppSettings";
}
