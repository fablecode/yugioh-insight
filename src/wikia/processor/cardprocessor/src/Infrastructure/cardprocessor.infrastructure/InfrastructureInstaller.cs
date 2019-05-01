using cardprocessor.domain.Queues.Cards;
using cardprocessor.domain.Repository;
using cardprocessor.infrastructure.Database;
using cardprocessor.infrastructure.Repository;
using cardprocessor.infrastructure.Services.Messaging.Cards;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace cardprocessor.infrastructure
{
    public static class InfrastructureInstaller
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string connectionString)
        {
            services.AddMessagingServices();
            services.AddRepositories();
            services.AddYgoDatabase(connectionString);


            return services;
        }
        public static IServiceCollection AddYgoDatabase(this IServiceCollection services, string connectionString)
        {
            //services.AddDbContextPool<YgoDbContext>(c => c.UseSqlServer(connectionString));
            services.AddDbContext<YgoDbContext>(c => c.UseSqlServer(connectionString), ServiceLifetime.Transient);

            return services;
        }

        public static IServiceCollection AddMessagingServices(this IServiceCollection services)
        {
            services.AddTransient<ICardImageQueue, CardImageQueue>();

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<ICardRepository, CardRepository>();
            services.AddTransient<ISubCategoryRepository, SubCategoryRepository>();
            services.AddTransient<ITypeRepository, TypeRepository>();
            services.AddTransient<ILinkArrowRepository, LinkArrowRepository>();
            services.AddTransient<IAttributeRepository, AttributeRepository>();
            services.AddTransient<ILimitRepository, LimitRepository>();

            return services;
        }

    }
}