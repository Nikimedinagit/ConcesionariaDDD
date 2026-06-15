using Application.Features.Ubicaciones.Commands.ActualizarSucursal;
using Application.Features.Ubicaciones.Commands.AgregarSucursal;
using Application.Features.Ubicaciones.Queries.ObtenerSucursalesActivas;
using Application.Features.Ubicaciones.Queries.ObtenerSucursalesInactivas;
using Concesionaria.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Concesionaria.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class SucursalesController : ControllerBase
{
    private readonly IMediator _mediator;

    public SucursalesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // METODO PARA OBTENER ACTIVAS
    [HttpGet("activas")]
    public async Task<ActionResult> ObtenerActivas([FromQuery] string filtro)
    {
        var resultadoSucursalesActivas = await _mediator.Send(
            new ObtenerSucursalesActivasQuery { Filtro = filtro }
        );

        return Ok(resultadoSucursalesActivas);
    }

    // METODO PARA OBTENER INACTIVAS

    [HttpGet("inactivas")]
    public async Task<ActionResult> ObtenerInactivas([FromQuery] string filtro)
    {
        var resultadoSucursalesInactivas = await _mediator.Send(
            new ObtenerSucursalesInactivasQuery { Filtro = filtro }
        );

        return Ok(resultadoSucursalesInactivas);
    }

    [HttpPost]
    public async Task<IActionResult> Agregar([FromBody] AgregarSucursalCommand command)
    {
        var id = await _mediator.Send(command);

        return Ok(new { mensaje = "Sucursal agregada correctamente.", sucursalId = id });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Actualizar(
        Guid id,
        [FromBody] ActualizarSucursalCommand command
    )
    {
        if (id != command.SucursalId)
        {
            return BadRequest(new { mensaje = "El Id no coincide." });
        }

        await _mediator.Send(command);
        return Ok(new { mensaje = "Sucursal actualizada correctamente." });
    }
}
