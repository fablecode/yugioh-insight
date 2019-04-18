using articledata.domain.Services.Messaging.Cards;
using articledata.infrastructure.Services.Messaging.Cards;
using Microsoft.Extensions.DependencyInjection;

namespace articledata.infrastructure
{
    public static class InfrastructureInstaller
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddMessagingServices();

            return services;
        }
        public static IServiceCollection AddMessagingServices(this IServiceCollection services)
        {
            services.AddTransient<ICardArticleQueue, CardArticleQueue>();

            return services;
        }
    }
}