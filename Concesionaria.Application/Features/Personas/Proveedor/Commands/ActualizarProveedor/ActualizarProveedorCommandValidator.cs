using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Interfaces.IRepositories;
using FluentValidation;

public class ActualizarProveedorCommandValidator : AbstractValidator<ActualizarProveedorCommand>
{
    public ActualizarProveedorCommandValidator(
        IProveedorRepository repository,
        ICurrentUserService currentUser
    )
    {
        RuleFor(c => c.Nombre).NotEmpty().WithMessage("El nombre es obligatorio.");

        RuleFor(c => c.Domicilio).NotEmpty().WithMessage("El domicilio es obligatorio");

        RuleFor(c => c.LocalidadId).NotEmpty().WithMessage("La localidad es obligatoria");

        RuleFor(c => c.Telefono).NotEmpty().WithMessage("El telefono es obligatorio");

        RuleFor(c => c.Cuil)
            .Cascade(CascadeMode.Stop)
            .Length(11)
            .WithMessage("El CUIL debe tener 11 dígitos")
            .CustomAsync(
                async (cuil, context, ct) =>
                {
                    var empresaId = currentUser.EmpresaId;
                    var estado = await repository.ExistePorCuilExcluyendoIdAsync(cuil.Trim(), empresaId, context.InstanceToValidate.ProveedorId);

                    if (estado == ClienteEstado.Activo)
                        context.AddFailure("Ya existe un proveedor activo con ese CUIL.");

                    if (estado == ClienteEstado.Desactivado)
                        context.AddFailure(
                            "Ya se encuentra un proveedor inactivo con ese CUIL. Puede reactivarlo."
                        );
                }
            );

        RuleFor(c => c.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("El Email es obligatorio")
            .CustomAsync(
                async (email, context, ct) =>
                {
                    var empresaId = currentUser.EmpresaId;
                    var estado = await repository.ExistePorEmailExcluyendoIdAsync(email.Trim(), empresaId, context.InstanceToValidate.ProveedorId);

                    if (estado == ClienteEstado.Activo)
                        context.AddFailure("Ya existe un proveedor activo con ese Email.");

                    if (estado == ClienteEstado.Desactivado)
                        context.AddFailure(
                            "Ya se encuentra un proveedor inactivo con ese Email. Puede reactivarlo."
                        );
                }
            );
    }
}
