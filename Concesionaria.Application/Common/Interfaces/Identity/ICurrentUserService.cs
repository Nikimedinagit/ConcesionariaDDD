namespace Concesionaria.Application.Common.Interfaces;

public interface ICurrentUserService
{
    string UserId { get; }
    int SessionVersion { get; }
    Guid SucursalId { get; }
    Guid EmpresaId { get; }
    bool EsAdministrador { get; }
}