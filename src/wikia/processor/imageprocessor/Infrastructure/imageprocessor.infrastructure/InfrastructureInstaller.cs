using imageprocessor.domain.SystemIO;
using imageprocessor.infrastructure.SystemIO;
using Microsoft.Extensions.DependencyInjection;

namespace imageprocessor.infrastructure
{
    public static class InfrastructureInstaller
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddTransient<IFileSystem, FileSystem>();

            return services;
        }
    }
}