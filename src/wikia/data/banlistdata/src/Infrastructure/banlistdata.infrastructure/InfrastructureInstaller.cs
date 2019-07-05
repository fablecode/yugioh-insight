using banlistdata.domain.Services.Messaging;
using banlistdata.infrastructure.Services.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace banlistdata.infrastructure
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
            services.AddTransient<IBanlistDataQueue, BanlistDataQueue>();

            return services;
        }
    }
}