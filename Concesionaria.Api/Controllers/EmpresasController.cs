using System.Security.Claims;
using Concesionaria.Application.Empresas.Queries;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Concesionaria.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class EmpresasController : ControllerBase
{
    private readonly GetPerfilQueryHandler _handler;

    public EmpresasController(GetPerfilQueryHandler handler)
    {
        _handler = handler;
    }

    [HttpGet("perfil")]
    public async Task<IActionResult> GetPerfil()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized(new
            {
                message = "No se encontró el userId"
            });
        }

        var perfil = await _handler.Handle(Guid.Parse(userId));

        return Ok(perfil);
    }
}