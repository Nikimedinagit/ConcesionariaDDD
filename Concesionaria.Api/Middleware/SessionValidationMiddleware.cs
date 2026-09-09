using System.Security.Claims;
using Concesionaria.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Concesionaria.Api.Middleware;

public class SessionValidationMiddleware
{
    private readonly RequestDelegate _next;

    public SessionValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext context,
        ApplicationDbContext dbContext)
    {
        var endpoint = context.GetEndpoint();

        var permiteAnonimo =
            endpoint?.Metadata.GetMetadata<IAllowAnonymous>() is not null;

        var estaAutenticado =
            context.User.Identity?.IsAuthenticated == true;

        // Login, registro y endpoints públicos no necesitan esta validación.
        if (permiteAnonimo || !estaAutenticado)
        {
            await _next(context);
            return;
        }

        var userId =
            context.User.FindFirstValue("nameid") ??
            context.User.FindFirstValue(ClaimTypes.NameIdentifier);

        var sessionVersionValue =
            context.User.FindFirstValue("sessionVersion");

        if (
            string.IsNullOrWhiteSpace(userId) ||
            !int.TryParse(sessionVersionValue, out var tokenSessionVersion)
        )
        {
            await ResponderSesionInvalida(context);
            return;
        }

        var session = await dbContext.Users
            .AsNoTracking()
            .Where(user => user.Id == userId)
            .Select(user => new
            {
                user.SessionVersion,
                user.LockoutEnd,
                EmpresaValida = dbContext.Empresas.Any(empresa =>
                    empresa.Id == user.EmpresaId &&
                    empresa.Activa &&
                    !empresa.Eliminado),
                SucursalValida = user.SucursalId.HasValue &&
                    dbContext.Sucursales
                        .IgnoreQueryFilters()
                        .Any(sucursal =>
                            sucursal.Id == user.SucursalId.Value &&
                            sucursal.EmpresaId == user.EmpresaId &&
                            !sucursal.Eliminado)
            })
            .FirstOrDefaultAsync(context.RequestAborted);

        var usuarioBloqueado =
            session?.LockoutEnd.HasValue == true &&
            session.LockoutEnd.Value > DateTimeOffset.UtcNow;

        var sesionDesactualizada =
            session is not null &&
            session.SessionVersion != tokenSessionVersion;

        if (
            session is null ||
            usuarioBloqueado ||
            sesionDesactualizada ||
            !session.EmpresaValida ||
            !session.SucursalValida
        )
        {
            await ResponderSesionInvalida(context);
            return;
        }

        await _next(context);
    }

    private static async Task ResponderSesionInvalida(
        HttpContext context)
    {
        context.Response.StatusCode =
            StatusCodes.Status401Unauthorized;

        context.Response.ContentType =
            "application/json";

        await context.Response.WriteAsJsonAsync(new
        {
            code = "SESSION_INVALIDATED",
            message =
                "Tu sesión cambió. Debés iniciar sesión nuevamente."
        });
    }
}
