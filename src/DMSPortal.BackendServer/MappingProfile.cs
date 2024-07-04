using AutoMapper;
using DMSPortal.BackendServer.Data.Entities;
using DMSPortal.Models.DTOs;

namespace DMSPortal.BackendServer;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>().ReverseMap();
    }
}