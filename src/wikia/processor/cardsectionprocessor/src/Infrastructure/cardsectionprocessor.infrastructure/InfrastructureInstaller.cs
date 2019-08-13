using cardsectionprocessor.domain.Repository;
using cardsectionprocessor.infrastructure.Database;
using cardsectionprocessor.infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace cardsectionprocessor.infrastructure
{
    public static class InfrastructureInstaller
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string connectionString)
        {
            services.AddYgoDatabase(connectionString);
            services.AddRepositories();

            return services;
        }

        public static IServiceCollection AddYgoDatabase(this IServiceCollection services, string connectionString)
        {
            services.AddDbContextPool<YgoDbContext>(c => c.UseSqlServer(connectionString));
            //services.AddDbContext<YgoDbContext>(c => c.UseSqlServer(connectionString), ServiceLifetime.Transient);

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<ICardRepository, CardRepository>();
            services.AddTransient<ICardTipRepository, CardTipRepository>();
            services.AddTransient<ICardRulingRepository, CardRulingRepository>();
            services.AddTransient<ICardTriviaRepository, CardTriviaRepository>();

            return services;
        }
    }
}