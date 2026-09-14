using FluentValidation;

namespace Application.Features.Vehiculos.Imagenes.ReordenarImagenes;

public sealed class ReordenarVehiculoImagenesCommandValidator
    : AbstractValidator<ReordenarVehiculoImagenesCommand>
{
    public ReordenarVehiculoImagenesCommandValidator()
    {
        RuleFor(command => command.VehiculoId)
            .NotEmpty()
            .WithMessage("El vehículo es obligatorio.");

        RuleFor(command => command.Imagenes)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage("Debe indicar las imágenes que desea ordenar.")
            .NotEmpty()
            .WithMessage("Debe indicar las imágenes que desea ordenar.")
            .Must(images => images.Count <= 10)
            .WithMessage("No se pueden ordenar más de 10 imágenes.")
            .Must(images => images.Select(image => image.ImagenId).Distinct().Count() == images.Count)
            .WithMessage("No puede repetir una imagen.")
            .Must(images => images.Select(image => image.Orden).Distinct().Count() == images.Count)
            .WithMessage("No puede repetir una posición.")
            .Must(images => images
                .Select(image => image.Orden)
                .OrderBy(order => order)
                .SequenceEqual(Enumerable.Range(0, images.Count)))
            .WithMessage("Las posiciones deben ser consecutivas y comenzar en cero.");

        RuleForEach(command => command.Imagenes).ChildRules(image =>
        {
            image.RuleFor(item => item.ImagenId)
                .NotEmpty()
                .WithMessage("La imagen es obligatoria.");

            image.RuleFor(item => item.Orden)
                .GreaterThanOrEqualTo(0)
                .WithMessage("El orden no puede ser negativo.");
        });
    }
}
