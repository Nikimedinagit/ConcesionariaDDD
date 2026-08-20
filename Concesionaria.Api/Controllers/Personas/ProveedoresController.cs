using Concesionaria.Application.Features.Personas.Proveedor.Queries.ObtenerProveedorActivas;
using Concesionaria.Application.Features.Personas.Proveedor.Queries.ObtenerProveedorInactivas;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Concesionaria.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ProveedoresController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProveedoresController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // METODO OBTENER ACTIVAS
    [HttpGet("activas")]
    public async Task<IActionResult> ObtenerActivas([FromQuery] string filtro)
    {
        var resultadoProveedoresActivas = await _mediator.Send(new ObtenerProveedorActivasQuery
        {
            Filtro = filtro
        });

        return Ok(resultadoProveedoresActivas);
    }
    // METODO OBTENER INACTIVAS
    [HttpGet("inactivas")]
    public async Task<IActionResult> ObtenerInactivas([FromQuery] string filtro)
    {
        var resultadoProveedoresInactivas = await _mediator.Send(new ObtenerProveedorInactivasQuery
        {
            Filtro = filtro
        });

        return Ok(resultadoProveedoresInactivas);
    }

    // METODO AGREGAR
    [HttpPost]
    public async Task<IActionResult> Agregar([FromBody] AgregarProveedorCommand command)
    {
        var id = await _mediator.Send(command);

        return Ok(
            new { mensaje = "Proveedor registrado correctamente.", proveedorId = id }
        );
    }
}