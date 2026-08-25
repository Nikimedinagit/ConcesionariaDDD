using System.ComponentModel.DataAnnotations;
using Concesionaria.Application.Auth.Commands.SolicitarCodigo;
using Concesionaria.Application.Auth.Commands.ValidarCodigo;
using Concesionaria.Domain.Empresas;
using Concesionaria.Domain.Empresas.Enums;
using Concesionaria.Domain.Identity;
using Concesionaria.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TuProyecto.Application.Auth.Commands.CambiarContraseña;

namespace Concesionaria.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IMediator _mediator;

    private readonly ITokenService _tokenService;

    public AuthController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IMediator mediator,
        ITokenService tokenService)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _mediator = mediator;
        _tokenService = tokenService;
    }


    [HttpPost("solicitar-codigo")]
    [AllowAnonymous]
    public async Task<IActionResult> SolicitarCodigo([FromBody] SolicitarCodigoCommand command)
    {
        try
        {
            await _mediator.Send(command);
            return Ok(new { message = "Código enviado correctamente." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("validar-codigo")]
    [AllowAnonymous]
    public async Task<IActionResult> ValidarCodigo([FromBody] ValidarCodigoCommand command)
    {
        try
        {
            var resultado = await _mediator.Send(command);
            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("cambiar-password")]
    [AllowAnonymous]
    public async Task<IActionResult> CambiarContraseña([FromBody] CambiarContraseñaCommand command)
    {
        bool result = await _mediator.Send(command);

        if (!result)
        {
            return BadRequest(new { message = "No se pudo cambiar la contraseña. Verificá que los datos sean correctos." });
        }

        return Ok(new { message = "Contraseña actualizada correctamente." });
    }



    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        var existingCuit = await _context.Empresas.AnyAsync(e => e.Cuit == request.Cuit);
        if (existingUser is not null || existingCuit)
        {
            return BadRequest(new { message = "El Email o CUIT ya está en uso." });
        }

        if (!await _context.Localidades.AnyAsync(l => l.Id == request.LocalidadId))
        {
            return BadRequest(new { message = "La localidad seleccionada no es válida." });
        }

        var moneda = request.Moneda.ToUpper() switch
        {
            "ARS" => Moneda.ARG,
            "ARG" => Moneda.ARG,
            "USD" => Moneda.USD,
            "BRL" => Moneda.BRL,
            _ => throw new InvalidOperationException("Moneda inválida."),
        };

        var localidad = await _context.Localidades.FindAsync(request.LocalidadId);
        if (localidad is null)
        {
            return BadRequest(new { message = "La localidad seleccionada no es válida." });
        }

        var empresa = Empresa.Crear(request.RazonSocial.ToUpper().Trim(), request.Cuit, request.NombreFantasia.ToUpper().Trim(), moneda, localidad);
        _context.Empresas.Add(empresa);

        var user = new ApplicationUser(empresa.Id, request.Email.ToLower().Trim(), request.NombreCompleto.ToUpper().Trim())
        {
            EmailConfirmed = true,
            LockoutEnabled = true,
            LockoutEnd = DateTimeOffset.MaxValue,
        };

        var identityResult = await _userManager.CreateAsync(user, request.Password);
        if (!identityResult.Succeeded)
        {
            return BadRequest(new { errors = identityResult.Errors.Select(e => e.Description) });
        }

        var administrador = await _roleManager.FindByNameAsync("ADMINISTRADOR");
        if (administrador is not null)
        {
            user.AsignarRol(administrador.Id);
            await _userManager.AddToRoleAsync(user, administrador.Name!);
        }

        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Registro completado. Tu cuenta está deshabilitada hasta que un administrador la active."
        });
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _userManager.FindByEmailAsync(request.Email.ToLower().Trim());
        if (user is null)
        {
            return Unauthorized(new { message = "Email o contraseña incorrectos." });
        }

        if (user.LockoutEnd.HasValue && user.LockoutEnd > DateTimeOffset.UtcNow)
        {
            return BadRequest(new { message = "Tu cuenta todavía está deshabilitada. Espera la activación." });
        }

        var validPassword = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!validPassword)
        {
            return Unauthorized(new { message = "Email o contraseña incorrectos." });
        }

        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u =>
                u.Email == user.Email &&
                u.EmpresaId == user.EmpresaId);

        if (usuario is not null)
        {
            usuario.RegistrarAcceso();
            await _context.SaveChangesAsync();
        }

        return Ok(new
        {
            email = user.Email,
            token = await _tokenService.CreateToken(user)
        });
    }
}

public class RegisterRequest
{
    [Required]
    public string RazonSocial { get; set; } = string.Empty;

    [Required]
    public string NombreFantasia { get; set; } = string.Empty;

    [Required]
    public string Cuit { get; set; } = string.Empty;

    [Required]
    public Guid LocalidadId { get; set; }

    [Required]
    public string Moneda { get; set; } = string.Empty;

    [Required]
    public string NombreCompleto { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;
}

public class LoginRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;
}
