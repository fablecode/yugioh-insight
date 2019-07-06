using AutoMapper;
using banlistprocessor.core.Models;
using banlistprocessor.core.Models.Db;

namespace banlistprocessor.domain.Mappings.Profiles
{
    public class BanlistProfile : Profile
    {
        public BanlistProfile()
        {
            CreateMap<YugiohBanlist, Banlist>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ArticleId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.ReleaseDate, opt => opt.MapFrom(src => src.StartDate));
        }
    }
}