using Application.Features.Vehiculos.Commands.ActualizarVehiculo;
using Application.Features.Vehiculos.Commands.AgregarVehiculo;
using Application.Features.Vehiculos.Commands.EliminarVehiculo;
using Application.Features.Vehiculos.Imagenes.AgregarImagen;
using Application.Features.Vehiculos.Imagenes.EliminarImagen;
using Application.Features.Vehiculos.Imagenes.EstablecerPrincipal;
using Application.Features.Vehiculos.Imagenes.ObtenerImagenes;
using Application.Features.Vehiculos.Imagenes.ReordenarImagenes;
using Application.Features.Vehiculos.Queries.ObtenerVehiculosDisponibles;
using Application.Features.Vehiculos.Queries.ObtenerVehiculosEnServicio;
using Application.Features.Vehiculos.Queries.ObtenerVehiculosReservados;
using Application.Features.Vehiculos.Queries.ObtenerVehiculosVendidos;
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

    // METODO OBTENER DISPONIBLES
    [HttpGet("disponibles")]
    public async Task<IActionResult> ObtenerDisponibles([FromQuery] string filtro, [FromQuery] Guid? sucursalId, [FromQuery] bool todasSucursales = false)
    {
        var resultadoVehiculosDisponibles = await _mediator.Send(
            new ObtenerVehiculosDisponiblesQuery { Filtro = filtro, SucursalId = sucursalId, TodasSucursales = todasSucursales }
        );

        return Ok(resultadoVehiculosDisponibles);
    }
    // METODO OBTENER RESERVADOS
    [HttpGet("reservados")]
    public async Task<IActionResult> ObtenerReservados([FromQuery] string filtro, [FromQuery] Guid? sucursalId, [FromQuery] bool todasSucursales = false)
    {
        var resultadoVehiculosReservados = await _mediator.Send(
            new ObtenerVehiculosReservadosQuery { Filtro = filtro, SucursalId = sucursalId, TodasSucursales = todasSucursales }
        );

        return Ok(resultadoVehiculosReservados);
    }
    
    // METODO OBTENER EN SERVICIO
    [HttpGet("servicio")]
    public async Task<IActionResult> ObtenerEnServicio([FromQuery] string filtro, [FromQuery] Guid? sucursalId, [FromQuery] bool todasSucursales = false)
    {
        var resultadoVehiculosEnServicio = await _mediator.Send(
            new ObtenerVehiculosEnServicioQuery { Filtro = filtro, SucursalId = sucursalId, TodasSucursales = todasSucursales }
        );

        return Ok(resultadoVehiculosEnServicio);
    }

    // METODO OBTENER VENDIDOS
    [HttpGet("vendidos")]
    public async Task<IActionResult> ObtenerVendidos([FromQuery] string filtro, [FromQuery] Guid? sucursalId, [FromQuery] bool todasSucursales = false)
    {
        var resultadoVehiculosVendidos = await _mediator.Send(
            new ObtenerVehiculosVendidosQuery { Filtro = filtro, SucursalId = sucursalId, TodasSucursales = todasSucursales }
        );

        return Ok(resultadoVehiculosVendidos);
    }

    //METODO AGREGAR VEHICULO
    [HttpPost]
    public async Task<IActionResult> Agregar([FromBody] AgregarVehiculoCommand command)
    {
        var id = await _mediator.Send(command);

        return Ok(
            new { mensaje = "Vehículo agregado correctamente.", vehiculoId = id }
        );
    }

      // METODO ACTUALIZAR
    [HttpPut("{id}")]
    public async Task<IActionResult> Actualizar(
        Guid id,
        [FromBody] ActualizarVehiculoCommand command
    )
    {
        if (id != command.VehiculoId)
        {
            return BadRequest(new { mensaje = "El Id no coincide." });
        }
        await _mediator.Send(command);

        return Ok(new { mensaje = "Vehículo actualizado correctamente." });
    }

      // METODO ELIMINAR
    [HttpDelete("{id}")]
    public async Task<IActionResult> Eliminar(Guid id)
    {
        await _mediator.Send(new EliminarVehiculoCommand { VehiculoId = id });

        return Ok(new { mensaje = "Vehículo eliminado correctamente." });
    }

    [HttpGet("{vehiculoId:guid}/imagenes")]
    public async Task<IActionResult> ObtenerImagenes(
        Guid vehiculoId,
        CancellationToken cancellationToken)
    {
        var images = await _mediator.Send(
            new ObtenerVehiculoImagenesQuery(vehiculoId),
            cancellationToken);

        return Ok(images);
    }

    [HttpPost("{vehiculoId:guid}/imagenes")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> AgregarImagen(
        Guid vehiculoId,
        [FromForm] AgregarVehiculoImagenRequest request,
        CancellationToken cancellationToken)
    {
        if (request.Imagen is null)
            return BadRequest(new { mensaje = "La imagen es obligatoria." });

        await using var content = request.Imagen.OpenReadStream();

        var image = await _mediator.Send(
            new AgregarVehiculoImagenCommand
            {
                VehiculoId = vehiculoId,
                Content = content,
                NombreOriginal = request.Imagen.FileName,
                ContentType = request.Imagen.ContentType,
                EsPrincipal = request.EsPrincipal
            },
            cancellationToken);

        return Ok(new
        {
            mensaje = "Imagen agregada correctamente.",
            imagen = image
        });
    }

    [HttpDelete("{vehiculoId:guid}/imagenes/{imagenId:guid}")]
    public async Task<IActionResult> EliminarImagen(
        Guid vehiculoId,
        Guid imagenId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new EliminarVehiculoImagenCommand(vehiculoId, imagenId),
            cancellationToken);

        return Ok(new { mensaje = "Imagen eliminada correctamente." });
    }

    [HttpPatch("{vehiculoId:guid}/imagenes/{imagenId:guid}/principal")]
    public async Task<IActionResult> EstablecerImagenPrincipal(
        Guid vehiculoId,
        Guid imagenId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new EstablecerImagenPrincipalCommand(vehiculoId, imagenId),
            cancellationToken);

        return Ok(new { mensaje = "Imagen principal actualizada correctamente." });
    }

    [HttpPut("{vehiculoId:guid}/imagenes/orden")]
    public async Task<IActionResult> ReordenarImagenes(
        Guid vehiculoId,
        [FromBody] ReordenarVehiculoImagenesRequest request,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new ReordenarVehiculoImagenesCommand
            {
                VehiculoId = vehiculoId,
                Imagenes = request.Imagenes
            },
            cancellationToken);

        return Ok(new { mensaje = "Imágenes reordenadas correctamente." });
    }
}

public sealed record AgregarVehiculoImagenRequest
{
    public IFormFile Imagen { get; init; }
    public bool EsPrincipal { get; init; }
}

public sealed record ReordenarVehiculoImagenesRequest
{
    public IReadOnlyCollection<VehiculoImagenOrdenDto> Imagenes { get; init; } =
        Array.Empty<VehiculoImagenOrdenDto>();
}
