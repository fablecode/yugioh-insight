using System.Reflection;
using AutoMapper;
using banlistprocessor.core.Services;
using banlistprocessor.domain.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace banlistprocessor.application
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
            services.AddTransient<IBanlistService, BanlistService>();
            services.AddTransient<IBanlistCardService, BanlistCardService>();

            return services;
        }

        private static IServiceCollection AddValidation(this IServiceCollection services)
        {
            return services;
        }
    }
}