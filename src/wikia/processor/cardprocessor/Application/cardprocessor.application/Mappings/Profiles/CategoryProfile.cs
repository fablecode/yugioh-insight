using AutoMapper;
using cardprocessor.application.Dto;
using cardprocessor.core.Models.Db;

namespace cardprocessor.application.Mappings.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>();
        }
    }
}