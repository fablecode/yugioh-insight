using AutoMapper;
using cardprocessor.application.Dto;
using cardprocessor.core.Models.Db;

namespace cardprocessor.application.Mappings.Profiles
{
    public class CardSubCategoryProfile : Profile
    {
        public CardSubCategoryProfile()
        {
            CreateMap<CardSubCategory, CardSubCategoryDto>();
        }
    }
}