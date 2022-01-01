namespace APITest.Services;

using Models.DTOs;
using Repositories;

public interface IIGDBService
{
    Task<List<IGDBDTO>> GetListOfGames();
}

public class IGDBService : IIGDBService
{
    private const string BaseAddress = "https://api.igdb.com/v4/";
    private readonly IIGDBRepository _igdbRepository;

    public IGDBService(IIGDBRepository igdbRepository)
    {
        _igdbRepository = igdbRepository;
    }

    public async Task<List<IGDBDTO>> GetListOfGames()
    {
        List<IGDBDTO> result;
        var query =
            "query games \"PlayStation Games\" { fields name,platforms.name; where platforms !=n & platforms = {48}; limit 1; };";

        try
        {
            var response =
                await _igdbRepository.GetListOfGames("multiquery/", query);
            result = response;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.StackTrace);
        }

        return result;
    }

    // private async Task<List<GameDTO>> AddAgeRatingsToGames(List<GameDTO> games)
    // {
    //     var request = new HttpRequestMessage(HttpMethod.Post,
    //         $"{BaseAddress}age_ratings?fields=rating");
    //     request.Headers.Add("Accept", "application/json");
    //     request.Headers.Add("Client-ID", "1xa40lonqfbzfujewgeskcs9yz0yb4");
    //     request.Headers.Add("Authorization", "Bearer mxmm48twe58z5jbi3h0101uf0uh99s");
    //
    //     try
    //     {
    //         var response = await _client.SendAsync(request);
    //         await using var responseStream = await response.Content.ReadAsStreamAsync();
    //         var result = await JsonSerializer.DeserializeAsync<IEnumerable<string>>(responseStream);
    //         
    //         games = (result ?? Array.Empty<GameDTO>()).ToList();
    //     }
    //     catch (Exception ex)
    //     {
    //         throw new Exception(ex.StackTrace);
    //     }
    //
    //     return games;
    // }
}