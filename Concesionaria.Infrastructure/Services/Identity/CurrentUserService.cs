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
            ?? string.Empty;

   public Guid EmpresaId
{
    get
    {
        var empresaId = _httpContextAccessor
            .HttpContext?
            .User?
            .FindFirst("EmpresaId")?
            .Value;

        Console.WriteLine("EMPRESA ID: " + empresaId);

        return empresaId != null
            ? Guid.Parse(empresaId)
            : Guid.Empty;
    }
}
}