using FluentValidation;

namespace Application.Features.Vehiculos.Imagenes.ObtenerImagenes;

public sealed class ObtenerVehiculoImagenesQueryValidator
    : AbstractValidator<ObtenerVehiculoImagenesQuery>
{
    public ObtenerVehiculoImagenesQueryValidator()
    {
        RuleFor(query => query.VehiculoId)
            .NotEmpty()
            .WithMessage("El vehículo es obligatorio.");
    }
}
