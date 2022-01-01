namespace APITest.Settings;

public class TwitchConfig
{
    public string BaseAddress = "https://id.twitch.tv/oauth2/token";
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
}