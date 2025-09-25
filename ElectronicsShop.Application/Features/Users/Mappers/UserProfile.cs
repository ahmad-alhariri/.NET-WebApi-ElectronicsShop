using AutoMapper;
using ElectronicsShop.Application.Features.Users.Dtos;
using ElectronicsShop.Domain.Users;

namespace ElectronicsShop.Application.Features.Users.Mappers;

public class UserProfile:Profile
{
    public UserProfile()
    {
        CreateMap<User, UserResponse>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
    }
}