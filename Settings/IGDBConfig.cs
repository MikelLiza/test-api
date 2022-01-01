namespace APITest.Settings;

using System.Text.Json.Serialization;

public class IGDBConfig
{
    public string BaseAddress = "https://api.igdb.com/v4/";
    public string ClientId { get; set; }
}

public class TokenInformationDTO
{
    [JsonPropertyName("access_token")] public string AccessToken { get; set; }
    [JsonPropertyName("expires_in")] public int ExpiresIn { get; set; }
}