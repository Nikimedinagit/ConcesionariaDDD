using Concesionaria.Application.Common.Interfaces;
using Application.Features.Vehiculos.Imagenes;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Application.Features.Vehiculos.Imagenes.EliminarImagen;

public sealed class EliminarVehiculoImagenCommandHandler
    : IRequestHandler<EliminarVehiculoImagenCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IFileStorage _fileStorage;
    private readonly ILogger<EliminarVehiculoImagenCommandHandler> _logger;

    public EliminarVehiculoImagenCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser,
        IFileStorage fileStorage,
        ILogger<EliminarVehiculoImagenCommandHandler> logger)
    {
        _context = context;
        _currentUser = currentUser;
        _fileStorage = fileStorage;
        _logger = logger;
    }

    public async Task Handle(
        EliminarVehiculoImagenCommand request,
        CancellationToken cancellationToken)
    {
        await VehiculoImagenAccess.EnsureVehicleAccessAsync(
            _context,
            _currentUser,
            request.VehiculoId,
            cancellationToken);

        await using var transaction = await _context.BeginTransactionAsync(
            IsolationLevel.Serializable,
            cancellationToken);

        var image = await _context.VehiculoImagenes.FirstOrDefaultAsync(
            current => current.Id == request.ImagenId &&
                       current.VehiculoId == request.VehiculoId,
            cancellationToken);

        if (image is null)
            throw new KeyNotFoundException("La imagen no existe o no está disponible.");

        var wasMain = image.EsPrincipal;
        image.Desactivar();
        await _context.SaveChangesAsync(cancellationToken);

        if (wasMain)
        {
            var nextImage = await _context.VehiculoImagenes
                .Where(current => current.VehiculoId == request.VehiculoId &&
                                  current.Id != request.ImagenId)
                .OrderBy(current => current.Orden)
                .FirstOrDefaultAsync(cancellationToken);

            nextImage?.MarcarComoPrincipal();

            if (nextImage is not null)
                await _context.SaveChangesAsync(cancellationToken);
        }

        await transaction.CommitAsync(cancellationToken);

        try
        {
            await _fileStorage.DeleteAsync(image.StorageKey, cancellationToken);
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            _logger.LogWarning(
                exception,
                "La imagen {ImageId} fue desactivada, pero el archivo {StorageKey} no pudo eliminarse del almacenamiento.",
                image.Id,
                image.StorageKey);
        }
    }
}
