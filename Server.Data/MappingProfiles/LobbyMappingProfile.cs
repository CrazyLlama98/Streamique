using AutoMapper;
using Server.Data.DTOs;
using Server.Data.Models;

namespace Server.Data.MappingProfiles
{
    public class LobbyMappingProfile : Profile
    {
        public LobbyMappingProfile()
        {
            CreateMap<LobbyJoinRequest, LobbyJoinRequestDto>();
            CreateMap<Lobby, LobbyDto>();
            CreateMap<LobbyCreationDto, Lobby>();
            CreateMap<LobbyUpdateDto, Lobby>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
