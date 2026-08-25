using Concesionaria.Domain.Identity;

public interface ITokenService
{
    Task<string> CreateToken(ApplicationUser user);
}
