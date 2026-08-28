using Concesionaria.Application.Common.Interfaces;
using MediatR;

namespace Application.Features.Vehiculos.Queries.ObtenerVehiculosActivas;

public class ObtenerVehiculosActivasQueryHandler
    : IRequestHandler<ObtenerVehiculosActivasQuery, List<VehiculoDto>>
{
    private readonly IVehiculoRepository _repository;
    private readonly ICurrentUserService _currentUserService;


    public ObtenerVehiculosActivasQueryHandler(IVehiculoRepository repository, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }

    public async Task<List<VehiculoDto>> Handle(
    ObtenerVehiculosActivasQuery request,
    CancellationToken cancellationToken)
    {
        var empresaId = _currentUserService.EmpresaId;

        var vehiculos = await _repository.ObtenerActivasAsync(
            empresaId,
            request.Filtro);

        return vehiculos
            .OrderBy(v => v.PrecioVenta)
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
