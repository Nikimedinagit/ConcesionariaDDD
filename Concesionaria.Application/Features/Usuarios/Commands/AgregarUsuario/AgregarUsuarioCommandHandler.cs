using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Identity;
using Concesionaria.Domain.Usuarios;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Usuarios.Commands.AgregarUsuario;

public class AgregarUsuarioCommandHandler : IRequestHandler<AgregarUsuarioCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AgregarUsuarioCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager
    )
    {
        _context = context;
        _currentUser = currentUser;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<Guid> Handle(
        AgregarUsuarioCommand request,
        CancellationToken cancellationToken
    )
    {
        var email = request.Email.ToLower().Trim();
        var existingIdentityUser = await _userManager.FindByEmailAsync(email);

        if (existingIdentityUser != null)
            throw new Exception("Ya existe un usuario de acceso con ese email.");

        var role = await _roleManager.FindByIdAsync(request.RolId);

        if (role?.Name == null)
            throw new Exception("El rol no es válido.");

        var identityUser = new ApplicationUser(
            _currentUser.EmpresaId,
            email,
            request.NombreCompleto.ToUpper().Trim()
        )
        {
            EmailConfirmed = true,
            LockoutEnabled = true,
            LockoutEnd = null,
        };
        identityUser.AsignarRol(request.RolId);

        var identityResult = await _userManager.CreateAsync(identityUser, request.Password);

        if (!identityResult.Succeeded)
        {
            var mensaje = string.Join(" ", identityResult.Errors.Select(e => e.Description));
            throw new Exception(mensaje);
        }

        var roleResult = await _userManager.AddToRoleAsync(identityUser, role.Name);

        if (!roleResult.Succeeded)
        {
            await _userManager.DeleteAsync(identityUser);
            var mensaje = string.Join(" ", roleResult.Errors.Select(e => e.Description));
            throw new Exception(mensaje);
        }

        var usuario = Usuario.Crear(
            _currentUser.EmpresaId,
            request.RolId,
            request.SucursalId,
            request.NombreCompleto,
            email,
            identityUser.PasswordHash
        );

        try
        {
            await _context.Usuarios.AddAsync(usuario, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            await _userManager.DeleteAsync(identityUser);
            throw;
        }

        return usuario.Id;
    }
}
