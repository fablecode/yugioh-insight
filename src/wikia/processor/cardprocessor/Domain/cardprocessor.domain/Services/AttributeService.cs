using System.Collections.Generic;
using System.Threading.Tasks;
using cardprocessor.core.Models.Db;
using cardprocessor.core.Services;
using cardprocessor.domain.Repository;

namespace cardprocessor.domain.Services
{
    public class AttributeService : IAttributeService
    {
        private readonly IAttributeRepository _attributeRepository;

        public AttributeService(IAttributeRepository attributeRepository)
        {
            _attributeRepository = attributeRepository;
        }
        public Task<List<Attribute>> AllAttributes()
        {
            return _attributeRepository.AllAttributes();
        }
    }
}