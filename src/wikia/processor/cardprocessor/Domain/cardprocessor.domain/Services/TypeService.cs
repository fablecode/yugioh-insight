using System.Collections.Generic;
using System.Threading.Tasks;
using cardprocessor.core.Models.Db;
using cardprocessor.core.Services;
using cardprocessor.domain.Repository;

namespace cardprocessor.domain.Services
{
    public class TypeService : ITypeService
    {
        private readonly ITypeRepository _typeRepository;

        public TypeService(ITypeRepository typeRepository)
        {
            _typeRepository = typeRepository;
        }
        public Task<List<Type>> AllTypes()
        {
            return _typeRepository.AllTypes();
        }
    }
}