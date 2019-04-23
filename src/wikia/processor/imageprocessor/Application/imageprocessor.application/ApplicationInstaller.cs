using System.Reflection;
using FluentValidation;
using imageprocessor.application.Commands.DownloadImage;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace imageprocessor.application
{
    public static class ApplicationInstaller
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services
                .AddCqrs()
                .AddValidation();

            return services;
        }

        private static IServiceCollection AddCqrs(this IServiceCollection services)
        {
            services.AddMediatR(typeof(ApplicationInstaller).GetTypeInfo().Assembly);

            return services;
        }

        private static IServiceCollection AddValidation(this IServiceCollection services)
        {
            services.AddTransient<IValidator<DownloadImageCommand>, DownloadImageCommandValidator>();

            return services;
        }
    }
}