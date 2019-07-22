using System.Threading.Tasks;
using archetypeprocessor.core.Models;

namespace archetypeprocessor.core.Processor
{
    public interface IArchetypeCardProcessor
    {
        Task<ArticleDataTaskResult> Process(ArchetypeCardMessage archetypeData);
    }
}