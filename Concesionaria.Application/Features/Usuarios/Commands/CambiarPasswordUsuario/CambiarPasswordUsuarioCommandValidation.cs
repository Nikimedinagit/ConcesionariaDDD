using FluentValidation;

namespace Application.Features.Usuarios.Commands.CambiarPasswordUsuario;

public class CambiarPasswordUsuarioCommandValidator
    : AbstractValidator<CambiarPasswordUsuarioCommand>
{
    public CambiarPasswordUsuarioCommandValidator()
    {
        RuleFor(u => u.UsuarioId)
            .NotEqual(Guid.Empty)
            .WithMessage("El usuario es obligatorio.");

        RuleFor(u => u.Password)
            .NotEmpty()
            .WithMessage("La contraseña es obligatoria.")
            .MinimumLength(6)
            .WithMessage("La contraseña debe tener al menos 6 caracteres.");
    }
}
