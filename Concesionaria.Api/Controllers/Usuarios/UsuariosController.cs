using Application.Features.Usuarios.Commands.ActivarUsuario;
using Application.Features.Usuarios.Commands.ActualizarUsuario;
using Application.Features.Usuarios.Commands.AgregarUsuario;
using Application.Features.Usuarios.Commands.CambiarPasswordUsuario;
using Application.Features.Usuarios.Commands.DesactivarUsuario;
using Application.Features.Usuarios.Queries.ObtenerUsuariosActivos;
using Application.Features.Usuarios.Queries.ObtenerUsuariosInactivos;
using Application.Features.Usuarios.Queries.ObtenerRoles;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Concesionaria.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UsuariosController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsuariosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("activos")]
    public async Task<IActionResult> ObtenerActivos([FromQuery] string filtro = "")
    {
        var usuarios = await _mediator.Send(
            new ObtenerUsuariosActivosQuery { Filtro = filtro }
        );

        return Ok(usuarios);
    }

    [HttpGet("roles")]
    public async Task<IActionResult> ObtenerRoles()
    {
        var roles = await _mediator.Send(new ObtenerRolesQuery());

        return Ok(roles);
    }

    [HttpGet("inactivos")]
    public async Task<IActionResult> ObtenerInactivos([FromQuery] string filtro = "")
    {
        var usuarios = await _mediator.Send(
            new ObtenerUsuariosInactivosQuery { Filtro = filtro }
        );

        return Ok(usuarios);
    }

    [HttpPost]
    public async Task<IActionResult> Agregar([FromBody] AgregarUsuarioCommand command)
    {
        var id = await _mediator.Send(command);

        return Ok(new { mensaje = "Usuario agregado correctamente.", usuarioId = id });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Actualizar(
        Guid id,
        [FromBody] ActualizarUsuarioCommand command
    )
    {
        if (id != command.UsuarioId)
        {
            return BadRequest(new { mensaje = "El Id no coincide." });
        }

        await _mediator.Send(command);
        return Ok(new { mensaje = "Usuario actualizado correctamente." });
    }

    [HttpPut("password/{id}")]
    public async Task<IActionResult> CambiarPassword(
        Guid id,
        [FromBody] CambiarPasswordUsuarioCommand command
    )
    {
        if (id != command.UsuarioId)
        {
            return BadRequest(new { mensaje = "El Id no coincide." });
        }

        await _mediator.Send(command);
        return Ok(new { mensaje = "Contraseña actualizada correctamente." });
    }

    [HttpPut("activar/{id}")]
    public async Task<IActionResult> Activar(Guid id, [FromBody] ActivarUsuarioCommand command)
    {
        if (id != command.UsuarioId)
        {
            return BadRequest(new { mensaje = "El Id no coincide." });
        }

        await _mediator.Send(command);
        return Ok(new { mensaje = "Usuario activado correctamente." });
    }

    [HttpPut("desactivar/{id}")]
    public async Task<IActionResult> Desactivar(
        Guid id,
        [FromBody] DesactivarUsuarioCommand command
    )
    {
        if (id != command.UsuarioId)
        {
            return BadRequest(new { mensaje = "El Id no coincide." });
        }

        await _mediator.Send(command);
        return Ok(new { mensaje = "Usuario desactivado correctamente." });
    }
}
