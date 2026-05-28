using System.Reflection;
using Application.Features.CategoriasGastos.Commands.ActualizarCategoriaGasto;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
// Asegúrate de importar el namespace donde vive tu clase de ejemplo
// Por ejemplo: using Concesionaria.Application.TusComandos; 

namespace Concesionaria.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
{
    // 1. Registrar MediatR (escaneando el ensamblado de esta clase)
    services.AddMediatR(cfg => {
        cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
    });

    // 2. Registrar FluentValidation de forma explícita y segura
    // Esto es lo que llena el 'Count' de validadores
    services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

    // 3. Registrar el Pipeline de Validación
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

    return services;
}
}