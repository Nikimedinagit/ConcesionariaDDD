using Concesionaria.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Vehiculos.Imagenes;

internal static class VehiculoImagenAccess
{
    public static async Task EnsureVehicleAccessAsync(
        IApplicationDbContext context,
        ICurrentUserService currentUser,
        Guid vehicleId,
        CancellationToken cancellationToken)
    {
        var hasAccess = await context.Vehiculos
            .IgnoreQueryFilters()
            .AnyAsync(
                vehicle => vehicle.Id == vehicleId &&
                           vehicle.EmpresaId == currentUser.EmpresaId &&
                           !vehicle.Eliminado &&
                           (currentUser.EsAdministrador ||
                            vehicle.SucursalId == currentUser.SucursalId),
                cancellationToken);

        if (!hasAccess)
            throw new KeyNotFoundException("El vehículo no existe o no está disponible.");
    }
}
