using System.Collections.Generic;
using System.Linq;

namespace archetypeprocessor.core.Models
{
    public class ArchetypeDataTaskResult<T>
    {
        public bool IsSuccessful => !Errors.Any();

        public T ArchetypeData { get; set; }

        public List<string> Errors { get; set; } = new List<string>();
    }
}