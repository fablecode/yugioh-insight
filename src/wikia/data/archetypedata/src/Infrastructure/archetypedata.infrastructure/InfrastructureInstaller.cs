using archetypedata.domain.WebPages;
using archetypedata.infrastructure.WebPages;
using Microsoft.Extensions.DependencyInjection;

namespace archetypedata.infrastructure
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
            services.AddTransient<IHtmlWebPage, HtmlWebPage>();

            services.AddTransient<IArchetypeThumbnail, ArchetypeThumbnail>();
            services.AddTransient<IArchetypeWebPage, IArchetypeWebPage>();

            return services;
        }
    }
}