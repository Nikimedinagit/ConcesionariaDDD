using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Application.Interfaces;
using Concesionaria.Domain.Identity;
using Concesionaria.Domain.Interfaces.IRepositories;
using Concesionaria.Infrastructure.Identity;
using Concesionaria.Infrastructure.Persistence;
using Concesionaria.Infrastructure.Services;
using Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Concesionaria.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // =========================
        // Base de Datos
        // =========================

        services.AddDbContext<ApplicationDbContext>(
            (serviceProvider, options) =>
            {
                var currentUser = serviceProvider.GetRequiredService<ICurrentUserService>();

                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            }
        );

        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<ApplicationDbContext>()
        );

        // =========================
        // Identity
        // =========================

        services
            .AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        // =========================
        // HttpContext
        // =========================

        services.AddHttpContextAccessor();

        // =========================
        // Servicios
        // =========================

        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.AddScoped<ITokenService, TokenService>();

        services.AddScoped<ILocalidadService, LocalidadService>();

        services.AddScoped<ISmsService, SmsService>();

        services.AddScoped<INotificationService, NotificationService>();

        // =========================
        // Repositories
        // =========================

        services.AddScoped<ICategoriaGastoRepository, CategoriaGastoRepository>();

        services.AddScoped<ISucursalRepository, SucursalRepository>();

        services.AddScoped<ICuentaRepository, CuentaRepository>();

        return services;
    }
}
