using System;
using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Cards.Application
{
    public static class ApplicationInstaller
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            return services
                    .AddCqrs();
        }

        private static IServiceCollection AddCqrs(this IServiceCollection services)
        {
            services.AddMediatR(typeof(ApplicationInstaller).GetTypeInfo().Assembly);

            return services;
        }
    }
}
