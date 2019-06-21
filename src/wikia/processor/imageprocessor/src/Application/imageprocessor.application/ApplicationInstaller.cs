using System.Reflection;
using FluentValidation;
using imageprocessor.application.Commands;
using imageprocessor.application.Commands.DownloadImage;
using imageprocessor.application.Decorators.Loggers;
using imageprocessor.core.Services;
using imageprocessor.domain.Services;
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
                .AddValidation()
                .AddDomainServices()
                .AddDecorators();

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
        private static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddTransient<IFileSystemService, FileSystemService>();

            return services;
        }

        private static IServiceCollection AddDecorators(this IServiceCollection services)
        {
            //services.Decorate<IRequestHandler<DownloadImageCommand, CommandResult>, DownloadImageCommandHandlerLoggerDecorator>();

            return services;
        }

    }
}