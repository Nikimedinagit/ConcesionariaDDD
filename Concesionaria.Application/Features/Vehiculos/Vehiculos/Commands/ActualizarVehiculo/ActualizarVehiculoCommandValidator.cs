namespace Application.Features.Vehiculos.Commands.ActualizarVehiculo;

using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Interfaces.IRepositories;
using FluentValidation;

public class ActualizarVehiculoCommandValidator : AbstractValidator<ActualizarVehiculoCommand>
{
    public ActualizarVehiculoCommandValidator(
        IVehiculoRepository repository,
        ICurrentUserService currentUser
    )
    {
        RuleFor(v => v.Version)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("La versión es obligatoria.")
            .MaximumLength(50)
            .WithMessage("La versión no puede superar los 50 caracteres.");

        RuleFor(v => v.Anio)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("El año del vehículo es obligatorio.")
            .InclusiveBetween(1900, DateTime.Now.Year)
            .WithMessage($"El año del vehículo debe estar entre 1900 y {DateTime.Now.Year}.");

        RuleFor(v => v.Color).NotEmpty().WithMessage("El color del vehículo es obligatorio.");

        RuleFor(v => v.Patente)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("La patente del vehículo es obligatoria.")
            .Matches(@"^([A-Za-z]{3}\d{3}|[A-Za-z]{2}\d{3}[A-Za-z]{2})$")
            .WithMessage(
                "La patente ingresada no tiene un formato válido (ejemplo: ABC123 o AB123CD)."
            )
            .MustAsync(
                async (Vehiculo, patente, cancellationToken) =>
                {
                    var empresaId = currentUser.EmpresaId;

                    var vehiculoExistente = await repository.ExistePorPatenteExluyendoIdAsync(
                        patente,
                        empresaId,
                        Vehiculo.VehiculoId                    
                    );
                    return !vehiculoExistente;
                }
            )
            .WithMessage("La patente del vehículo ya está en uso.");

        RuleFor(v => v.Kilometraje)
            .GreaterThan(0)
            .WithMessage("El kilometraje debe ser mayor a cero.")
            .When(v => v.Condicion == CondicionVehiculo.Usado);

        RuleFor(v => v.Kilometraje)
            .Equal(0)
            .WithMessage("El kilometraje de un vehículo nuevo debe ser 0.")
            .When(v => v.Condicion == CondicionVehiculo.Nuevo);

        RuleFor(v => v.ModeloId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("El modelo del vehículo es obligatorio.")
            .MustAsync(
                async (modeloId, cancellationToken) =>
                {
                    var empresaId = currentUser.EmpresaId;
                    var modelo = await repository.ObtenerPorModeloIdAsync(modeloId, empresaId);
                    return modelo;
                }
            )
            .WithMessage("El modelo del vehículo no existe.");

        RuleFor(v => v.Condicion)
            .NotEmpty()
            .WithMessage("La condición del vehículo es obligatoria.");

        RuleFor(v => v.Estado).NotEmpty().WithMessage("El estado del vehículo es obligatorio.");

        RuleFor(v => v.PrecioCompra)
            .GreaterThan(0)
            .WithMessage("El precio de compra debe ser mayor a cero.");

        RuleFor(v => v.PrecioVenta)
            .GreaterThan(0)
            .WithMessage("El precio de venta debe ser mayor a cero.");
    }
}
