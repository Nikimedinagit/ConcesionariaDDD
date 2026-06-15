using Application.Features.Cuentas.Commands.AgregarCuenta;
using Application.Features.Cuentas.Queries.ObtenerCuentasActivas;
using Application.Features.Cuentas.Queries.ObtenerCuentasInactivas;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Concesionaria.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CuentasController : ControllerBase
{
    // private readonly ObtenerCuentasActivasQuery _handler;
    private readonly IMediator _mediator;

    public CuentasController(IMediator mediator)
    {
        _mediator = mediator;
    }

    //METODO OBTENER ACTIVAS
    [HttpGet("activas")]
    public async Task<IActionResult> ObtenerActivas([FromQuery] string filtro)
    {
        var resultadoCuentasActivas = await _mediator.Send(
            new ObtenerCuentasActivasQuery { Filtro = filtro }
        );

        return Ok(resultadoCuentasActivas);
    }

    // METODO PARA OBTENER INACTIVAS
    [HttpGet("inactivas")]
    public async Task<IActionResult> ObtenerInactivas([FromQuery] string filtro)
    {
        var resultadoCuentasInactivas = await _mediator.Send(
            new ObtenerCuentasInactivasQuery { Filtro = filtro }
        );

        return Ok(resultadoCuentasInactivas);
    }

    // METODO PARA AGREGAR CUENTA
    [HttpPost]
    public async Task<IActionResult> Agregar([FromBody] AgregarCuentaCommand command)
    {
        var id = await _mediator.Send(command);

        return Ok(
            new { mensaje = "Cuenta agregada correctamente.", cuentaId = id }
        );
    }
}
