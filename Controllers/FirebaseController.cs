namespace APITest.Controllers;

using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using Repositories;

[Route("api/[controller]")]
[ApiController]
public class FirebaseController : ControllerBase
{
    private readonly IFirebaseRepository _firebaseRepository;

    public FirebaseController(IFirebaseRepository firebaseRepository)
    {
        _firebaseRepository = firebaseRepository;
    }

    [HttpGet("GetListOfUsers")]
    public async Task<ActionResult<List<FirebaseUserDTO>>> GetListOfUsers()
    {
        return await _firebaseRepository.GetAllUsers();
    }

    [HttpPost("PostUser")]
    public async Task<ActionResult<FirebaseUserDTO>> PostUser(FirebaseUserDTO newUser)
    {
        return await _firebaseRepository.PostUser(newUser);
    }

    [HttpPost("Login")]
    public async Task<ActionResult<FirebaseUserDTO>> Login(FirebaseUserDTO request)
    {
        return await _firebaseRepository.Login(request);
    }
}