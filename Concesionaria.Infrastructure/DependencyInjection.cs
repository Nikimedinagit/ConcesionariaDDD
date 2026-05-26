using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Application.Empresas.Queries;
using Concesionaria.Application.Interfaces;
using Concesionaria.Domain.Identity;
using Concesionaria.Infrastructure.Identity;
using Concesionaria.Infrastructure.Persistence;
using Concesionaria.Infrastructure.Persistence;
using Concesionaria.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Concesionaria.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // 1. Configuración de Base de Datos
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<ApplicationDbContext>());

        // 2. Configuración de Identity
        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            options.Password.RequireNonAlphanumeric = true;
        })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddHttpContextAccessor();

        services.AddScoped<ICurrentUserService,
            CurrentUserService>();

        // 3. Servicios de Aplicación y Negocio (Aquí agrupas los que mencionaste)
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ILocalidadService, LocalidadService>();
        services.AddScoped<GetPerfilQueryHandler>();
        services.AddScoped<ISmsService, SmsService>();
        services.AddScoped<INotificationService, NotificationService>();

        return services;
    }
}