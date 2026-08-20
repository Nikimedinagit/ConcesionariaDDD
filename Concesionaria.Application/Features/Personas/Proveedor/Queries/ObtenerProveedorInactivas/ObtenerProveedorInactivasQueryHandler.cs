using Concesionaria.Application.Common.Interfaces;
using MediatR;

namespace Concesionaria.Application.Features.Personas.Proveedor.Queries.ObtenerProveedorInactivas;

public class ObtenerProveedorInactivasQueryHandler
    : IRequestHandler<ObtenerProveedorInactivasQuery, List<ProveedorDto>>
{
    private readonly IProveedorRepository _repository;
    private readonly ICurrentUserService _currentUserService;

    public ObtenerProveedorInactivasQueryHandler(IProveedorRepository repository, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }

    public async Task<List<ProveedorDto>> Handle(
        ObtenerProveedorInactivasQuery request,
        CancellationToken cancellationToken)
    {
        var empresaId = _currentUserService.EmpresaId;

        var proveedores = await _repository.ObtenerInactivasAsync(empresaId, request.Filtro);

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
                Observacion = p.Observacion
            })
            .ToList();
    }
}
