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

    [HttpGet("activas")]
    public async Task<IActionResult> ObtenerActivas()
    {
        var resultadoCategoriasGastosActivas = await _mediator.Send(
            new ObtenerCategoriasGastosActivasQuery()
        );

        return Ok(resultadoCategoriasGastosActivas);
    }

    [HttpGet("inactivas")]
    public async Task<IActionResult> ObtenerInactivas()
    {
        var resultadoCategoriasGastosInactivas = await _mediator.Send(
            new ObtenerCategoriasGastosInactivasQuery()
        );

        return Ok(resultadoCategoriasGastosInactivas);
    }

    [HttpPost]
    public async Task<IActionResult> Agregar([FromBody] AgregarCategoriaGastoCommand command)
    {
        var id = await _mediator.Send(command);

        return Ok(
            new { mensaje = "Categoría de gasto creada correctamente.", categoriaGastoId = id }
        );
    }
}
