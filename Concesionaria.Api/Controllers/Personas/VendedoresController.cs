using Concesionaria.Application.Features.Personas.Vendedores.Queries.ObtenerVendedoresActivas;
using Concesionaria.Application.Features.Personas.Vendedores.Queries.ObtenerVendedoresInactivas;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Concesionaria.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class VendedoresController : ControllerBase
{
    private readonly IMediator _mediator;

    public VendedoresController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // METODO OBTENER ACTIVAS
    [HttpGet("activas")]
    public async Task<IActionResult> ObtenerActivas([FromQuery] string filtro)
    {
        var resultadoVendedoresActivas = await _mediator.Send(new ObtenerVendedoresActivasQuery
        {
            Filtro = filtro
        });

        return Ok(resultadoVendedoresActivas);
    }
    // METODO OBTENER INACTIVAS
    [HttpGet("inactivas")]
    public async Task<IActionResult> ObtenerInactivas([FromQuery] string filtro)
    {
        var resultadoVendedoresInactivas = await _mediator.Send(new ObtenerVendedoresInactivasQuery
        {
            Filtro = filtro
        });

        return Ok(resultadoVendedoresInactivas);
    }
}