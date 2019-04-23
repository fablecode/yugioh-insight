using System.Reflection;
using AutoMapper;
using cardprocessor.application.Commands;
using cardprocessor.application.Commands.DownloadImage;
using cardprocessor.application.Models.Cards.Input;
using cardprocessor.application.Validations.Cards;
using cardprocessor.core.Services;
using cardprocessor.core.Services.Messaging.Cards;
using cardprocessor.core.Strategies;
using cardprocessor.domain.Services;
using cardprocessor.domain.Services.Messaging.Cards;
using cardprocessor.domain.Strategies;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace cardprocessor.application
{
    public static class ApplicationInstaller
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services
                .AddCqrs()
                .AddValidation()
                .AddDomainServices()
                .AddStrategies()
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
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IAttributeService, AttributeService>();
            services.AddTransient<ILimitService, LimitService>();
            services.AddTransient<ILinkArrowService, LinkArrowService>();
            services.AddTransient<ISubCategoryService, SubCategoryService>();
            services.AddTransient<ITypeService, TypeService>();
            services.AddTransient<IFileSystemService, FileSystemService>();
            services.AddTransient<ICardCommandMapper, CardCommandMapper>();
            services.AddTransient<ICardImageQueueService, CardImageQueueService>();

            return services;
        }

        private static IServiceCollection AddValidation(this IServiceCollection services)
        {
            services.AddTransient<IValidator<CardInputModel>, CardValidator>();
            services.AddTransient<IValidator<DownloadImageCommand>, DownloadImageCommandValidator>();

            return services;
        }

        public static IServiceCollection AddStrategies(this IServiceCollection services)
        {

            services.AddTransient<ICardTypeStrategy, MonsterCardTypeStrategy>();
            services.AddTransient<ICardTypeStrategy, SpellCardTypeStrategy>();
            services.AddTransient<ICardTypeStrategy, TrapCardTypeStrategy>();

            return services;
        }

    }
}