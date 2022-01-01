namespace APITest.Controllers;

using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using Services;

[Route("api/[controller]")]
[ApiController]
public class ItemsController : ControllerBase
{
    private readonly IFakeStoreService _fakeStoreService;
    private readonly IItemsService _itemsService;

    public ItemsController(IItemsService itemsService, IFakeStoreService fakeStoreService)
    {
        _itemsService = itemsService;
        _fakeStoreService = fakeStoreService;
    }

    [HttpGet("GetListOfAllItems")]
    public async Task<ActionResult<List<ItemDTO>>> GetListOfAllItems()
    {
        return await _itemsService.GetListOfAllItems();
    }

    [HttpGet("GetListOfFakeStoreItems")]
    public async Task<ActionResult<List<FakeStoreDTO>>> GetListOfFakeStoreItems()
    {
        return await _fakeStoreService.GetListOfFakeStoreItems();
    }

    [HttpPost("CreateOrUpdateItem")]
    public async Task<ActionResult<ItemDTO>> CreateOrUpdateItem(ItemDTO itemDTO)
    {
        return await _itemsService.CreateOrUpdateItem(itemDTO);
    }
}