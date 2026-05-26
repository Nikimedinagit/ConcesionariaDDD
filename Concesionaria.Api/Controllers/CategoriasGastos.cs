using Application.Features.CategoriasGastos.Queries.ObtenerCategoriasGastosActivas;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Concesionaria.API.Controllers;

[ApiController]
[Route("api/[controller]")]
// [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CategoriasGastosController : ControllerBase
{
    // private readonly ObtenerCategoriasGastosActivasQuery _handler;
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
}
