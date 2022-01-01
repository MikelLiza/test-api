namespace APITest.Services;

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Models.DTOs;
using Models.Entities;
using Repositories;

public interface IItemsService
{
    Task<List<ItemDTO>> GetListOfAllItems();
    Task<ItemDTO> CreateOrUpdateItem(ItemDTO itemDTO);
}

public class ItemsService : IItemsService
{
    private readonly IBaseRepository<Item> _itemRepo;
    private readonly IMapper _mapper;

    public ItemsService(IMapper mapper, IBaseRepository<Item> itemRepo)
    {
        _mapper = mapper;
        _itemRepo = itemRepo;
    }

    public async Task<List<ItemDTO>> GetListOfAllItems()
    {
        var result = await _itemRepo.GetAll().ToListAsync();
        return _mapper.Map<List<ItemDTO>>(result);
    }

    public async Task<ItemDTO> CreateOrUpdateItem(ItemDTO itemDTO)
    {
        var item = _mapper.Map<Item>(itemDTO);

        var existing = _itemRepo.GetAll().ToList().FirstOrDefault(t => t.Id == itemDTO.Id);
        if (existing == null)
        {
            var created = await _itemRepo.CreateAndSaveAsync(item);
            return _mapper.Map<ItemDTO>(created);
        }

        var updated = await _itemRepo.UpdateAndSaveAsync(item);
        return _mapper.Map<ItemDTO>(updated);
    }
}