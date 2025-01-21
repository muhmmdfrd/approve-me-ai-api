namespace ApproveMe.Core.Settings;

public class JwtConfigs
{
    public string TokenSecret { get; set; } = null!;
    public int TokenLifeTimes { get; set; }
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
}