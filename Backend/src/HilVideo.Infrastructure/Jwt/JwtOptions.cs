using Microsoft.Extensions.Configuration;

namespace AuthService.Infrastructure;

public class JwtOptions
{
    public string SecretKey { get; set; } = String.Empty;
    public int ExpitesHours { get; set; } = 12;
    public string Audience { get; set; } = String.Empty;
    public string Issuer { get; set; } = String.Empty;

}
