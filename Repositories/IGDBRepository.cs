namespace APITest.Repositories;

using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Models.DTOs;
using Services;
using Settings;

public interface IIGDBRepository
{
    Task<IGDBDTO?> GetSingleGame(string? fields = null,
        string? query = null);

    Task<List<IGDBDTO>> GetListOfGames(string? fields = null,
        string? query = null);
}

public class IGDBRepository : IIGDBRepository
{
    private readonly HttpClient _client;
    private readonly IOptions<IGDBConfig> _options;
    private readonly ISettingsService _settingsService;

    public IGDBRepository(IHttpClientFactory clientFactory, ISettingsService settingsService,
        IOptions<IGDBConfig> options)
    {
        _settingsService = settingsService;
        _options = options;
        _client = clientFactory.CreateClient();
    }

    public async Task<IGDBDTO?> GetSingleGame(string? fields = null,
        string? query = null)
    {
        IGDBDTO? result;

        try
        {
            var authToken = await _settingsService.GetIGDBAuthToken();
            _client.DefaultRequestHeaders.Add("Client-ID", $"{_options.Value.ClientId}");
            _client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer {authToken}");
            var content = new StringContent("");
            if (!string.IsNullOrWhiteSpace(query))
                content = new StringContent(query, Encoding.Default,
                    "application/json");
            var response = await _client.PostAsync($"{_options.Value.BaseAddress}{fields}", content);

            await using var responseStream = await response.Content.ReadAsStreamAsync();
            var deserializedResponse = await JsonSerializer.DeserializeAsync<IGDBDTO>(responseStream);
            result = deserializedResponse;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.StackTrace);
        }

        return result;
    }

    public async Task<List<IGDBDTO>> GetListOfGames(string? fields = null,
        string? query = null)
    {
        List<IGDBDTO> result;

        try
        {
            var authToken = await _settingsService.GetIGDBAuthToken();
            _client.DefaultRequestHeaders.Add("Client-ID", $"{_options.Value.ClientId}");
            _client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer {authToken}");
            var content = new StringContent("");
            if (!string.IsNullOrWhiteSpace(query))
                content = new StringContent(query, Encoding.Default,
                    "application/json");
            var response = await _client.PostAsync($"{_options.Value.BaseAddress}{fields}", content);

            await using var responseStream = await response.Content.ReadAsStreamAsync();
            var deserializedResponse = await JsonSerializer.DeserializeAsync<IEnumerable<IGDBDTO>>(responseStream);
            result = (deserializedResponse ?? Array.Empty<IGDBDTO>()).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.StackTrace);
        }

        return result;
    }
}