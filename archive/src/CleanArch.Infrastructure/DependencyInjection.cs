using CleanArch.Application.Abstractions;
using CleanArch.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArch.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IProductRepository, InMemoryProductRepository>();
        return services;
    }
}
