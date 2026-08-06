using Application.Features.Cuentas.Commands.ActivarCuenta;
using Application.Features.Cuentas.Commands.ActualizarCuenta;
using Application.Features.Cuentas.Commands.AgregarCuenta;
using Application.Features.Cuentas.Commands.DesactivarCuenta;
using Application.Features.Cuentas.Queries.ObtenerCuentasActivas;
using Application.Features.Cuentas.Queries.ObtenerCuentasInactivas;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Concesionaria.Domain.Cuentas.Enums;

namespace Concesionaria.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CuentasController : ControllerBase
{
    // private readonly ObtenerCuentasActivasQuery _handler;
    private readonly IMediator _mediator;

    public CuentasController(IMediator mediator)
    {
        _mediator = mediator;
    }

    //METODO OBTENER ACTIVAS
    [HttpGet("activas")]
    public async Task<IActionResult> ObtenerActivas(
        [FromQuery] string filtro,
        [FromQuery] TipoCuenta? tipo,
        [FromQuery] int? nivel)
    {
        var resultadoCuentasActivas = await _mediator.Send(
            new ObtenerCuentasActivasQuery
            {
                Filtro = filtro,
                Tipo = tipo,
                Nivel = nivel
            }
        );

        return Ok(resultadoCuentasActivas);
    }

    // METODO PARA OBTENER INACTIVAS
    [HttpGet("inactivas")]
    public async Task<IActionResult> ObtenerInactivas(
        [FromQuery] string filtro,
        [FromQuery] TipoCuenta? tipo,
        [FromQuery] int? nivel)
    {
        var resultadoCuentasInactivas = await _mediator.Send(
            new ObtenerCuentasInactivasQuery
            {
                Filtro = filtro,
                Tipo = tipo,
                Nivel = nivel
            }
        );

        return Ok(resultadoCuentasInactivas);
    }

    // METODO PARA AGREGAR CUENTA
    [HttpPost]
    public async Task<IActionResult> Agregar([FromBody] AgregarCuentaCommand command)
    {
        var id = await _mediator.Send(command);

        return Ok(
            new { mensaje = "Cuenta agregada correctamente.", cuentaId = id }
        );
    }

    // METODO ACTUALIZAR
    [HttpPut("{id}")]
    public async Task<IActionResult> Actualizar(
        Guid id,
        [FromBody] ActualizarCuentaCommand command
    )
    {
        if (id != command.CuentaId)
        {
            return BadRequest(new { mensaje = "El Id no coincide." });
        }
        await _mediator.Send(command);

        return Ok(new { mensaje = "Cuenta actualizada correctamente." });
    }

    // METODO ACTUALIZAR ESTADO A ACTIVAR
    [HttpPut("activar/{id}")]
    public async Task<IActionResult> Activar(
        Guid id,
        [FromBody] ActivarCuentaCommand command
    )
    {
        if (id != command.CuentaId)
        {
            return BadRequest(new { mensaje = "El Id no coincide." });
        }
        
        await _mediator.Send(command);

        return Ok(new { mensaje = "Cuenta activada correctamente." });
    }

    // MEOTODO ACTUALIZAR ESTADO A DESACTIVAR
    [HttpPut("desactivar/{id}")]
    public async Task<IActionResult> Desactivar(
        Guid id,
        [FromBody] DesactivarCuentaCommand command
    )
    {
        if (id != command.CuentaId)
        {
            return BadRequest(new { mensaje = "El Id no coincide." });
        }
        
        await _mediator.Send(command);

        return Ok(new { mensaje = "Cuenta desactivada correctamente." });
    }
}
