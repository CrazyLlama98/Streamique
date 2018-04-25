using AutoMapper;
using Server.Data.DTOs;
using Server.Data.Models;

namespace Server.Data.MappingsProfiles
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<RegistrationDto, User>()
                .ForMember(dest => dest.Email, opt => opt.ResolveUsing(src => src.Email))
                .ForMember(dest => dest.UserName, opt => opt.ResolveUsing(src => src.Email))
                .ForMember(dest => dest.Nickname, opt => opt.ResolveUsing(src => src.NickName))
                .ForMember(dest => dest.PhoneNumber, opt => opt.ResolveUsing(src => src.Phone))
                .ForMember(dest => dest.AccountImageUrl, opt => opt.ResolveUsing(src => $"/images/profile-{src.AccountImageId}.png"));
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Username, opt => opt.ResolveUsing(src => src.UserName))
                .ForMember(dest => dest.Phone, opt => opt.ResolveUsing(src => src.PhoneNumber));
        }
    }
}
