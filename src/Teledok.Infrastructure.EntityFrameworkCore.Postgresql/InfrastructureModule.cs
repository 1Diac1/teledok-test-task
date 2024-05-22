using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Teledok.Infrastructure.EntityFrameworkCore.Postgresql;

public static class InfrastructureModule
{
    public static IServiceCollection AddInfrastructureModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TeledokDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString(nameof(TeledokDbContext))));
        
        return services;
    }
}