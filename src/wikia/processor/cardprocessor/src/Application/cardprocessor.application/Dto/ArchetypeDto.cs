using System.Collections.Generic;

namespace cardprocessor.application.Dto
{
    public class ArchetypeDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }

        public List<ArchetypeCardDto> Cards { get; set; } = new List<ArchetypeCardDto>();
    }
}