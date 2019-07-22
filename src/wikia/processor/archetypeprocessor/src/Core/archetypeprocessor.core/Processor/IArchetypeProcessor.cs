using System.Threading.Tasks;
using archetypeprocessor.core.Models;

namespace archetypeprocessor.core.Processor
{
    public interface IArchetypeProcessor
    {
        Task<ArchetypeDataTaskResult<ArchetypeMessage>> Process(ArchetypeMessage archetypeData);
    }
}