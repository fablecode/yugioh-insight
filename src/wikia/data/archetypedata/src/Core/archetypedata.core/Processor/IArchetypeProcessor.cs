using System.Threading.Tasks;
using archetypedata.core.Models;

namespace archetypedata.core.Processor
{
    public interface IArchetypeProcessor
    {
        Task<ArticleTaskResult> Process(Article article);
    }
}