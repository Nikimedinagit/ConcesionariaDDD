using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Application.DTOs.Vehiculos;
using Application.Features.Vehiculos.Imagenes;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Application.Features.Vehiculos.Imagenes.AgregarImagen;

public sealed class AgregarVehiculoImagenCommandHandler
    : IRequestHandler<AgregarVehiculoImagenCommand, VehiculoImagenDto>
{
    private const int MaxImagesPerVehicle = 10;

    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IImageProcessor _imageProcessor;
    private readonly IFileStorage _fileStorage;

    public AgregarVehiculoImagenCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser,
        IImageProcessor imageProcessor,
        IFileStorage fileStorage)
    {
        _context = context;
        _currentUser = currentUser;
        _imageProcessor = imageProcessor;
        _fileStorage = fileStorage;
    }

    public async Task<VehiculoImagenDto> Handle(
        AgregarVehiculoImagenCommand request,
        CancellationToken cancellationToken)
    {
        await VehiculoImagenAccess.EnsureVehicleAccessAsync(
            _context,
            _currentUser,
            request.VehiculoId,
            cancellationToken);

        var empresaId = _currentUser.EmpresaId;

        await using var processedImage = await _imageProcessor.ProcessAsync(
            request.Content,
            request.NombreOriginal,
            request.ContentType,
            cancellationToken);

        var storageKey = BuildStorageKey(
            empresaId,
            request.VehiculoId,
            processedImage.FileExtension);

        var fileUploaded = false;

        try
        {
            var storedFile = await _fileStorage.UploadAsync(
                processedImage.Content,
                storageKey,
                processedImage.ContentType,
                processedImage.SizeBytes,
                cancellationToken);

            fileUploaded = true;

            var url = await _fileStorage.GetReadUrlAsync(
                storedFile.StorageKey,
                cancellationToken);

            await using var transaction = await _context.BeginTransactionAsync(
                IsolationLevel.Serializable,
                cancellationToken);

            var currentImages = await _context.VehiculoImagenes
                .Where(image => image.VehiculoId == request.VehiculoId)
                .OrderBy(image => image.Orden)
                .ToListAsync(cancellationToken);

            if (currentImages.Count >= MaxImagesPerVehicle)
            {
                throw new InvalidOperationException(
                    $"El vehículo no puede tener más de {MaxImagesPerVehicle} imágenes.");
            }

            var isMain = request.EsPrincipal || currentImages.Count == 0;

            if (isMain)
            {
                var hadMainImage = currentImages.Any(image => image.EsPrincipal);

                foreach (var image in currentImages.Where(image => image.EsPrincipal))
                    image.QuitarComoPrincipal();

                if (hadMainImage)
                    await _context.SaveChangesAsync(cancellationToken);
            }

            var nextOrder = currentImages.Count == 0
                ? 0
                : currentImages.Max(image => image.Orden) + 1;

            var vehicleImage = VehiculoImagen.Crear(
                empresaId,
                request.VehiculoId,
                storedFile.StorageKey,
                Path.GetFileName(request.NombreOriginal),
                storedFile.ContentType,
                storedFile.SizeBytes,
                processedImage.Width,
                processedImage.Height,
                nextOrder,
                isMain);

            await _context.VehiculoImagenes.AddAsync(vehicleImage, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return Map(vehicleImage, url);
        }
        catch
        {
            if (fileUploaded)
            {
                try
                {
                    await _fileStorage.DeleteAsync(storageKey, CancellationToken.None);
                }
                catch
                {
                    // El error original conserva la causa del fallo del caso de uso.
                }
            }

            throw;
        }
    }

    private static string BuildStorageKey(
        Guid empresaId,
        Guid vehiculoId,
        string extension)
    {
        return $"empresas/{empresaId:N}/vehiculos/{vehiculoId:N}/{Guid.NewGuid():N}{extension}";
    }

    private static VehiculoImagenDto Map(VehiculoImagen image, string url)
    {
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
    }
}
