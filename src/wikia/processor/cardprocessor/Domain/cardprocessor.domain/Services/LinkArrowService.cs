using System.Collections.Generic;
using System.Threading.Tasks;
using cardprocessor.core.Models.Db;
using cardprocessor.core.Services;
using cardprocessor.domain.Repository;

namespace cardprocessor.domain.Services
{
    public class LinkArrowService : ILinkArrowService
    {
        private readonly ILinkArrowRepository _linkArrowRepository;

        public LinkArrowService(ILinkArrowRepository linkArrowRepository)
        {
            _linkArrowRepository = linkArrowRepository;
        }
        public Task<List<LinkArrow>> AllLinkArrows()
        {
            return _linkArrowRepository.AllLinkArrows();
        }
    }
}