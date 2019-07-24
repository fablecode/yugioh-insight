using archetypeprocessor.domain.Messaging;
using archetypeprocessor.domain.Repository;
using archetypeprocessor.infrastructure.Database;
using archetypeprocessor.infrastructure.Messaging;
using archetypeprocessor.infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace archetypeprocessor.infrastructure
{
    public static class InfrastructureInstaller
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string connectionString)
        {
            services
                .AddYgoDatabase(connectionString)
                .AddRepositories()
                .AddMessaging();


            return services;
        }
        public static IServiceCollection AddYgoDatabase(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<YgoDbContext>(c => c.UseSqlServer(connectionString), ServiceLifetime.Transient);

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IArchetypeRepository, ArchetypeRepository>();
            services.AddTransient<IArchetypeCardsRepository, ArchetypeCardsRepository>();

            return services;
        }
        public static IServiceCollection AddMessaging(this IServiceCollection services)
        {
            services.AddTransient<IImageQueueService, ImageQueueService>();
            services.AddTransient<IArchetypeImageQueueService, ArchetypeImageQueueService>();

            return services;
        }
    }
}