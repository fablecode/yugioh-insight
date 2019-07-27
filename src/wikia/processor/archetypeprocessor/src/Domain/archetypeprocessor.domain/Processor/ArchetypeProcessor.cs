using System;
using System.Threading.Tasks;
using archetypeprocessor.core.Models;
using archetypeprocessor.core.Models.Db;
using archetypeprocessor.core.Processor;
using archetypeprocessor.core.Services;
using archetypeprocessor.domain.Messaging;

namespace archetypeprocessor.domain.Processor
{
    public class ArchetypeProcessor : IArchetypeProcessor
    {
        private readonly IArchetypeService _archetypeService;
        private readonly IArchetypeImageQueueService _archetypeImageQueueService;

        public ArchetypeProcessor(IArchetypeService archetypeService, IArchetypeImageQueueService archetypeImageQueueService)
        {
            _archetypeService = archetypeService;
            _archetypeImageQueueService = archetypeImageQueueService;
        }

        public async Task<ArchetypeDataTaskResult<ArchetypeMessage>> Process(ArchetypeMessage archetypeData)
        {
            var articleDataTaskResult = new ArchetypeDataTaskResult<ArchetypeMessage>
            {
                ArchetypeData = archetypeData
            };

            var existingArchetype = await _archetypeService.ArchetypeById(archetypeData.Id);

            if (existingArchetype == null)
            {
                var newArchetype = new Archetype
                {
                    Id = archetypeData.Id,
                    Name = archetypeData.Name,
                    Url = archetypeData.ProfileUrl,
                    Created = DateTime.UtcNow,
                    Updated = archetypeData.Revision
                };

                await _archetypeService.Add(newArchetype);
            }
            else
            {
                existingArchetype.Name = archetypeData.Name;
                existingArchetype.Url = archetypeData.ProfileUrl;
                existingArchetype.Updated = archetypeData.Revision;

                await _archetypeService.Update(existingArchetype);
            }

            if (!string.IsNullOrWhiteSpace(archetypeData.ImageUrl))
            {
                var downloadImage = new DownloadImage
                {
                    RemoteImageUrl = new Uri(archetypeData.ImageUrl),
                    ImageFileName = archetypeData.Id.ToString(),
                };

                await _archetypeImageQueueService.Publish(downloadImage);
            }

            return articleDataTaskResult;
        }
    }
}