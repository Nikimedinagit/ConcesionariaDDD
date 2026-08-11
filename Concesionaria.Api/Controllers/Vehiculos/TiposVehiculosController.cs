using Application.Features.Vehiculos.Commands.ActivarTipoVehiculo;
using Application.Features.Vehiculos.Commands.ActualizarTipoVehiculo;
using Application.Features.Vehiculos.Commands.AgregarTipoVehiculo;
using Application.Features.Vehiculos.Commands.DesactivarTipoVehiculo;
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
    
      // METODO ACTUALIZAR
    [HttpPut("{id}")]
    public async Task<IActionResult> Actualizar(
        Guid id,
        [FromBody] ActualizarTipoVehiculoCommand command
    )
    {
        if (id != command.TipoVehiculoId)
        {
            return BadRequest(new { mensaje = "El Id no coincide." });
        }
        await _mediator.Send(command);

        return Ok(new { mensaje = "Tipo de vehículo actualizado correctamente." });
    }

      // METODO ACTUALIZAR ESTADO A ACTIVAR
    [HttpPut("activar/{id}")]
    public async Task<IActionResult> Activar(
        Guid id,
        [FromBody] ActivarTipoVehiculoCommand command
    )
    {
        if (id != command.TipoVehiculoId)
        {
            return BadRequest(new { mensaje = "El Id no coincide." });
        }
        
        await _mediator.Send(command);

        return Ok(new { mensaje = "Tipo de vehículo activado correctamente." });
    }

        // MEOTODO ACTUALIZAR ESTADO A DESACTIVAR
    [HttpPut("desactivar/{id}")]
    public async Task<IActionResult> Desactivar(
        Guid id,
        [FromBody] DesactivarTipoVehiculoCommand command
    )
    {
        if (id != command.TipoVehiculoId)
        {
            return BadRequest(new { mensaje = "El Id no coincide." });
        }
        
        await _mediator.Send(command);

        return Ok(new { mensaje = "Tipo de vehículo desactivado correctamente." });
    }
}
