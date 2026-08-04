using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Features.Vehiculos.Queries.ObtenerMarcasVehiculosActivas;
using Application.Features.Vehiculos.Queries.ObtenerMarcasVehiculosInactivas;
using Application.Features.Vehiculos.Commands.ActivarMarcaVehiculo;
using Application.Features.Vehiculos.Commands.AgregarMarcaVehiculo;
using Application.Features.Vehiculos.Commands.ActualizarMarcaVehiculo;
using Application.Features.Vehiculos.Commands.DesactivarMarcaVehiculo;

namespace Concesionaria.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class MarcasVehiculosController : ControllerBase
{
    private readonly IMediator _mediator;

    public MarcasVehiculosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // METODO OBTENER ACTIVAS
    [HttpGet("activas")]
    public async Task<IActionResult> ObtenerActivas([FromQuery] string filtro)
    {
        var resultadoCategoriasGastosActivas = await _mediator.Send(new ObtenerMarcasVehiculosActivasQuery
        {
            Filtro = filtro
        });

        return Ok(resultadoCategoriasGastosActivas);
    }

    // METODO OBTENER INACTIVAS
    [HttpGet("inactivas")]
    public async Task<IActionResult> ObtenerInactivas([FromQuery] string filtro)
    {
        var resultadoCategoriasGastosInactivas = await _mediator.Send(
            new ObtenerMarcasVehiculosInactivasQuery
            {
                Filtro = filtro
            }
        );

        return Ok(resultadoCategoriasGastosInactivas);
    }

    // METODO AGREGAR
    [HttpPost]
    public async Task<IActionResult> Agregar([FromBody] AgregarMarcaVehiculoCommand command)
    {
        var id = await _mediator.Send(command);

        return Ok(
            new { mensaje = "Marca de vehículo agregada correctamente.", marcaVehiculoId = id }
        );
    }

    // METODO ACTUALIZAR
    [HttpPut("{id}")]
    public async Task<IActionResult> Actualizar(
        Guid id,
        [FromBody] ActualizarMarcaVehiculoCommand command
    )
    {
        if (id != command.MarcaVehiculoId)
        {
            return BadRequest(new { mensaje = "El Id no coincide." });
        }
        await _mediator.Send(command);

        return Ok(new { mensaje = "Marca de vehículo actualizada correctamente." });
    }

    // METODO ACTUALIZAR ESTADO A ACTIVAR
    [HttpPut("activar/{id}")]
    public async Task<IActionResult> Activar(
        Guid id,
        [FromBody] ActivarMarcaVehiculoCommand command
    )
    {
        if (id != command.MarcaVehiculoId)
        {
            return BadRequest(new { mensaje = "El Id no coincide." });
        }
        
        await _mediator.Send(command);

        return Ok(new { mensaje = "Marca de vehículo activada correctamente." });
    }

    // MEOTODO ACTUALIZAR ESTADO A DESACTIVAR
    [HttpPut("desactivar/{id}")]
    public async Task<IActionResult> Desactivar(
        Guid id,
        [FromBody] DesactivarMarcaVehiculoCommand command
    )
    {
        if (id != command.MarcaVehiculoId)
        {
            return BadRequest(new { mensaje = "El Id no coincide." });
        }
        
        await _mediator.Send(command);

        return Ok(new { mensaje = "Marca de vehículo desactivada correctamente." });
    }
}
