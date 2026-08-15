using Application.Features.Vehiculos.Commands.AgregarModeloVehiculo;
using Application.Features.Vehiculos.Queries.ObtenerModelosVehiculosActivas;
using Application.Features.Vehiculos.Queries.ObtenerModelosVehiculosInactivas;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Concesionaria.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ModelosVehiculosController : ControllerBase
{
    private readonly IMediator _mediator;

    public ModelosVehiculosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // METODO OBTENER ACTIVAS
    [HttpGet("activas")]
    public async Task<IActionResult> ObtenerActivas([FromQuery] string filtro)
    {
        var resultadoModelosActivas = await _mediator.Send(new ObtenerModelosVehiculosActivasQuery
        {
            Filtro = filtro
        });

        return Ok(resultadoModelosActivas);
    }

    // METODO OBTENER INACTIVAS
    [HttpGet("inactivas")]
    public async Task<IActionResult> ObtenerInactivas([FromQuery] string filtro)
    {
        var resultadoModelosInactivas = await _mediator.Send(new ObtenerModelosVehiculosInactivasQuery
        {
            Filtro = filtro
        });

        return Ok(resultadoModelosInactivas);
    }

        // METODO AGREGAR
    [HttpPost]
    public async Task<IActionResult> Agregar([FromBody] AgregarModeloVehiculoCommand command)
    {
        var id = await _mediator.Send(command);

        return Ok(
            new { mensaje = "Modelo de vehículo agregado correctamente.", modeloVehiculoId = id }
        );
    }
}
