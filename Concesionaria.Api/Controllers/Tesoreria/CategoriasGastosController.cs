using Application.Features.CategoriasGastos.Commands.ActivarCategoriaGasto;
using Application.Features.CategoriasGastos.Commands.ActualizarCategoriaGasto;
using Application.Features.CategoriasGastos.Commands.AgregarCategoriaGasto;
using Application.Features.CategoriasGastos.Commands.DesactivarCategoriaGasto;
using Application.Features.CategoriasGastos.Queries.ObtenerCategoriasGastosActivas;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Concesionaria.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CategoriasGastosController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriasGastosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // METODO OBTENER ACTIVAS
    [HttpGet("activas")]
    public async Task<IActionResult> ObtenerActivas([FromQuery] string filtro)
    {
        var resultadoCategoriasGastosActivas = await _mediator.Send(new ObtenerCategoriasGastosActivasQuery
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
            new ObtenerCategoriasGastosInactivasQuery
            {
                Filtro = filtro
            }
        );

        return Ok(resultadoCategoriasGastosInactivas);
    }

    // METODO AGREGAR
    [HttpPost]
    public async Task<IActionResult> Agregar([FromBody] AgregarCategoriaGastoCommand command)
    {
        var id = await _mediator.Send(command);

        return Ok(
            new { mensaje = "Categoría de gasto agregada correctamente.", categoriaGastoId = id }
        );
    }

    // METODO ACTUALIZAR
    [HttpPut("{id}")]
    public async Task<IActionResult> Actualizar(
        Guid id,
        [FromBody] ActualizarCategoriaGastoCommand command
    )
    {
        if (id != command.CategoriaGastoId)
        {
            return BadRequest(new { mensaje = "El Id no coincide." });
        }
        await _mediator.Send(command);

        return Ok(new { mensaje = "Categoría de gasto actulizada correctamente." });
    }

    // METODO ACTUALIZAR ESTADO A ACTIVAR
    [HttpPut("activar/{id}")]
    public async Task<IActionResult> Activar(
        Guid id,
        [FromBody] ActivarCategoriaGastoCommand command
    )
    {
        if (id != command.CategoriaGastoId)
        {
            return BadRequest(new { mensaje = "El Id no coincide." });
        }
        
        await _mediator.Send(command);

        return Ok(new { mensaje = "Categoría de gasto activada correctamente." });
    }

    // MEOTODO ACTUALIZAR ESTADO A DESACTIVAR
    [HttpPut("desactivar/{id}")]
    public async Task<IActionResult> Desactivar(
        Guid id,
        [FromBody] DesactivarCategoriaGastoCommand command
    )
    {
        if (id != command.CategoriaGastoId)
        {
            return BadRequest(new { mensaje = "El Id no coincide." });
        }
        
        await _mediator.Send(command);

        return Ok(new { mensaje = "Categoría de gasto desactivada correctamente." });
    }
}
