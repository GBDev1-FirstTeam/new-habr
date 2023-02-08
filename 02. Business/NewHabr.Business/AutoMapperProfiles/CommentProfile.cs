using AutoMapper;
using NewHabr.Domain.Dto;
using NewHabr.Domain.Models;

namespace NewHabr.Business.AutoMapperProfiles;

public class CommentProfile : Profile
{
    public CommentProfile()
    {
        CreateMap<Comment, CommentDto>()
            .ForMember(dest => dest.CreatedAt, e => e.MapFrom(src => src.CreatedAt.ToUnixTimeMilliseconds()))
            .ForMember(dest => dest.ModifiedAt, e => e.MapFrom(src => src.ModifiedAt.ToUnixTimeMilliseconds()));

        CreateMap<CommentDto, Comment>()
            .ForMember(dest => dest.CreatedAt, e => e.MapFrom(src => new DateTimeOffset(new DateTime(src.CreatedAt))))
            .ForMember(dest => dest.ModifiedAt, e => e.MapFrom(src => new DateTimeOffset(new DateTime(src.ModifiedAt))));

        CreateMap<CreateCommentRequest, Comment>();
        CreateMap<UpdateCommentRequest, Comment>();
    }
}
