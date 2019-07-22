using archetypeprocessor.core.Services;
using archetypeprocessor.domain.Services;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace archetypeprocessor.application
{
    public static class ApplicationInstaller
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services
                .AddCqrs()
                .AddDomainServices()
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

            return services;
        }

        private static IServiceCollection AddValidation(this IServiceCollection services)
        {
            return services;
        }
    }
}
