using System.Collections.Generic;
using System.Threading.Tasks;
using cardprocessor.core.Models.Db;
using cardprocessor.core.Services;
using cardprocessor.domain.Repository;

namespace cardprocessor.domain.Services
{
    public class SubCategoryService : ISubCategoryService
    {
        private readonly ISubCategoryRepository _subCategoryRepository;

        public SubCategoryService(ISubCategoryRepository subCategoryRepository)
        {
            _subCategoryRepository = subCategoryRepository;
        }
        public Task<List<SubCategory>> AllSubCategories()
        {
            return _subCategoryRepository.AllSubCategories();
        }
    }
}