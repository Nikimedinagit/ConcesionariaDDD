using Application.Features.Vehiculos.Queries.ObtenerVehiculosActivas;
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
    public async Task<IActionResult> ObtenerActivas(
        [FromQuery] string filtro)
    {
        var resultadoVehiculosActivas = await _mediator.Send(new ObtenerVehiculosActivasQuery
        {
            Filtro = filtro,
        });

        return Ok(resultadoVehiculosActivas);
    }
}
