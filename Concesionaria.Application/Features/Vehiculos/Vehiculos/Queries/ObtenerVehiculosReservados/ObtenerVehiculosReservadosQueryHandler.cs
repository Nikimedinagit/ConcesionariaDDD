using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Interfaces.IRepositories;
using MediatR;

namespace Application.Features.Vehiculos.Queries.ObtenerVehiculosReservados;

public class ObtenerVehiculosReservadosQueryHandler
    : IRequestHandler<ObtenerVehiculosReservadosQuery, List<VehiculoDto>>
{
    private readonly IVehiculoRepository _repository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IFileStorage _fileStorage;

    public ObtenerVehiculosReservadosQueryHandler(
        IVehiculoRepository repository,
        ICurrentUserService currentUserService,
        IFileStorage fileStorage
    )
    {
        _repository = repository;
        _currentUserService = currentUserService;
        _fileStorage = fileStorage;
    }

    public async Task<List<VehiculoDto>> Handle(
        ObtenerVehiculosReservadosQuery request,
        CancellationToken cancellationToken
    )
    {
        var empresaId = _currentUserService.EmpresaId;
        var sucursalActualId = _currentUserService.SucursalId;

        if (sucursalActualId == Guid.Empty)
            throw new UnauthorizedAccessException("El usuario no tiene una sucursal asignada.");

        Guid? sucursalId = _currentUserService.EsAdministrador
            ? request.TodasSucursales
                ? null
                : request.SucursalId ?? sucursalActualId
            : sucursalActualId;

        var vehiculos = await _repository.ObtenerReservadosAsync(
            empresaId,
            sucursalId,
            request.Filtro);

        var result = vehiculos
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

        foreach (var dto in result)
        {
            var principal = vehiculos.First(v => v.Id == dto.VehiculoId).Imagenes
                .FirstOrDefault(imagen => imagen.EsPrincipal);

            if (principal is not null)
                dto.ImagenPrincipalUrl = await _fileStorage.GetReadUrlAsync(principal.StorageKey, cancellationToken);
        }

        return result;
    }
}
