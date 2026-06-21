using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Interfaces.IRepositories;
using FluentValidation;

namespace Application.Features.Usuarios.Commands.AgregarUsuario;

public class AgregarUsuarioCommandValidator : AbstractValidator<AgregarUsuarioCommand>
{
    public AgregarUsuarioCommandValidator(
        IUsuarioRepository repository,
        ICurrentUserService currentUser
    )
    {
        RuleFor(u => u.NombreCompleto)
            .NotEmpty()
            .WithMessage("El nombre completo es obligatorio.")
            .MaximumLength(100)
            .WithMessage("Máximo 100 caracteres.");

        RuleFor(u => u.Email)
            .NotEmpty()
            .WithMessage("El email es obligatorio.")
            .EmailAddress()
            .WithMessage("El email no es válido.")
            .MustAsync(async (email, cancellationToken) =>
                !await repository.ExisteEmailAsync(email, currentUser.EmpresaId)
            )
            .WithMessage("Ya existe un usuario con ese email.");

        RuleFor(u => u.Password)
            .NotEmpty()
            .WithMessage("La contraseña es obligatoria.")
            .MinimumLength(6)
            .WithMessage("La contraseña debe tener al menos 6 caracteres.");

        RuleFor(u => u.RolId)
            .NotEmpty()
            .WithMessage("El rol es obligatorio.")
            .MustAsync(async (rolId, cancellationToken) =>
                await repository.RolExisteAsync(rolId)
            )
            .WithMessage("El rol no es válido.");

        RuleFor(u => u.SucursalId)
            .NotEqual(Guid.Empty)
            .WithMessage("La sucursal es obligatoria.")
            .MustAsync(async (sucursalId, cancellationToken) =>
                await repository.SucursalExisteAsync(sucursalId, currentUser.EmpresaId)
            )
            .WithMessage("La sucursal no es válida.");
    }
}
