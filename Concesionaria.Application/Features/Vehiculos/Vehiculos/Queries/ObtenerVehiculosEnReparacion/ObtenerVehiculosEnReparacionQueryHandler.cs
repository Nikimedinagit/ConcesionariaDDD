using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Interfaces.IRepositories;
using MediatR;

namespace Application.Features.Vehiculos.Queries.ObtenerVehiculosEnServicio;

public class ObtenerVehiculosEnServicioQueryHandler
    : IRequestHandler<ObtenerVehiculosEnServicioQuery, List<VehiculoDto>>
{
    private readonly IVehiculoRepository _repository;
    private readonly ICurrentUserService _currentUserService;

    public ObtenerVehiculosEnServicioQueryHandler(
        IVehiculoRepository repository,
        ICurrentUserService currentUserService
    )
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }

    public async Task<List<VehiculoDto>> Handle(
        ObtenerVehiculosEnServicioQuery request,
        CancellationToken cancellationToken
    )
    {
        var empresaId = _currentUserService.EmpresaId;
        var sucursalActualId = _currentUserService.SucursalId;

        if (sucursalActualId == Guid.Empty)
            throw new UnauthorizedAccessException("El usuario no tiene una sucursal asignada.");

        Guid? sucursalId = request.TodasSucursales
            ? null
            : request.SucursalId ?? sucursalActualId;

        var vehiculos = await _repository.ObtenerEnServicioAsync(
            empresaId,
            sucursalId,
            request.Filtro);

        return vehiculos
            .OrderBy(v => v.Anio)
            .Select(v => new VehiculoDto
            {
                VehiculoId = v.Id,
                Version = v.Version,
                Patente = v.Patente,
                Color = v.Color,
                Anio = v.Anio,
                Kilometraje = v.Kilometraje,
                Condicion = v.Condicion,
                Estado = v.Estado,
                PrecioCompra = v.PrecioCompra,
                PrecioVenta = v.PrecioVenta,
                ModeloId = v.ModeloId,
                ModeloNombre = v.Modelo.Nombre,
                MarcaNombre = v.Modelo.MarcaVehiculo.Nombre,
                TipoVehiculoNombre = v.Modelo.TipoVehiculo.Nombre,
                SucursalId = v.SucursalId,
                SucursalNombre = v.Sucursal.Nombre,
            })
            .ToList();
    }
}
