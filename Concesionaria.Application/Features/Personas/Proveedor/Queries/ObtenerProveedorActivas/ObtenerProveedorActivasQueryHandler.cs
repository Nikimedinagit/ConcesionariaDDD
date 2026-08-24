using Concesionaria.Application.Common.Interfaces;
using MediatR;

namespace Concesionaria.Application.Features.Personas.Proveedor.Queries.ObtenerProveedorActivas;

public class ObtenerProveedorActivasQueryHandler
    : IRequestHandler<ObtenerProveedorActivasQuery, List<ProveedorDto>>
{
    private readonly IProveedorRepository _repository;
    private readonly ICurrentUserService _currentUserService;

    public ObtenerProveedorActivasQueryHandler(IProveedorRepository repository, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }

    public async Task<List<ProveedorDto>> Handle(
        ObtenerProveedorActivasQuery request,
        CancellationToken cancellationToken)
    {
        var empresaId = _currentUserService.EmpresaId;

        var proveedores = await _repository.ObtenerActivasAsync(empresaId, request.Filtro);

        return proveedores
            .OrderBy(p => p.Nombre)
            .Select(p => new ProveedorDto
            {
                ProveedorId = p.Id,
                Nombre = p.Nombre,
                Cuil = p.Cuil,
                Telefono = p.Telefono,
                Email = p.Email,
                Domicilio = p.Domicilio,
                Servicio = p.Servicio,
                Observacion = p.Observacion,
                LocalidadId = p.LocalidadId,

            })
            .ToList();
    }
}
