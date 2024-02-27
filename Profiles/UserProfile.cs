using AutoMapper;
using RindusBackend.Dto.Users;
using RindusBackend.Models;

namespace RindusBackend.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserModel, UserDataResponse>();
        CreateMap<UserDataResponse, UserModel>();
    }
}