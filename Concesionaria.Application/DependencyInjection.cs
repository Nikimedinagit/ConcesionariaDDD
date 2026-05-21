using Microsoft.Extensions.DependencyInjection;

namespace Concesionaria.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // La capa Application no debe registrar implementaciones de Infrastructure.
        return services;
    }
}