using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Interfaces.IRepositories;
using FluentValidation;

namespace Application.Features.Cuentas.Commands.AgregarCuenta;
public class AgregarCuentaCommandValidation : AbstractValidator<AgregarCuentaCommand>
{
    public AgregarCuentaCommandValidation(
        ICuentaRepository repository,
        ICurrentUserService currentUser
    )
    {
        RuleFor(c => c.Codigo)
            .NotEmpty()
            .WithMessage("El código es obligatorio.")
            .MustAsync(
                async (codigo, cancellationToken) =>
                {
                    var empresaId = currentUser.EmpresaId;

                    return !await repository.ExistePorCodigoAsync(codigo, empresaId);
                }
            )
            .WithMessage("Ya existe esa Cuenta.");
            
        RuleFor(c => c.Nombre)
            .NotEmpty()
            .WithMessage("El nombre es obligatorio.")
            .MustAsync(
                async (nombre, cancellationToken) =>
                {
                    var empresaId = currentUser.EmpresaId;

                    return !await repository.ExistePorNombreAsync(nombre, empresaId);
                }
            )
            .WithMessage("Ya existe esa Cuenta.");
    }
}