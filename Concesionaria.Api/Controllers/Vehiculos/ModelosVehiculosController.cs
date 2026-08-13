using Application.Features.Vehiculos.Queries.ObtenerModelosVehiculosActivas;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Concesionaria.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ModelosVehiculosController : ControllerBase
{
    private readonly IMediator _mediator;

    public ModelosVehiculosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // METODO OBTENER ACTIVAS
    [HttpGet("activas")]
    public async Task<IActionResult> ObtenerActivas([FromQuery] string filtro)
    {
        var resultadoModelosActivas = await _mediator.Send(new ObtenerModelosVehiculosActivasQuery
        {
            Filtro = filtro
        });

        return Ok(resultadoModelosActivas);
    }
}
