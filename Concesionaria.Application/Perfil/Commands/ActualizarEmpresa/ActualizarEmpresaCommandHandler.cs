using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Empresas.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Concesionaria.Application.Perfil.Commands.ActualizarEmpresa;

public class ActualizarEmpresaCommandHandler
    : IRequestHandler<ActualizarEmpresaCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public ActualizarEmpresaCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(
        ActualizarEmpresaCommand request,
        CancellationToken cancellationToken)
    {
        var empresa = await _context.Empresas
            .FirstOrDefaultAsync(
                x => x.Id == request.EmpresaId,
                cancellationToken);

        if (empresa == null)
            throw new Exception("Empresa no encontrada");

        empresa.ActualizarNombreFantasia(
            request.NombreFantasia);

        empresa.ActualizarLocalidad(
            request.LocalidadId);

        empresa.ActualizarMoneda(
            Enum.Parse<Moneda>(request.Moneda));

        await _context.SaveChangesAsync(
            cancellationToken);

        return true;
    }
}