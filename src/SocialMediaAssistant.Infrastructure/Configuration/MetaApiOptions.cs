namespace SocialMediaAssistant.Infrastructure.Configuration;

public class MetaApiOptions
{
    public const string SectionName = "MetaApi";

    public string AppId { get; set; } = string.Empty;
    public string AppSecret { get; set; } = string.Empty;
    public string VerifyToken { get; set; } = string.Empty;
    public string PageAccessToken { get; set; } = string.Empty;
    public string GraphApiBaseUrl { get; set; } = "https://graph.facebook.com/v19.0";
}
