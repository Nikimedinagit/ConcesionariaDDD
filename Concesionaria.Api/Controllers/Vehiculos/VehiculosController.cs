using Application.Features.Vehiculos.Queries.ObtenerVehiculosActivas;
using Application.Features.Vehiculos.Queries.ObtenerVehiculosInactivas;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Concesionaria.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class VehiculosController : ControllerBase
{
    private readonly IMediator _mediator;

    public VehiculosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // METODO OBTENER ACTIVAS
    [HttpGet("activas")]
    public async Task<IActionResult> ObtenerActivas([FromQuery] string filtro)
    {
        var resultadoVehiculosActivas = await _mediator.Send(
            new ObtenerVehiculosActivasQuery { Filtro = filtro }
        );

        return Ok(resultadoVehiculosActivas);
    }

    // METODO OBTENER INACTIVAS
    [HttpGet("inactivas")]
    public async Task<IActionResult> ObtenerInactivas([FromQuery] string filtro)
    {
        var resultadoVehiculosInactivas = await _mediator.Send(
            new ObtenerVehiculosInactivasQuery { Filtro = filtro }
        );

        return Ok(resultadoVehiculosInactivas);
    }
}
