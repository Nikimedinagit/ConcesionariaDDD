using FluentValidation;

namespace Application.Features.Vehiculos.Imagenes.EstablecerPrincipal;

public sealed class EstablecerImagenPrincipalCommandValidator
    : AbstractValidator<EstablecerImagenPrincipalCommand>
{
    public EstablecerImagenPrincipalCommandValidator()
    {
        RuleFor(command => command.VehiculoId)
            .NotEmpty()
            .WithMessage("El vehículo es obligatorio.");

        RuleFor(command => command.ImagenId)
            .NotEmpty()
            .WithMessage("La imagen es obligatoria.");
    }
}
