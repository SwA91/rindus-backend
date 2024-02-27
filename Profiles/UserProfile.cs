using AutoMapper;
using RindusBackend.Dto.Users;
using RindusBackend.Models;

namespace RindusBackend.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserAddRequest, UserModel>();
        CreateMap<UserModel, UserAddRequest>();

        CreateMap<UserAddResponse, UserModel>();
        CreateMap<UserModel, UserAddResponse>();

        CreateMap<UserDataResponse, UserModel>();
        CreateMap<UserModel, UserDataResponse>();

        CreateMap<UserUpdateRequest, UserModel>();
        CreateMap<UserModel, UserUpdateRequest>();

        CreateMap<UserUpdateResponse, UserModel>();
        CreateMap<UserModel, UserUpdateResponse>();
    }
}