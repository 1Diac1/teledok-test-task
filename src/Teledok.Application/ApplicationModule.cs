using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Teledok.Domain.Entities;
using Teledok.Infrastructure.Abstractions.Repositories;
using Teledok.Infrastructure.EntityFrameworkCore;
using Teledok.Infrastructure.EntityFrameworkCore.Postgresql;

namespace Teledok.Application;

public static class ApplicationModule
{
    public static IServiceCollection AddApplicationModule(this IServiceCollection services)
    {
        // inject services
        services.AddScoped<IEntityRepository<int, Client>, BaseEntityRepository<int, Client, TeledokDbContext>>();
        services.AddScoped<IReadEntityRepository<int, Client>, BaseReadEntityRepository<int, Client, TeledokDbContext>>();

        services.AddScoped<IEntityRepository<int, Founder>, BaseEntityRepository<int, Founder, TeledokDbContext>>();
        services.AddScoped<IReadEntityRepository<int, Founder>, BaseReadEntityRepository<int, Founder, TeledokDbContext>>();

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        return services;
    }
}