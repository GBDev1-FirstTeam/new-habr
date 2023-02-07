using AutoMapper;
using NewHabr.Domain.Dto;
using NewHabr.Domain.Models;

namespace NewHabr.Business.AutoMapperProfiles;

public class TagProfile : Profile
{
    public TagProfile()
    {
        CreateMap<Tag, TagDto>().ReverseMap();
        CreateMap<CreateTagRequest, Tag>();
        CreateMap<UpdateTagRequest, Tag>();
    }
}
