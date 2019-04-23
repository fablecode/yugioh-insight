using System.IO;
using AutoMapper;
using cardprocessor.application.Dto;
using cardprocessor.application.Models.Cards.Input;
using cardprocessor.core.Models;
using cardprocessor.core.Models.Db;

namespace cardprocessor.application.Mappings.Profiles
{
    public class CardProfile : Profile
    {
        public CardProfile()
        {
            CreateMap<CardInputModel, CardModel>();

            CreateMap<Card, MonsterCardDto>()
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => $"/api/images/cards/{string.Concat(src.Name.Split(Path.GetInvalidFileNameChars()))}"));

            CreateMap<Card, SpellCardDto>()
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => $"/api/images/cards/{string.Concat(src.Name.Split(Path.GetInvalidFileNameChars()))}"));

            CreateMap<Card, TrapCardDto>()
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => $"/api/images/cards/{string.Concat(src.Name.Split(Path.GetInvalidFileNameChars()))}"));
        }
    }
}