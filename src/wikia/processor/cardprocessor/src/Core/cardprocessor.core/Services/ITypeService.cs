using System.Collections.Generic;
using System.Threading.Tasks;
using cardprocessor.core.Models.Db;

namespace cardprocessor.core.Services
{
    public interface ITypeService
    {
        Task<List<Type>> AllTypes();
    }
}