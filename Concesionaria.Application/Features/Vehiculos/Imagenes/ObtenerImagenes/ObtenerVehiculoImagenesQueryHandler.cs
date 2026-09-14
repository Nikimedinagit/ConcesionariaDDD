using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Application.DTOs.Vehiculos;
using Application.Features.Vehiculos.Imagenes;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Vehiculos.Imagenes.ObtenerImagenes;

public sealed class ObtenerVehiculoImagenesQueryHandler
    : IRequestHandler<ObtenerVehiculoImagenesQuery, List<VehiculoImagenDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IFileStorage _fileStorage;

    public ObtenerVehiculoImagenesQueryHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser,
        IFileStorage fileStorage)
    {
        _context = context;
        _currentUser = currentUser;
        _fileStorage = fileStorage;
    }

    public async Task<List<VehiculoImagenDto>> Handle(
        ObtenerVehiculoImagenesQuery request,
        CancellationToken cancellationToken)
    {
        await VehiculoImagenAccess.EnsureVehicleAccessAsync(
            _context,
            _currentUser,
            request.VehiculoId,
            cancellationToken);

        var images = await _context.VehiculoImagenes
            .AsNoTracking()
            .Where(image => image.VehiculoId == request.VehiculoId)
            .OrderByDescending(image => image.EsPrincipal)
            .ThenBy(image => image.Orden)
            .ToListAsync(cancellationToken);

        var mappedImages = await Task.WhenAll(images.Select(async image =>
        {
            var url = await _fileStorage.GetReadUrlAsync(
                image.StorageKey,
                cancellationToken);

            return new VehiculoImagenDto(
                image.Id,
                image.VehiculoId,
                url,
                image.NombreOriginal,
                image.ContentType,
                image.TamanioBytes,
                image.Ancho,
                image.Alto,
                image.Orden,
                image.EsPrincipal,
                image.CreatedAt);
        }));

        return mappedImages.ToList();
    }
}
