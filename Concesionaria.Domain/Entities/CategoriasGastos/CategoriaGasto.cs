using Concesionaria.Domain.Common;
using Concesionaria.Domain.Empresas;

namespace Concesionaria.Domain.CategoriasGastos;

public class CategoriaGasto : BaseEntity<Guid>, ISoftDelete
{
    public string Nombre { get; private set; }
    public bool Eliminado { get; set; } = false;
    public Guid EmpresaId {get; private set;}
    public Empresa Empresa {get; private set;}


    protected CategoriaGasto(){}

    private CategoriaGasto(string nombre, Empresa empresa)
    {
        Id = Guid.NewGuid();
        Nombre = nombre;
        Empresa = empresa;
    }

    public static CategoriaGasto Crear(string nombre, Empresa empresa)
    {
        return new CategoriaGasto(nombre, empresa);
    }

    public void ActualizarCategoriaGasto(string nombre)
    {
        Nombre = nombre.ToUpper().Trim();
    }

    public void Desactivar()
    {
        Eliminado = true;
    }
    
    public void Activar()
    {
        Eliminado = false;
    }

}