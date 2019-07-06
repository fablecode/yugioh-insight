using System.Threading.Tasks;
using banlistprocessor.core.Models.Db;

namespace banlistprocessor.domain.Repository
{
    public interface IFormatRepository
    {
        Task<Format> FormatByAcronym(string acronym);
    }
}