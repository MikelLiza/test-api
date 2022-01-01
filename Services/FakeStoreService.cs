namespace APITest.Services;

using Models.DTOs;
using Repositories;

public interface IFakeStoreService
{
    Task<List<FakeStoreDTO>> GetListOfFakeStoreItems();
}

public class FakeStoreService : IFakeStoreService
{
    private const string BaseAddress = "https://fakestoreapi.com/products/";
    private readonly IExternalRepository<FakeStoreDTO> _externalRepository;

    public FakeStoreService(IExternalRepository<FakeStoreDTO> externalRepository)
    {
        _externalRepository = externalRepository;
    }

    public async Task<List<FakeStoreDTO>> GetListOfFakeStoreItems()
    {
        List<FakeStoreDTO> result;

        try
        {
            var response = await _externalRepository.GetListOfEntities(BaseAddress, "Get");
            result = response;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.StackTrace);
        }

        return result;
    }
}