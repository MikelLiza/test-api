namespace APITest.Repositories;

using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Models.DTOs;
using Services;
using Settings;

public interface IIGDBRepository
{
    Task<List<IGDBDTO>> GetListOfEntities(string baseAddress, string method,
        Dictionary<string, string>? headers = null, string? fields = null,
        string? query = null);

    Task<List<IGDBDTO>> GetListOfGames(string? fields = null,
        string? query = null);
}

public class IGDBRepository : IIGDBRepository
{
    private readonly ISettingsService _settingsService;
    private readonly IOptions<IGDBConfig> _options;
    private readonly HttpClient _client;

    public IGDBRepository(IHttpClientFactory clientFactory, ISettingsService settingsService,
        IOptions<IGDBConfig> options)
    {
        _settingsService = settingsService;
        _options = options;
        _client = clientFactory.CreateClient();
    }

    public async Task<IGDBDTO?> GetSingleEntity(string baseAddress, string method,
        Dictionary<string, string>? headers = null, string? fields = null,
        string? query = null)
    {
        IGDBDTO? result;

        try
        {
            if (headers != null)
                foreach (var key in headers.Keys)
                    _client.DefaultRequestHeaders.Add($"{key}", $"{headers[key]}");
            HttpResponseMessage response;
            if (method.Contains("Get"))
            {
                response = await _client.GetAsync($"{baseAddress}{fields}");
            }
            else
            {
                var content = new StringContent("");
                if (!string.IsNullOrWhiteSpace(query))
                    content = new StringContent(query, Encoding.Default,
                        "application/json");
                response = await _client.PostAsync($"{baseAddress}{fields}", content);
            }

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

    public async Task<List<IGDBDTO>> GetListOfEntities(string baseAddress, string method,
        Dictionary<string, string>? headers = null, string? fields = null,
        string? query = null)
    {
        List<IGDBDTO> result;

        try
        {
            if (headers != null)
                foreach (var key in headers.Keys)
                    _client.DefaultRequestHeaders.Add($"{key}", $"{headers[key]}");
            HttpResponseMessage response;
            if (method.Contains("Get"))
            {
                response = await _client.GetAsync($"{baseAddress}{fields}");
            }
            else
            {
                var content = new StringContent("");
                if (!string.IsNullOrWhiteSpace(query))
                    content = new StringContent(query, Encoding.Default,
                        "application/json");
                response = await _client.PostAsync($"{baseAddress}{fields}", content);
            }

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