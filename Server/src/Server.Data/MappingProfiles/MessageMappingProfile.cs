using AutoMapper;
using Server.Data.DTOs;
using Server.Data.Models;

namespace Server.Data.MappingProfiles
{
    public class MessageMappingProfile : Profile
    {
        public MessageMappingProfile()
        {
            CreateMap<Message, MessageDto>();
            CreateMap<MessageCreationDto, Message>();
        }
    }
}
