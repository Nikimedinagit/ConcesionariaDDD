using Concesionaria.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Concesionaria.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UbicacionesController : ControllerBase
{
    private readonly ILocalidadService _service;

    public UbicacionesController(ILocalidadService service)
    {
        _service = service;
    }


    // GET: api/ubicaciones/localidades
    [HttpGet("localidades")] 
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<LocalidadDto>>> GetAllLocalidades()
    {
        return Ok(await _service.GetAllLocalidadesAsync());
    }
}