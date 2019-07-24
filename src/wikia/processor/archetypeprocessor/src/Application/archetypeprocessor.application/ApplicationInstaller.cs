using archetypeprocessor.core.Services;
using archetypeprocessor.domain.Services;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using archetypeprocessor.application.MessageConsumers.ArchetypeCardInformation;
using archetypeprocessor.application.MessageConsumers.ArchetypeInformation;
using archetypeprocessor.core.Processor;
using archetypeprocessor.domain.Processor;
using FluentValidation;

namespace archetypeprocessor.application
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
            services.AddTransient<IArchetypeService, ArchetypeService>();
            services.AddTransient<IArchetypeCardsService, ArchetypeCardsService>();
            services.AddTransient<IArchetypeProcessor, ArchetypeProcessor>();
            services.AddTransient<IArchetypeCardProcessor, ArchetypeCardProcessor>();

            return services;
        }

        private static IServiceCollection AddValidation(this IServiceCollection services)
        {
            services.AddTransient<IValidator<ArchetypeInformationConsumer>, ArchetypeInformationConsumerValidator>();
            services.AddTransient<IValidator<ArchetypeCardInformationConsumer>, ArchetypeCardInformationConsumerValidator>();

            return services;
        }
    }
}
