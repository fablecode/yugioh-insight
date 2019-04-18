using article.domain.Services.Messaging.Cards;
using article.infrastructure.Services.Messaging.Cards;
using Microsoft.Extensions.DependencyInjection;

namespace article.infrastructure
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