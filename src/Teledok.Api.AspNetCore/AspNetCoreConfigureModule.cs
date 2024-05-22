using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Teledok.Api.AspNetCore.Options;

namespace Teledok.Api.AspNetCore;

public static class AspNetCoreConfigureModule
{
    public static void ConfigureAspNetCoreModule(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            ConfigureSwagger(app);
        }

        app.MapControllers();
    }

    private static void ConfigureSwagger(WebApplication app)
    {
        var swaggerOptions = app.Services.GetRequiredService<IOptions<SwaggerOptions>>().Value;
            
        app.UseSwagger(options =>
        {
            options.RouteTemplate = "{documentName}/swagger.json";
        });

        app.UseSwaggerUI(options =>
        {
            options.RoutePrefix = string.Empty;
            options.SwaggerEndpoint("v1/swagger.json", swaggerOptions.Version);
            options.DisplayRequestDuration();
        });
    }
}