/* eslint-disable react-hooks/set-state-in-effect */
import { useEffect, useMemo, useState } from "react";
import { KeyRound, ShieldCheck } from "lucide-react";
import PageHeader from "@/components/ui/custom/PageHeader";
import { AppSelect } from "@/components/ui/custom/AppSelect";
import { Button } from "@/components/ui/button";
import permisosService from "@/services/Acceso/permisosService";
import { toastService } from "@/services/toastService";
import { PermissionSwitch } from "@/components/ui/custom/PermissionSwitch";
import { usePermissions } from "@/context/PermissionContext";

const moduleOrder = [
  "Acceso",
  "Contabilidad",
  "Tesorería",
  "Operaciones",
  "Gestoría",
  "Vehículos",
  "Personas",
  "Ubicaciones",
];

const moduleByView = {
  PERFIL: "Acceso",
  USUARIOS: "Acceso",
  CUENTAS: "Contabilidad",
  CATEGORIAS: "Tesorería",
  CLIENTES: "Personas",
  PROVEEDORES: "Personas",
  MARCAS: "Vehículos",
  TIPOS: "Vehículos",
  MODELOS: "Vehículos",
  SUCURSALES: "Ubicaciones",
  LOCALIDADES: "Ubicaciones",
  PROVINCIAS: "Ubicaciones",
};

const viewLabels = {
  CUENTAS: "Cuentas",
  CATEGORIAS: "Categorías de gastos",
  MARCAS: "Marcas",
  TIPOS: "Tipos de vehículo",
  MODELOS: "Modelos",
};

export function PermisosPage() {
  const [catalogo, setCatalogo] = useState({ permisos: [], roles: [] });
  const [roleId, setRoleId] = useState("");
  const [seleccionados, setSeleccionados] = useState(new Set());
  const [saving, setSaving] = useState(false);
  const { refresh: refreshPermissions } = usePermissions();
  const [selectedModule, setSelectedModule] = useState("Acceso");

  const rol = catalogo.roles.find((item) => item.id === roleId);
  const isAdministrator = rol?.nombre === "ADMINISTRADOR";
  const grupos = useMemo(() => {
    return catalogo.permisos.reduce((acc, permiso) => {
      const modulo = moduleByView[permiso.vista] || "Otros";
      acc[modulo] ??= [];
      acc[modulo].push(permiso);
      return acc;
    }, {});
  }, [catalogo.permisos]);

  useEffect(() => {
    permisosService.obtenerCatalogo().then((data) => {
      setCatalogo(data);
      setRoleId(data.roles[0]?.id || "");
    }).catch(() => toastService.error("Error", { description: "No se pudo cargar la configuración de permisos." }));
  }, []);

  useEffect(() => {
    setSeleccionados(isAdministrator ? new Set(catalogo.permisos.map((item) => item.codigo)) : new Set(rol?.permisos || []));
  }, [rol, isAdministrator, catalogo.permisos]);

  const toggle = (codigo) => {
    setSeleccionados((current) => {
      const next = new Set(current);
      next.has(codigo) ? next.delete(codigo) : next.add(codigo);
      return next;
    });
  };

  const save = async () => {
    setSaving(true);
    try {
      await permisosService.actualizarRol(roleId, [...seleccionados]);
      await refreshPermissions();
      toastService.success("Éxito", { description: "Permisos actualizados correctamente." });
      setCatalogo((current) => ({ ...current, roles: current.roles.map((item) => item.id === roleId ? { ...item, permisos: [...seleccionados] } : item) }));
    } catch {
      toastService.error("Error", { description: "No se pudieron actualizar los permisos." });
    } finally {
      setSaving(false);
    }
  };

  return (
    <div>
      <PageHeader title="Permisos" icon={ShieldCheck} />

      <div className="space-y-4 rounded-xl border border-slate-200 bg-white p-4 shadow-sm sm:p-5">
        <div className="flex flex-col gap-3 sm:flex-row sm:items-end">
          <div className="min-w-0 flex-1">
            <AppSelect label="Rol a configurar" icon={KeyRound} value={roleId} onValueChange={setRoleId} options={catalogo.roles.map((item) => ({ value: item.id, label: item.nombre }))} placeholder="Seleccione un rol" />
          </div>
          <Button onClick={save} disabled={!roleId || saving || isAdministrator} className="h-9 rounded-xl px-6 font-semibold text-white shadow-sm hover:opacity-90" style={{ background: "hsl(var(--nav-bg))" }}>
            {saving ? "Procesando..." : "Guardar"}
          </Button>
        </div>
        <div className="grid gap-4 lg:grid-cols-[190px_minmax(0,1fr)]">
          <nav className="space-y-1 rounded-xl bg-slate-50 p-2">
            {moduleOrder.map((modulo) => grupos[modulo]?.length ? (
              <button key={modulo} type="button" onClick={() => setSelectedModule(modulo)} className={`w-full rounded-lg px-3 py-2.5 text-left text-xs font-bold uppercase tracking-wide transition-colors ${selectedModule === modulo ? "bg-[hsl(var(--nav-bg))] text-white shadow-sm" : "text-slate-600 hover:bg-white hover:text-slate-900"}`}>
                {modulo}
              </button>
            ) : null)}
          </nav>

          <section className="min-w-0 rounded-xl border border-slate-200 p-4">
            <div className="mb-4 border-b border-slate-200 pb-3">
              <p className="text-[10px] font-semibold uppercase tracking-[0.18em] text-[hsl(var(--nav-bg))]">Módulo</p>
              <h2 className="mt-1 text-lg font-bold uppercase tracking-tight text-slate-900">{selectedModule}</h2>
            </div>
            <div className="space-y-4">
              {Object.entries((grupos[selectedModule] || []).reduce((acc, permiso) => {
                const vista = permiso.vista;
                acc[vista] ??= [];
                acc[vista].push(permiso);
                return acc;
              }, {})).map(([vista, permisos]) => (
                <div key={vista} className="border-b border-slate-100 pb-4 last:border-b-0 last:pb-0">
                  <h3 className="mb-2 text-xs font-bold uppercase tracking-wide text-slate-500">{viewLabels[vista] || vista}</h3>
                  <div className="grid gap-2 sm:grid-cols-2">
                    {permisos.map((permiso) => (
                      <PermissionSwitch key={permiso.codigo} label={permiso.nombre} checked={isAdministrator || seleccionados.has(permiso.codigo)} onCheckedChange={() => toggle(permiso.codigo)} disabled={isAdministrator} />
                    ))}
                  </div>
                </div>
              ))}
            </div>
          </section>
        </div>
      </div>
    </div>
  );
}
