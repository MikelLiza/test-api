namespace APITest;

using AutoMapper;
using Models.DTOs;
using Models.Entities;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Item, ItemDTO>();
        CreateMap<ItemDTO, Item>();

        CreateMap<User, UserDTO>();
        CreateMap<UserDTO, User>();
    }
}