using AutoMapper;
using cardsectionprocessor.core.Processor;
using cardsectionprocessor.core.Service;
using cardsectionprocessor.core.Strategy;
using cardsectionprocessor.domain.Processor;
using cardsectionprocessor.domain.Service;
using cardsectionprocessor.domain.Strategy;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace cardsectionprocessor.application
{
    public static class ApplicationInstaller
    {
            public static IServiceCollection AddApplicationServices(this IServiceCollection services)
            {
                services
                    .AddCqrs()
                    .AddDomainServices()
                    .AddValidation()
                    .AddAutoMapper();

                return services;
            }

            private static IServiceCollection AddCqrs(this IServiceCollection services)
            {
                services.AddMediatR(typeof(ApplicationInstaller).GetTypeInfo().Assembly);

                return services;
            }


            private static IServiceCollection AddDomainServices(this IServiceCollection services)
            {
                services.AddTransient<ICardService, CardService>();
                services.AddTransient<ICardTipService, CardTipService>();
                services.AddTransient<ICardRulingService, CardRulingService>();
                services.AddTransient<ICardTriviaService, CardTriviaService>();

                services.AddTransient<ICardSectionProcessor, CardSectionProcessor>();

                services.AddTransient<ICardSectionProcessorStrategy, CardTipsProcessorStrategy>();
                services.AddTransient<ICardSectionProcessorStrategy, CardRulingsProcessorStrategy>();
                services.AddTransient<ICardSectionProcessorStrategy, CardTriviaProcessorStrategy>();

                return services;
            }

            private static IServiceCollection AddValidation(this IServiceCollection services)
            {
                return services;
            }
    }
}