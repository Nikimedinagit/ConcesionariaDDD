using Application.Features.Vehiculos.Commands.ActivarModeloVehiculo;
using Application.Features.Vehiculos.Commands.ActualizarModeloVehiculo;
using Application.Features.Vehiculos.Commands.AgregarModeloVehiculo;
using Application.Features.Vehiculos.Commands.DesactivarModeloVehiculo;
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
    public async Task<IActionResult> ObtenerActivas(
        [FromQuery] string filtro,
        [FromQuery] Guid? marcaVehiculoId,
        [FromQuery] Guid? tipoVehiculoId)
    {
        var resultadoModelosActivas = await _mediator.Send(new ObtenerModelosVehiculosActivasQuery
        {
            Filtro = filtro,
            MarcaVehiculoId = marcaVehiculoId,
            TipoVehiculoId = tipoVehiculoId
        });

        return Ok(resultadoModelosActivas);
    }

    // METODO OBTENER INACTIVAS
    [HttpGet("inactivas")]
    public async Task<IActionResult> ObtenerInactivas(
        [FromQuery] string filtro,
        [FromQuery] Guid? marcaVehiculoId,
        [FromQuery] Guid? tipoVehiculoId)
    {
        var resultadoModelosInactivas = await _mediator.Send(new ObtenerModelosVehiculosInactivasQuery
        {
            Filtro = filtro,
            MarcaVehiculoId = marcaVehiculoId,
            TipoVehiculoId = tipoVehiculoId
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

      // METODO ACTUALIZAR
    [HttpPut("{id}")]
    public async Task<IActionResult> Actualizar(
        Guid id,
        [FromBody] ActualizarModeloVehiculoCommand command
    )
    {
        if (id != command.ModeloVehiculoId)
        {
            return BadRequest(new { mensaje = "El Id no coincide." });
        }
        await _mediator.Send(command);

        return Ok(new { mensaje = "Modelo de vehículo actualizado correctamente." });
    }

     // METODO ACTUALIZAR ESTADO A ACTIVAR
    [HttpPut("activar/{id}")]
    public async Task<IActionResult> Activar(
        Guid id,
        [FromBody] ActivarModeloVehiculoCommand command
    )
    {
        if (id != command.ModeloVehiculoId)
        {
            return BadRequest(new { mensaje = "El Id no coincide." });
        }
        
        await _mediator.Send(command);

        return Ok(new { mensaje = "Modelo de vehículo activado correctamente." });
    }

      // MEOTODO ACTUALIZAR ESTADO A DESACTIVAR
    [HttpPut("desactivar/{id}")]
    public async Task<IActionResult> Desactivar(
        Guid id,
        [FromBody] DesactivarModeloVehiculoCommand command
    )
    {
        if (id != command.ModeloVehiculoId)
        {
            return BadRequest(new { mensaje = "El Id no coincide." });
        }
        
        await _mediator.Send(command);

        return Ok(new { mensaje = "Modelo de vehículo desactivado correctamente." });
    }
}
