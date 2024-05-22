using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Teledok.Api.AspNetCore.Filters;
using Teledok.Api.AspNetCore.Options;
using Teledok.Application;
using Teledok.Infrastructure.EntityFrameworkCore.Postgresql;

namespace Teledok.Api.AspNetCore;

public static class AspNetCoreConfigureServicesModule
{
    public static void AddAspNetCoreConfigureServicesModule(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<SwaggerOptions>(configuration.GetSection(nameof(SwaggerOptions)));
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        services.AddInfrastructureModule(configuration);
        services.AddApplicationModule();

        services.AddControllers(options =>
        {
            options.Filters.Add<ApiExceptionFilter>();
            options.Filters.Add<ValidatorActionFilter>();
            options.ModelValidatorProviders.Clear();
        })
            .AddNewtonsoftJson();

        services.AddFluentValidationAutoValidation();
        ConfigureSwaggerModule(services, configuration);
    }

    private static void ConfigureSwaggerModule(IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer();

        using var serviceProvider = services.BuildServiceProvider();
        var swaggerOptions = serviceProvider.GetRequiredService<IOptions<SwaggerOptions>>().Value;
        
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(swaggerOptions.Version, new OpenApiInfo()
            {
                Version = swaggerOptions.Version,
                Title = swaggerOptions.Title,
                Description = swaggerOptions.Description
            });
        });
    }
}