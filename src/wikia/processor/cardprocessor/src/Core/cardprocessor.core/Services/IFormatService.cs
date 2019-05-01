using System.Threading.Tasks;
using cardprocessor.core.Models.Db;

namespace cardprocessor.core.Services
{
    public interface IFormatService
    {
        Task<Format> FormatByAcronym(string acronym);
    }
}