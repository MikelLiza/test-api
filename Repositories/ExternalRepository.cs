namespace APITest.Repositories;

using System.Text;
using System.Text.Json;

public interface IExternalRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetSingleEntity(string baseAddress, string method,
        Dictionary<string, string>? headers = null, string? fields = null, string? query = null);

    Task<List<TEntity>> GetListOfEntities(string baseAddress, string method,
        Dictionary<string, string>? headers = null, string? fields = null, string? query = null);
}

public class ExternalRepository<TEntity> : IExternalRepository<TEntity> where TEntity : class
{
    private readonly HttpClient _client;

    public ExternalRepository(IHttpClientFactory clientFactory)
    {
        _client = clientFactory.CreateClient();
    }

    public async Task<TEntity?> GetSingleEntity(string baseAddress, string method,
        Dictionary<string, string>? headers = null, string? fields = null, string? query = null)
    {
        TEntity? result;

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
            var deserializedResponse = await JsonSerializer.DeserializeAsync<TEntity>(responseStream);
            result = deserializedResponse;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.StackTrace);
        }

        return result;
    }

    public async Task<List<TEntity>> GetListOfEntities(string baseAddress, string method,
        Dictionary<string, string>? headers = null, string? fields = null, string? query = null)
    {
        List<TEntity> result;

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
            var deserializedResponse = await JsonSerializer.DeserializeAsync<IEnumerable<TEntity>>(responseStream);
            result = (deserializedResponse ?? Array.Empty<TEntity>()).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.StackTrace);
        }

        return result;
    }
}