using Application.Features.Vehiculos.Commands.AgregarTipoVehiculo;
using Application.Features.Vehiculos.Queries.ObtenerTiposVehiculosActivas;
using Application.Features.Vehiculos.Queries.ObtenerTiposVehiculosInactivas;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Concesionaria.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class TiposVehiculosController : ControllerBase
{
    private readonly IMediator _mediator;

    public TiposVehiculosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // METODO OBTENER ACTIVAS
    [HttpGet("activas")]
    public async Task<IActionResult> ObtenerActivas([FromQuery] string filtro)
    {
        var resultadoTiposVehiculosInctivas = await _mediator.Send(
            new ObtenerTiposVehiculosActivasQuery { Filtro = filtro }
        );

        return Ok(resultadoTiposVehiculosInctivas);
    }

    // METODO OBTENER INACTIVAS
    [HttpGet("inactivas")]
    public async Task<IActionResult> ObtenerInactivas([FromQuery] string filtro)
    {
        var resultadoTiposVehiculosInctivas = await _mediator.Send(
            new ObtenerTiposVehiculosInactivasQuery { Filtro = filtro }
        );

        return Ok(resultadoTiposVehiculosInctivas);
    }

        // METODO AGREGAR
    [HttpPost]
    public async Task<IActionResult> Agregar([FromBody] AgregarTipoVehiculoCommand command)
    {
        var id = await _mediator.Send(command);

        return Ok(
            new { mensaje = "Tipo de vehículo agregado correctamente.", tipoVehiculoId = id }
        );
    }
}
