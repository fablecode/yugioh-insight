using System;
using System.Threading.Tasks;
using AutoMapper;
using banlistprocessor.core.Models;
using banlistprocessor.core.Models.Db;
using banlistprocessor.core.Services;
using banlistprocessor.domain.Repository;

namespace banlistprocessor.domain.Services
{
    public class BanlistService : IBanlistService
    {
        private readonly IBanlistCardService _banlistCardService;
        private readonly IBanlistRepository _banlistRepository;
        private readonly IFormatRepository _formatRepository;
        private readonly IMapper _mapper;

        public BanlistService
        (
            IBanlistCardService banlistCardService,
            IBanlistRepository banlistRepository, 
            IFormatRepository formatRepository,
            IMapper mapper
        )
        {
            _banlistCardService = banlistCardService;
            _banlistRepository = banlistRepository;
            _formatRepository = formatRepository;
            _mapper = mapper;
        }
        public Task<bool> BanlistExist(int id)
        {
            return _banlistRepository.BanlistExist(id);
        }

        public async Task<Banlist> Add(YugiohBanlist yugiohBanlist)
        {
            var format = await _formatRepository.FormatByAcronym(yugiohBanlist.BanlistType.ToString());

            var newBanlist = _mapper.Map<Banlist>(yugiohBanlist);
            newBanlist.Format = format ?? throw new ArgumentException($"Format with acronym '{yugiohBanlist.BanlistType.ToString()}' not found.");

            newBanlist = await _banlistRepository.Add(newBanlist);
            newBanlist.BanlistCard = await _banlistCardService.Update(newBanlist.Id, yugiohBanlist.Sections);

            return newBanlist;
        }

        public async Task<Banlist> Update(YugiohBanlist yugiohBanlist)
        {
            var banlistToupdate = await _banlistRepository.GetBanlistById(yugiohBanlist.ArticleId);
            var format = await _formatRepository.FormatByAcronym(yugiohBanlist.BanlistType.ToString());

            banlistToupdate.Format = format ?? throw new ArgumentException($"Format with acronym '{yugiohBanlist.BanlistType.ToString()}' not found."); ;
            banlistToupdate.Name = yugiohBanlist.Title;
            banlistToupdate.ReleaseDate = yugiohBanlist.StartDate;
            banlistToupdate.Updated = DateTime.UtcNow;

            banlistToupdate = await _banlistRepository.Update(banlistToupdate);
            banlistToupdate.BanlistCard = await _banlistCardService.Update(banlistToupdate.Id, yugiohBanlist.Sections);

            return banlistToupdate;
        }
    }
}