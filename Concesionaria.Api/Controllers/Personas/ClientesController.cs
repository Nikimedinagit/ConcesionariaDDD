using Application.Features.Personas.Commands.ActivarCliente;
using Application.Features.Personas.Commands.ActualizarCliente;
using Application.Features.Personas.Commands.DesactivarCliente;
using Concesionaria.Application.Features.Personas.Clientes.Queries.ObtenerClientesActivas;
using Concesionaria.Application.Features.Personas.Clientes.Queries.ObtenerClientesInactivas;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Concesionaria.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ClientesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ClientesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // METODO OBTENER ACTIVAS
    [HttpGet("activas")]
    public async Task<IActionResult> ObtenerActivas([FromQuery] string filtro)
    {
        var resultadoClientesActivas = await _mediator.Send(new ObtenerClientesActivasQuery
        {
            Filtro = filtro
        });

        return Ok(resultadoClientesActivas);
    }

    // METODO OBTENER INACTIVAS
    [HttpGet("inactivas")]
    public async Task<IActionResult> ObtenerInactivas([FromQuery] string filtro)
    {
        var resultadoClientesActivas = await _mediator.Send(new ObtenerClientesInactivasQuery
        {
            Filtro = filtro
        });

        return Ok(resultadoClientesActivas);
    }

        // METODO AGREGAR
    [HttpPost]
    public async Task<IActionResult> Agregar([FromBody] AgregarClienteCommand command)
    {
        var id = await _mediator.Send(command);

        return Ok(
            new { mensaje = "Cliente registrado correctamente.", clienteId = id }
        );
    }

  // METODO ACTUALIZAR
    [HttpPut("{id}")]
    public async Task<IActionResult> Actualizar(
        Guid id,
        [FromBody] ActualizarClienteCommand command
    )
    {
        if (id != command.ClienteId)
        {
            return BadRequest(new { mensaje = "El Id no coincide." });
        }
        await _mediator.Send(command);

        return Ok(new { mensaje = "Cliente actualizado correctamente." });
    }

       // METODO ACTUALIZAR ESTADO A ACTIVAR
    [HttpPut("activar/{id}")]
    public async Task<IActionResult> Activar(
        Guid id,
        [FromBody] ActivarClienteCommand command
    )
    {
        if (id != command.ClienteId)
        {
            return BadRequest(new { mensaje = "El Id no coincide." });
        }
        
        await _mediator.Send(command);

        return Ok(new { mensaje = "Cliente activado correctamente." });
    }
   
       // METODO ACTUALIZAR ESTADO A DESACTIVAR
    [HttpPut("desactivar/{id}")]
    public async Task<IActionResult> Desactivar(
        Guid id,
        [FromBody] DesactivarClienteCommand command
    )
    {
        if (id != command.ClienteId)
        {
            return BadRequest(new { mensaje = "El Id no coincide." });
        }
        
        await _mediator.Send(command);

        return Ok(new { mensaje = "Cliente desactivado correctamente." });
    }
}