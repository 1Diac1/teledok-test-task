using Microsoft.Extensions.DependencyInjection;

namespace Teledok.Application;

public static class ApplicationModule
{
    public static IServiceCollection AddApplicationModule(this IServiceCollection services)
    {
        // inject services

        return services;
    }
}