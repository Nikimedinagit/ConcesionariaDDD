using Concesionaria.Application.Common.Interfaces;
using MediatR;

namespace Application.Features.Vehiculos.Queries.ObtenerVehiculosDisponibles;

public class ObtenerVehiculosDisponiblesQueryHandler
    : IRequestHandler<ObtenerVehiculosDisponiblesQuery, List<VehiculoDto>>
{
    private readonly IVehiculoRepository _repository;
    private readonly ICurrentUserService _currentUserService;


    public ObtenerVehiculosDisponiblesQueryHandler(IVehiculoRepository repository, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }

    public async Task<List<VehiculoDto>> Handle(
    ObtenerVehiculosDisponiblesQuery request,
    CancellationToken cancellationToken)
    {
        var empresaId = _currentUserService.EmpresaId;
        var sucursalId = _currentUserService.SucursalId;

        if (sucursalId == Guid.Empty)
            throw new UnauthorizedAccessException("El usuario no tiene una sucursal asignada.");

        var vehiculos = await _repository.ObtenerDisponiblesAsync(
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
                SucursalId = v.SucursalId
            })
            .ToList();
    }
}
