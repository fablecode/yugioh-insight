using banlistprocessor.domain.Repository;
using banlistprocessor.infrastructure.Database;
using banlistprocessor.infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace banlistprocessor.infrastructure
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
            services.AddDbContext<YgoDbContext>(c => c.UseSqlServer(connectionString), ServiceLifetime.Transient);

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IBanlistCardRepository, BanlistCardRepository>();
            services.AddTransient<IBanlistRepository, BanlistRepository>();
            services.AddTransient<ICardRepository, CardRepository>();
            services.AddTransient<IFormatRepository, FormatRepository>();
            services.AddTransient<ILimitRepository, LimitRepository>();

            return services;
        }

    }
}