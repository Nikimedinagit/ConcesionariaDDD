using Application.Features.CategoriasGastos.Commands.ActualizarCategoriaGasto;
using Application.Features.CategoriasGastos.Commands.AgregarCategoriaGasto;
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
    public async Task<IActionResult> ObtenerActivas()
    {
        var resultadoCategoriasGastosActivas = await _mediator.Send(
            new ObtenerCategoriasGastosActivasQuery()
        );

        return Ok(resultadoCategoriasGastosActivas);
    }

    // METODO OBTENER INACTIVAS
    [HttpGet("inactivas")]
    public async Task<IActionResult> ObtenerInactivas()
    {
        var resultadoCategoriasGastosInactivas = await _mediator.Send(
            new ObtenerCategoriasGastosInactivasQuery()
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
}
