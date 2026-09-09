using System.Security.Claims;
using Concesionaria.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Concesionaria.Infrastructure.Identity;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(
        IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string UserId =>
        _httpContextAccessor
            .HttpContext?
            .User?
            .FindFirstValue(ClaimTypes.NameIdentifier)
        ?? _httpContextAccessor
            .HttpContext?
            .User?
            .FindFirstValue("nameid")
            ?? string.Empty;

    public Guid EmpresaId
    {
        get
        {
            var empresaId = _httpContextAccessor
                .HttpContext?
                .User?
                .FindFirstValue("empresaId")
                ?? _httpContextAccessor
                    .HttpContext?
                    .User?
                    .FindFirstValue("EmpresaId");

            return Guid.TryParse(empresaId, out var parsedEmpresaId)
                ? parsedEmpresaId
                : Guid.Empty;
        }
    }

    public Guid SucursalId
    {
        get
        {
            var sucursalId = _httpContextAccessor
                .HttpContext?
                .User?
                .FindFirstValue("sucursalId");

            return Guid.TryParse(sucursalId, out var parsedSucursalId)
                ? parsedSucursalId
                : Guid.Empty;
        }
    }

    public int SessionVersion
    {
        get
        {
            var sessionVersion = _httpContextAccessor
                .HttpContext?
                .User?
                .FindFirstValue("sessionVersion");

            return int.TryParse(sessionVersion, out var parsedSessionVersion)
                ? parsedSessionVersion
                : 0;
        }
    }

    public bool EsAdministrador => _httpContextAccessor
        .HttpContext?
        .User?
        .IsInRole("ADMINISTRADOR") == true;
}
