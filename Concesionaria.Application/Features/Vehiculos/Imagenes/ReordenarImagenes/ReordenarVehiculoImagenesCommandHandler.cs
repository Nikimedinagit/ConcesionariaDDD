using Concesionaria.Application.Common.Interfaces;
using Application.Features.Vehiculos.Imagenes;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Vehiculos.Imagenes.ReordenarImagenes;

public sealed class ReordenarVehiculoImagenesCommandHandler
    : IRequestHandler<ReordenarVehiculoImagenesCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public ReordenarVehiculoImagenesCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task Handle(
        ReordenarVehiculoImagenesCommand request,
        CancellationToken cancellationToken)
    {
        await VehiculoImagenAccess.EnsureVehicleAccessAsync(
            _context,
            _currentUser,
            request.VehiculoId,
            cancellationToken);

        var images = await _context.VehiculoImagenes
            .Where(image => image.VehiculoId == request.VehiculoId)
            .ToListAsync(cancellationToken);

        if (images.Count != request.Imagenes.Count ||
            request.Imagenes.Any(item => images.All(image => image.Id != item.ImagenId)))
        {
            throw new InvalidOperationException(
                "Debe enviar todas las imágenes activas del vehículo para reordenarlas.");
        }

        var requestedOrder = request.Imagenes.ToDictionary(item => item.ImagenId, item => item.Orden);

        foreach (var image in images)
            image.CambiarOrden(requestedOrder[image.Id]);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
