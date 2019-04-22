using carddata.domain.Services.Messaging;
using carddata.domain.WebPages;
using carddata.domain.WebPages.Cards;
using carddata.infrastructure.Services.Messaging.Cards;
using carddata.infrastructure.WebPages;
using carddata.infrastructure.WebPages.Cards;
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

            services.AddTransient<ICardWebPage, CardWebPage>();
            services.AddTransient<ICardHtmlTable, CardHtmlTable>();
            services.AddTransient<ICardHtmlDocument, CardHtmlDocument>();
            services.AddTransient<IHtmlWebPage, HtmlWebPage>();

            return services;
        }
    }
}