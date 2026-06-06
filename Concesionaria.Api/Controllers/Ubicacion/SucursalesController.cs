using Application.Features.Ubicaciones.Queries.ObtenerSucursalesActivas;
using Application.Features.Ubicaciones.Queries.ObtenerSucursalesInactivas;
using Concesionaria.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Concesionaria.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class SucursalesController : ControllerBase
{
    private readonly IMediator _mediator;

    public SucursalesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // METODO PARA OBTENER ACTIVAS
    [HttpGet("activas")]
    public async Task<ActionResult> ObtenerActivas([FromQuery] string filtro)
    {
        var resultadoSucursalesActivas = await _mediator.Send(
            new ObtenerSucursalesActivasQuery { Filtro = filtro }
        );

        return Ok(resultadoSucursalesActivas);
    }

    // METODO PARA OBTENER INACTIVAS

    [HttpGet("inactivas")]
    public async Task<ActionResult> ObtenerInactivas([FromQuery] string filtro)
    {
        var resultadoSucursalesInactivas = await _mediator.Send(
            new ObtenerSucursalesInactivasQuery { Filtro = filtro }
        );

        return Ok(resultadoSucursalesInactivas);
    }
}
