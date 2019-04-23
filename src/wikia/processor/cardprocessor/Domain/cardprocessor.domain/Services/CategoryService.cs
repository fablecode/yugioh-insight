using System.Collections.Generic;
using System.Threading.Tasks;
using cardprocessor.core.Models.Db;
using cardprocessor.core.Services;
using cardprocessor.domain.Repository;

namespace cardprocessor.domain.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public Task<List<Category>> AllCategories()
        {
            return _categoryRepository.AllCategories();
        }

        public Task<Category> CategoryById(int id)
        {
            return _categoryRepository.CategoryById(id);
        }

        public Task<Category> Add(Category category)
        {
            return _categoryRepository.Add(category);
        }
    }
}