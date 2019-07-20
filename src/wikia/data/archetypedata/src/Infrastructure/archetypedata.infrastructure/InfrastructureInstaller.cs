using archetypedata.core.Models;
using archetypedata.domain.Services.Messaging;
using archetypedata.domain.WebPages;
using archetypedata.infrastructure.Services.Messaging;
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
            services.AddTransient<IArchetypeWebPage, ArchetypeWebPage>();
            services.AddTransient<IQueue<Archetype>, ArchetypeQueue>();
            services.AddTransient<IQueue<ArchetypeCard>, ArchetypeCardQueue>();

            return services;
        }
    }
}