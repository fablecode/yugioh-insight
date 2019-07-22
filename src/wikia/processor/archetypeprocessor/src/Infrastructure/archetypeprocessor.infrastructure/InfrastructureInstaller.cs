using archetypeprocessor.domain.Repository;
using archetypeprocessor.infrastructure.Database;
using archetypeprocessor.infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace archetypeprocessor.infrastructure
{
    public static class InfrastructureInstaller
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string connectionString)
        {
            services.AddYgoDatabase(connectionString)
                .AddRepositories();


            return services;
        }
        public static IServiceCollection AddYgoDatabase(this IServiceCollection services, string connectionString)
        {
            services.AddDbContextPool<YgoDbContext>(c => c.UseSqlServer(connectionString));

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IArchetypeRepository, ArchetypeRepository>();
            services.AddTransient<IArchetypeCardsRepository, ArchetypeCardsRepository>();

            return services;
        }
    }
}