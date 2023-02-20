using AutoMapper;
using NewHabr.Domain.Dto;
using NewHabr.Domain.Models;

namespace NewHabr.Business.AutoMapperProfiles;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CategoryDto>().ReverseMap();
        CreateMap<CategoryCreateRequest, Category>();
        CreateMap<CategoryUpdateRequest, Category>();

        CreateMap<CommentExt, CommentDto>();
    }
}
