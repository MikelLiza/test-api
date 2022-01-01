namespace APITest.Services;

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Models.DTOs;
using Models.Entities;
using Repositories;

public interface IUsersService
{
    Task<List<UserDTO>> GetListOfAllUsers();
    Task<UserDTO> CreateOrUpdateUser(UserDTO userDTO);
}

public class UsersService : IUsersService
{
    private readonly IMapper _mapper;
    private readonly IBaseRepository<User> _userRepo;

    public UsersService(IMapper mapper, IBaseRepository<User> userRepo)
    {
        _mapper = mapper;
        _userRepo = userRepo;
    }

    public async Task<List<UserDTO>> GetListOfAllUsers()
    {
        var result = await _userRepo.GetAll().ToListAsync();
        return _mapper.Map<List<UserDTO>>(result);
    }

    public async Task<UserDTO> CreateOrUpdateUser(UserDTO userDTO)
    {
        var user = _mapper.Map<User>(userDTO);

        var existing = _userRepo.GetAll().ToList().FirstOrDefault(u => u.Id == userDTO.Id);
        if (existing == null)
        {
            var created = await _userRepo.CreateAndSaveAsync(user);
            return _mapper.Map<UserDTO>(created);
        }

        var updated = await _userRepo.UpdateAndSaveAsync(user);
        return _mapper.Map<UserDTO>(updated);
    }
}