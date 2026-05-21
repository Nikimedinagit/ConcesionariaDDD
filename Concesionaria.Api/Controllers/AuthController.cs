using System.ComponentModel.DataAnnotations;
using Concesionaria.Domain.Empresas;
using Concesionaria.Domain.Empresas.Enums;
using Concesionaria.Infrastructure.Persistence;
using Concesionaria.Infrastructure.Persistence.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Concesionaria.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    private readonly TokenService _tokenService;

    public AuthController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, TokenService tokenService)
    {
        _context = context;
        _userManager = userManager;
        _tokenService = tokenService;
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

        var empresa = Empresa.Crear(request.RazonSocial.ToUpper().Trim(), request.Cuit, request.NombreFantasia.ToUpper().Trim(), moneda);
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

        return Ok(new
        {
            email = user.Email,
            token = _tokenService.CreateToken(user)
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
