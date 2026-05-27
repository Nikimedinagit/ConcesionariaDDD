using Concesionaria.Domain.Identity;

public interface ITokenService
{
    string CreateToken(ApplicationUser user);
}