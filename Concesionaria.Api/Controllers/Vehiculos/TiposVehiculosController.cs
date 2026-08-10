using Application.Features.Vehiculos.Queries.ObtenerTiposVehiculosActivas;
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
}
