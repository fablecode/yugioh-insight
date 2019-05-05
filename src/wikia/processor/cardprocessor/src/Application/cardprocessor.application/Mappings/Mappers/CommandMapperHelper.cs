using AutoMapper;
using cardprocessor.application.Dto;
using cardprocessor.application.Enums;
using cardprocessor.core.Models.Db;
using System;

namespace cardprocessor.application.Mappings.Mappers
{
    public static class CommandMapperHelper
    {
        public static object MapCardByCardType(IMapper mapper, YgoCardType cardCardType, Card cardUpdated)
        {
            switch (cardCardType)
            {
                case YgoCardType.Monster:
                    return mapper.Map<MonsterCardDto>(cardUpdated);
                case YgoCardType.Spell:
                    return mapper.Map<SpellCardDto>(cardUpdated);
                case YgoCardType.Trap:
                    return mapper.Map<TrapCardDto>(cardUpdated);
                default:
                    throw new ArgumentOutOfRangeException(nameof(cardCardType), cardCardType, null);
            }
        }

    }
}