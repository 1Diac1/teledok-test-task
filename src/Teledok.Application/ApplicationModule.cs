using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Teledok.Application;

public static class ApplicationModule
{
    public static IServiceCollection AddApplicationModule(this IServiceCollection services)
    {
        // inject services
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        
        return services;
    }
}