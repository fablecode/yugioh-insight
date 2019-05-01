using System.Collections.Generic;
using System.Threading.Tasks;
using cardprocessor.core.Models.Db;

namespace cardprocessor.domain.Repository
{
    public interface IAttributeRepository
    {
        Task<List<Attribute>> AllAttributes();
    }
}