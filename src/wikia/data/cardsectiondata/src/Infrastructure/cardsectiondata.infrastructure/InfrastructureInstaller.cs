using cardsectiondata.domain.Services.Messaging;
using cardsectiondata.infrastructure.Services.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace cardsectiondata.infrastructure
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
            services.AddTransient<IQueue, CardTipsQueue>();
            services.AddTransient<IQueue, CardRulingQueue>();
            services.AddTransient<IQueue, CardTriviaQueue>();

            return services;
        }
    }
}