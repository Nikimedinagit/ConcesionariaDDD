using FluentValidation;

namespace Application.Features.Vehiculos.Imagenes.AgregarImagen;

public sealed class AgregarVehiculoImagenCommandValidator
    : AbstractValidator<AgregarVehiculoImagenCommand>
{
    public AgregarVehiculoImagenCommandValidator()
    {
        RuleFor(command => command.VehiculoId)
            .NotEmpty()
            .WithMessage("El vehículo es obligatorio.");

        RuleFor(command => command.Content)
            .NotNull()
            .WithMessage("La imagen es obligatoria.");

        RuleFor(command => command.NombreOriginal)
            .NotEmpty()
            .WithMessage("El nombre original de la imagen es obligatorio.")
            .MaximumLength(255)
            .WithMessage("El nombre de la imagen no puede superar los 255 caracteres.");

        RuleFor(command => command.ContentType)
            .NotEmpty()
            .WithMessage("El tipo de contenido de la imagen es obligatorio.");
    }
}
