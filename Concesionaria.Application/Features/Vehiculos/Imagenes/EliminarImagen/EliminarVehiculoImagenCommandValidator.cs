using FluentValidation;

namespace Application.Features.Vehiculos.Imagenes.EliminarImagen;

public sealed class EliminarVehiculoImagenCommandValidator
    : AbstractValidator<EliminarVehiculoImagenCommand>
{
    public EliminarVehiculoImagenCommandValidator()
    {
        RuleFor(command => command.VehiculoId)
            .NotEmpty()
            .WithMessage("El vehículo es obligatorio.");

        RuleFor(command => command.ImagenId)
            .NotEmpty()
            .WithMessage("La imagen es obligatoria.");
    }
}
