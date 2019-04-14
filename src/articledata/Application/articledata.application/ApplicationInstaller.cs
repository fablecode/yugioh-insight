using articledata.domain.Contracts;
using GreenPipes;
using MassTransit;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;
using articledata.application.ScheduledTasks.CardInformation;
using articledata.core.ArticleList.Processor;
using articledata.domain.ArticleList.DataSource;
using articledata.domain.ArticleList.Item;
using articledata.domain.ArticleList.Processor;
using FluentValidation;
using RabbitMQ.Client;
using wikia.Api;

namespace articledata.application
{
    public static class ApplicationInstaller
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services
                .AddCqrs()
                .AddMassTransitConfiguration()
                .AddValidation()
                .AddDomainServices();

            return services;
        }

        private static IServiceCollection AddCqrs(this IServiceCollection services)
        {
            services.AddMediatR(typeof(ApplicationInstaller).GetTypeInfo().Assembly);

            return services;
        }

        private static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddTransient<IArticleCategoryDataSource, ArticleCategoryDataSource>();
            services.AddTransient<IArticleBatchProcessor, ArticleBatchProcessor>();
            services.AddTransient<IArticleCategoryProcessor, ArticleCategoryProcessor>();
            services.AddTransient<IArticleProcessor, ArticleProcessor>();

            services.AddTransient<IArticleItemProcessor, CardItemProcessor>();


            var appSettings = services.BuildServiceProvider().GetService<IOptions<Configuration.AppSettings>>();

            services.AddSingleton<IWikiArticleList>(new WikiArticleList(appSettings.Value.WikiaDomainUrl));

            return services;
        }

        private static IServiceCollection AddMassTransitConfiguration(this IServiceCollection services)
        {
            var rabbitMqSettings = services.BuildServiceProvider().GetService<IOptions<Configuration.RabbitMqSettings>>();

            // Register MassTransit
            services.AddMassTransit(x =>
            {
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    var host = cfg.Host(rabbitMqSettings.Value.Host, h =>
                    {
                        h.Username(rabbitMqSettings.Value.Username);
                        h.Password(rabbitMqSettings.Value.Password);
                    });

                    cfg.ReceiveEndpoint(host, "card-article", e =>
                    {
                        e.PrefetchCount = 16;
                        e.UseMessageRetry(c => c.Interval(2, 100));

                        EndpointConvention.Map<ISubmitArticle>(e.InputAddress);
                    });

                    // or, configure the endpoints by convention
                    cfg.ConfigureEndpoints(provider);
                }));

                //x.AddRequestClient<ISubmitArticle>();
            });

            return services;
        }

        private static IServiceCollection AddValidation(this IServiceCollection services)
        {
            services.AddTransient<IValidator<CardInformationTask>, CardInformationTaskValidator>();

            return services;
        }
    }
}