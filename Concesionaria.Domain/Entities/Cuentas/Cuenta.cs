
using Concesionaria.Domain.Common;
using Concesionaria.Domain.Cuentas.Enums;
using Concesionaria.Domain.Empresas;

namespace Concesionaria.Domain.Cuentas;

public class Cuenta : BaseEntity<Guid>, ISoftDelete
{
    public Guid EmpresaId { get; private set; }
    public Empresa Empresa { get; private set; }
    public string Codigo { get; private set; }
    public string Nombre { get; private set; }
    public TipoCuenta Tipo { get; private set; }
    public int Nivel { get; private set; }
    public bool Eliminado { get; set; } = false;

    protected Cuenta() { }

    private Cuenta(Guid empresaId, string codigo, string nombre, TipoCuenta tipo, int nivel)
    {
        Id = Guid.NewGuid();
        EmpresaId = empresaId;
        Codigo = codigo;
        Nombre = nombre;
        Tipo = tipo;
        Nivel = nivel;
    }

    public static Cuenta Crear(Guid empresaId, string codigo, string nombre, TipoCuenta tipo, int nivel)
    {
        return new Cuenta(empresaId, codigo, nombre, tipo, nivel);
    }

    public void ActualizarNombre(string nombre)
    {
        Nombre = nombre.ToUpper().Trim();
    }

    public void ActualizarTipo(TipoCuenta tipo)
    {
        Tipo = tipo;
    }

    public void ActualizarNivel(int nivel)
    {
        Nivel = nivel;
    }

    public void ActualizarCodigo(string codigo)
    {
        Codigo = codigo.ToUpper().Trim();
    }

    public void Activar()
    {
        Eliminado = false;
    }

    public void Desactivar()
    {
        Eliminado = true;
    }
}