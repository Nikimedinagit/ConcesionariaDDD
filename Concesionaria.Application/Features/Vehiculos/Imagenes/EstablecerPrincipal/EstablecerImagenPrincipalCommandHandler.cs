using Concesionaria.Application.Common.Interfaces;
using Application.Features.Vehiculos.Imagenes;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Application.Features.Vehiculos.Imagenes.EstablecerPrincipal;

public sealed class EstablecerImagenPrincipalCommandHandler
    : IRequestHandler<EstablecerImagenPrincipalCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public EstablecerImagenPrincipalCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task Handle(
        EstablecerImagenPrincipalCommand request,
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

        var images = await _context.VehiculoImagenes
            .Where(image => image.VehiculoId == request.VehiculoId)
            .ToListAsync(cancellationToken);

        var selectedImage = images.FirstOrDefault(image => image.Id == request.ImagenId);

        if (selectedImage is null)
            throw new KeyNotFoundException("La imagen no existe o no está disponible.");

        if (selectedImage.EsPrincipal)
            return;

        foreach (var image in images.Where(image => image.EsPrincipal))
            image.QuitarComoPrincipal();

        await _context.SaveChangesAsync(cancellationToken);

        selectedImage.MarcarComoPrincipal();
        await _context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
    }
}
