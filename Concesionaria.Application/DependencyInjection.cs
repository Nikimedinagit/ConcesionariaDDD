using Microsoft.Extensions.DependencyInjection;

namespace Concesionaria.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Aquí configuraremos MediatR y validadores en el futuro
        return services;
    }
}