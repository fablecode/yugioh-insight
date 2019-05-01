using AutoMapper;
using cardprocessor.application.Dto;
using cardprocessor.core.Models.Db;

namespace cardprocessor.application.Mappings.Profiles
{
    public class LimitProfile : Profile
    {
        public LimitProfile()
        {
            CreateMap<Limit, LimitDto>();
        }
    }
}