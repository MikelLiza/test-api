namespace APITest.Services;

using System.Text.Json;
using Microsoft.Extensions.Options;
using Repositories;
using Settings;

public interface ISettingsService
{
    Task<string> GetIGDBAuthToken();
}

public class SettingsService : ISettingsService
{
    private readonly IExternalRepository<object> _externalRepository;
    private readonly IOptions<TwitchConfig> _options;

    public SettingsService(IOptions<TwitchConfig> options, IExternalRepository<object> externalRepository)
    {
        _options = options;
        _externalRepository = externalRepository;
    }

    public async Task<string> GetIGDBAuthToken()
    {
        var response = await _externalRepository.GetSingleEntity($"{_options.Value.BaseAddress}", "Post", null,
            $"?client_id={_options.Value.ClientId}&client_secret={_options.Value.ClientSecret}&grant_type=client_credentials");

        if (string.IsNullOrWhiteSpace(response?.ToString()))
            throw new Exception("Could not generate auth token.");

        return JsonSerializer.Deserialize<TokenInformationDTO>(response.ToString() ?? string.Empty)?.AccessToken ??
               string.Empty;
    }
}