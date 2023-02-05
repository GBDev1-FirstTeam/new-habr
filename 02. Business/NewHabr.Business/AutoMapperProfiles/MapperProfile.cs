using AutoMapper;
using NewHabr.Domain.Dto;
using NewHabr.Domain.Models;
using Serilog.Parsing;

namespace NewHabr.Business.AutoMapperProfiles;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Login, opt => opt.MapFrom(src => src.UserName))
            .ReverseMap();

        CreateMap<RegistrationRequest, User>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Login))
            .ForMember(dest => dest.SecureAnswer, opt => opt.MapFrom(src => src.SecurityQuestionAnswer))
            .ForMember(dest => dest.SecureQuestionId, opt => opt.MapFrom(src => src.SecurityQuestionId));

        CreateMap<RegistrationRequest, AuthorizationRequest>();
    }
}
