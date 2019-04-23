using System.Collections.Generic;
using System.Threading.Tasks;
using cardprocessor.core.Models.Db;

namespace cardprocessor.domain.Repository
{
    public interface ICategoryRepository
    {
        Task<List<Category>> AllCategories();
        Task<Category> CategoryById(int id);
        Task<Category> Add(Category category);
    }
}