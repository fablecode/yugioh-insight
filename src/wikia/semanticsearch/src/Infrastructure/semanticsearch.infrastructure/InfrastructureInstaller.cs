using System;
using Microsoft.Extensions.DependencyInjection;

namespace semanticsearch.infrastructure
{
    public static class InfrastructureInstaller
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            return services;
        }
    }
}
