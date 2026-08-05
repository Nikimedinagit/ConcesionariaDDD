using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Cuentas;
using Concesionaria.Domain.Cuentas.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Cuentas.Queries.ObtenerCuentasActivas;

public class ObtenerCuentasActivasQueryHandler
    : IRequestHandler<ObtenerCuentasActivasQuery, List<CuentaDto>>
{
    private readonly ICuentaRepository _repository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IApplicationDbContext _context;

    public ObtenerCuentasActivasQueryHandler(
        ICuentaRepository repository,
        ICurrentUserService currentUserService,
        IApplicationDbContext context)
    {
        _repository = repository;
        _currentUserService = currentUserService;
        _context = context;
    }

    public async Task<List<CuentaDto>> Handle(
        ObtenerCuentasActivasQuery request,
        CancellationToken cancellationToken)
    {
        var empresaId = _currentUserService.EmpresaId;

        await AsegurarCuentasBaseAsync(empresaId, cancellationToken);

        var cuentas = await _repository.ObtenerActivasAsync(empresaId, request.Filtro);

        return cuentas
            .Select(c => new CuentaDto
            {
                CuentaId = c.Id,
                Codigo = c.Codigo,
                Nombre = c.Nombre,
                Tipo = c.Tipo,
                Nivel = c.Nivel,
                CuentaPadreId = c.CuentaPadreId
            })
            .OrderBy(c => c.Codigo)
            .ToList();
    }

    private async Task AsegurarCuentasBaseAsync(
        Guid empresaId,
        CancellationToken cancellationToken)
    {
        var cuentasBase = new[]
        {
            new { Codigo = "1", Nombre = "ACTIVO", Tipo = TipoCuenta.ACTIVO },
            new { Codigo = "2", Nombre = "PASIVO", Tipo = TipoCuenta.PASIVO },
            new { Codigo = "3", Nombre = "PATRIMONIO NETO", Tipo = TipoCuenta.PATRIMONIO },
            new { Codigo = "4", Nombre = "INGRESO", Tipo = TipoCuenta.INGRESO },
            new { Codigo = "5", Nombre = "EGRESO", Tipo = TipoCuenta.EGRESO },
        };

        var existentes = await _context.Cuentas
            .IgnoreQueryFilters()
            .Where(c => c.EmpresaId == empresaId && c.Nivel == 0)
            .Select(c => new { c.Codigo, c.Nombre })
            .ToListAsync(cancellationToken);

        foreach (var cuentaBase in cuentasBase)
        {
            var existe = existentes.Any(c =>
                c.Codigo == cuentaBase.Codigo || c.Nombre == cuentaBase.Nombre);

            if (!existe)
            {
                var cuenta = Cuenta.Crear(
                    empresaId,
                    cuentaBase.Codigo,
                    cuentaBase.Nombre,
                    cuentaBase.Tipo,
                    0);

                await _context.Cuentas.AddAsync(cuenta, cancellationToken);
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
