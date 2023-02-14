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
            .ReverseMap();

        CreateMap<RegistrationRequest, User>()
            .ForMember(dest => dest.SecureAnswer, opt => opt.MapFrom(src => src.SecurityQuestionAnswer))
            .ForMember(dest => dest.SecureQuestionId, opt => opt.MapFrom(src => src.SecurityQuestionId));

        CreateMap<UserForManipulationDto, User>();


        CreateMap<long?, DateTime?>()
            .ConstructUsing((src, dst) => src.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(src.Value).UtcDateTime : null);
        CreateMap<DateTime?, long?>()
            .ConstructUsing((src, dst) => src.HasValue ? new DateTimeOffset(src.Value).ToUnixTimeMilliseconds() : null);

        CreateMap<long, DateTimeOffset>()
            .ConstructUsing((src, dst) => DateTimeOffset.FromUnixTimeMilliseconds(src));
        CreateMap<DateTimeOffset, long>()
            .ConstructUsing((src, dst) => src.ToUnixTimeMilliseconds());


        CreateMap<UserArticle, UserArticleDto>();

        CreateMap<User, UserProfileDto>();

        CreateMap<RegistrationRequest, AuthorizationRequest>();

        CreateMap<SecureQuestionCreateRequest, SecureQuestion>();

        CreateMap<SecureQuestion, SecureQuestionDto>().ReverseMap();
        CreateMap<SecureQuestionUpdateRequest, SecureQuestion>();

        CreateMap<NotificationCreateRequest, Notification>();
        CreateMap<Notification, NotificationDto>()
            .ReverseMap();

        CreateMap<UserComment, UserCommentDto>();

        CreateMap<UserLikedArticle, LikedArticleDto>();
        CreateMap<UserLikedComment, LikedCommentDto>();
        CreateMap<UserLikedUser, LikedUserDto>();
    }
}
