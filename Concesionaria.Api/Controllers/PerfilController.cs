using Concesionaria.Application.Perfil.Commands.ActualizarEmpresa;
using Concesionaria.Application.Perfil.Commands.ActualizarUsuario;
using Concesionaria.Application.Perfil.Queries.GetPerfil;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Concesionaria.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class PerfilController : ControllerBase
{
    private readonly IMediator _mediator;

    public PerfilController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetPerfil()
    {
        var result = await _mediator.Send(
            new GetPerfilQuery());

        return Ok(result);
    }

    [HttpPut("empresa")]
    public async Task<IActionResult> ActualizarEmpresa(
        ActualizarEmpresaCommand command)
    {
        var result = await _mediator.Send(command);

        return Ok(result);
    }

    [HttpPut("usuario")]
    public async Task<IActionResult> ActualizarUsuario(
        ActualizarUsuarioCommand command)
    {
        var result = await _mediator.Send(command);

        return Ok(result);
    }
}