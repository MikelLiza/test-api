namespace APITest.Services;

using System.Text.Json;
using Microsoft.Extensions.Options;
using Repositories;
using Settings;

public interface ISettingsService
{
    FirebaseConfig GetFirebaseConfig();
    Task<string> GetIGDBAuthToken();
}

public class SettingsService : ISettingsService
{
    private readonly IExternalRepository<object> _externalRepository;
    private readonly IOptions<FirebaseConfig> _fireBaseOptions;
    private readonly IOptions<TwitchConfig> _twitchOptions;

    public SettingsService(IOptions<TwitchConfig> twitchOptions, IExternalRepository<object> externalRepository,
        IOptions<FirebaseConfig> fireBaseOptions)
    {
        _twitchOptions = twitchOptions;
        _fireBaseOptions = fireBaseOptions;
        _externalRepository = externalRepository;
    }

    public FirebaseConfig GetFirebaseConfig()
    {
        return _fireBaseOptions.Value;
    }

    public async Task<string> GetIGDBAuthToken()
    {
        var response = await _externalRepository.GetSingleEntity($"{_twitchOptions.Value.BaseAddress}", "Post", null,
            $"?client_id={_twitchOptions.Value.ClientId}&client_secret={_twitchOptions.Value.ClientSecret}&grant_type=client_credentials");

        if (string.IsNullOrWhiteSpace(response?.ToString()))
            throw new Exception("Could not generate auth token.");

        return JsonSerializer.Deserialize<TokenInformationDTO>(response.ToString() ?? string.Empty)?.AccessToken ??
               string.Empty;
    }
}