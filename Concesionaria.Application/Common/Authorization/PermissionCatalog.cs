namespace Concesionaria.Application.Common.Authorization;

public static class PermissionCatalog
{
    public const string View = "permiso.vista";
    public const string Action = "permiso.accion";

    public static readonly IReadOnlyDictionary<string, string> All =
        new Dictionary<string, string>
        {
            ["PERFIL_VER"] = "Ver perfil",
            ["USUARIOS_VER"] = "Ver usuarios",
            ["USUARIOS_CREAR"] = "Crear usuarios",
            ["USUARIOS_EDITAR"] = "Editar usuarios",
            ["USUARIOS_DESACTIVAR"] = "Activar o desactivar usuarios",
            ["USUARIOS_PASSWORD"] = "Cambiar contraseñas",
            ["CLIENTES_VER"] = "Ver clientes",
            ["CLIENTES_CREAR"] = "Crear clientes",
            ["CLIENTES_EDITAR"] = "Editar clientes",
            ["CLIENTES_DESACTIVAR"] = "Desactivar clientes",
            ["CLIENTES_ACTIVAR"] = "Activar clientes",
            ["PROVEEDORES_VER"] = "Ver proveedores",
            ["PROVEEDORES_CREAR"] = "Crear proveedores",
            ["PROVEEDORES_EDITAR"] = "Editar proveedores",
            ["PROVEEDORES_DESACTIVAR"] = "Desactivar proveedores",
            ["PROVEEDORES_ACTIVAR"] = "Activar proveedores",
            ["SUCURSALES_VER"] = "Ver sucursales",
            ["SUCURSALES_CREAR"] = "Crear sucursales",
            ["SUCURSALES_EDITAR"] = "Editar sucursales",
            ["SUCURSALES_ACTIVAR"] = "Activar sucursales",
            ["SUCURSALES_DESACTIVAR"] = "Desactivar sucursales",
            ["LOCALIDADES_VER"] = "Ver localidades",
            ["PROVINCIAS_VER"] = "Ver provincias",
            ["MARCAS_VER"] = "Ver marcas",
            ["MARCAS_CREAR"] = "Crear marcas",
            ["MARCAS_EDITAR"] = "Editar marcas",
            ["MARCAS_ACTIVAR"] = "Activar marcas",
            ["MARCAS_DESACTIVAR"] = "Desactivar marcas",
            ["TIPOS_VEHICULO_VER"] = "Ver tipos de vehículo",
            ["TIPOS_VEHICULO_CREAR"] = "Crear tipos de vehículo",
            ["TIPOS_VEHICULO_EDITAR"] = "Editar tipos de vehículo",
            ["TIPOS_VEHICULO_ACTIVAR"] = "Activar tipos de vehículo",
            ["TIPOS_VEHICULO_DESACTIVAR"] = "Desactivar tipos de vehículo",
            ["MODELOS_VER"] = "Ver modelos",
            ["MODELOS_CREAR"] = "Crear modelos",
            ["MODELOS_EDITAR"] = "Editar modelos",
            ["MODELOS_ACTIVAR"] = "Activar modelos",
            ["MODELOS_DESACTIVAR"] = "Desactivar modelos",
            ["CATEGORIAS_GASTOS_VER"] = "Ver categorías de gastos",
            ["CATEGORIAS_GASTOS_CREAR"] = "Crear categorías de gastos",
            ["CATEGORIAS_GASTOS_EDITAR"] = "Editar categorías de gastos",
            ["CATEGORIAS_GASTOS_ACTIVAR"] = "Activar categorías de gastos",
            ["CATEGORIAS_GASTOS_DESACTIVAR"] = "Desactivar categorías de gastos",
            ["CUENTAS_VER"] = "Ver cuentas",
            ["CUENTAS_CREAR"] = "Crear cuentas",
            ["CUENTAS_EDITAR"] = "Editar cuentas",
            ["CUENTAS_ACTIVAR"] = "Activar cuentas",
            ["CUENTAS_DESACTIVAR"] = "Desactivar cuentas",
        };
}
