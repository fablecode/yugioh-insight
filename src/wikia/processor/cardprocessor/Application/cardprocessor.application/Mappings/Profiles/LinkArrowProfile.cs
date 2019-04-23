using AutoMapper;
using cardprocessor.application.Dto;
using cardprocessor.core.Models.Db;

namespace cardprocessor.application.Mappings.Profiles
{
    public class LinkArrowProfile : Profile
    {
        public LinkArrowProfile()
        {
            CreateMap<LinkArrow, LinkArrowDto>();
        }
    }
}