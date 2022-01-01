namespace APITest.Controllers;

using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using Services;

[Route("api/[controller]")]
[ApiController]
public class IGDBController : ControllerBase
{
    private readonly IIGDBService _igdbService;

    public IGDBController(IIGDBService igdbService)
    {
        _igdbService = igdbService;
    }

    [HttpGet("GetListOfGames")]
    public async Task<ActionResult<List<IGDBDTO>>> GetListOfGames()
    {
        return await _igdbService.GetListOfGames();
    }
}