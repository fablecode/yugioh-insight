using AutoMapper;
using cardprocessor.application.Dto;
using cardprocessor.core.Models.Db;

namespace cardprocessor.application.Mappings.Profiles
{
    public class TypeProfile : Profile
    {
        public TypeProfile()
        {
            CreateMap<Type, TypeDto>();
        }
    }
}