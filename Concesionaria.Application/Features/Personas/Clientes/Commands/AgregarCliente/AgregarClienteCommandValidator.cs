using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Common.Enums;
using Concesionaria.Domain.Interfaces.IRepositories;
using FluentValidation;

public class AgregarClienteCommandValidator : AbstractValidator<AgregarClienteCommand>
{
    public AgregarClienteCommandValidator(
        IClienteRepository repository,
        ICurrentUserService currentUser
    )
    {
        RuleFor(c => c.NombreCompleto).NotEmpty().WithMessage("El nombre es obligatorio.");

        RuleFor(c => c.Domicilio).NotEmpty().WithMessage("El domicilio es obligatorio");

        RuleFor(c => c.LocalidadId).NotEmpty().WithMessage("La localidad es obligatoria");

        RuleFor(c => c.Telefono).NotEmpty().WithMessage("El telefono es obligatorio");

        RuleFor(c => c.Dni)
            .Length(8)
            .WithMessage("El DNI debe tener 8 dígitos")
            .CustomAsync(
                async (dni, context, ct) =>
                {
                    var empresaId = currentUser.EmpresaId;
                    var estado = await repository.ExistePorDniAsync(dni.Trim(), empresaId);

                    if (estado == EstadoExistencia.Activo)
                        context.AddFailure(
                            "Ya existe un cliente activo con ese DNI."
                        );

                    if (estado == EstadoExistencia.Desactivado)
                        context.AddFailure(
                            "Ya se encuentra un cliente inactivo con ese DNI. Puede reactivarlo."
                        );
                }
            );

        RuleFor(c => c.Email)
            .NotEmpty()
            .WithMessage("El Email es obligatorio")
                  .CustomAsync(
                async (email, context, ct) =>
                {
                    var empresaId = currentUser.EmpresaId;
                    var estado = await repository.ExistePorEmailAsync(email.Trim(), empresaId);

                    if (estado == EstadoExistencia.Activo)
                        context.AddFailure(
                            "Ya existe un cliente activo con ese Email."
                        );

                    if (estado == EstadoExistencia.Desactivado)
                        context.AddFailure(
                            "Ya se encuentra un cliente inactivo con ese Email. Puede reactivarlo."
                        );
                }
            );
    }
}
