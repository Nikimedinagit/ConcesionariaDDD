using Application.Features.Cuentas.Queries.ObtenerCuentasActivas;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Concesionaria.API.Controllers;

[ApiController]
[Route("api/[controller]")]
// [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CuentasController : ControllerBase
{
    // private readonly ObtenerCuentasActivasQuery _handler;
    private readonly IMediator _mediator;

    public CuentasController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("activas")]
    public async Task<IActionResult> ObtenerActivas()
    {
        var resultadoCuentasActivas = await _mediator.Send(
            new ObtenerCuentasActivasQuery()
        );

        return Ok(resultadoCuentasActivas);
    }

    // [HttpGet("inactivas")]
    // public async Task<IActionResult> ObtenerInactivas()
    // {
    //     var resultadoCuentasInactivas = await _mediator.Send(
    //         new ObtenerCuentasInactivasQuery()
    //     );

    //     return Ok(resultadoCuentasInactivas);
    // }
}
