using carddata.domain.Services.Messaging;
using carddata.infrastructure.Services.Messaging.Cards;
using Microsoft.Extensions.DependencyInjection;

namespace carddata.infrastructure
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
            services.AddTransient<IYugiohCardQueue, YugiohCardQueue>();

            return services;
        }
    }
}