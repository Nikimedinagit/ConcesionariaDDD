import { useMemo } from "react";
import {
  AtSign,
  ContactRound,
  MapPin,
  MapPinned,
  Phone,
  Wrench,
} from "lucide-react";
import { useLocation, useNavigate } from "react-router-dom";
import PageHeader from "@/components/ui/custom/PageHeader";
import { BackButton } from "@/components/ui/custom/BackButton";

export function ProveedorDetallePage() {
  const navigate = useNavigate();
  const { state } = useLocation();
  const proveedor = state?.proveedor;

  const localidad = useMemo(
    () => proveedor?.localidad?.nombre || state?.localidadNombre || "—",
    [proveedor, state?.localidadNombre],
  );

  if (!proveedor) {
    return (
      <div className="space-y-4">
        <PageHeader title="Proveedor no encontrado" icon={ContactRound} />
        <div className="rounded-xl border border-slate-200 bg-white p-6 text-slate-600 shadow-sm">
          No se encontró la información del proveedor. Volvé al listado para
          seleccionarlo nuevamente.
          <BackButton
            className="mt-4"
            onClick={() => navigate("/layout/proveedores")}
          >
            Volver a proveedores
          </BackButton>
        </div>
      </div>
    );
  }

  const fields = [
    ["CUIL", proveedor.cuil, ContactRound],
    ["Teléfono", proveedor.telefono, Phone],
    ["Email", proveedor.email?.toLowerCase(), AtSign],
    ["Domicilio", proveedor.domicilio, MapPinned],
    ["Localidad", localidad, MapPin],
    ["Servicio", proveedor.servicio, Wrench],
  ];

  return (
    <div>
      <PageHeader title="Detalle del proveedor" icon={ContactRound}>
        <BackButton onClick={() => navigate("/layout/proveedores")} />
      </PageHeader>

      <div className="rounded-xl border border-slate-200 bg-white p-4 shadow-[0_3px_12px_rgba(15,23,42,0.06)] sm:p-5">
        <div className="mb-4 flex items-center justify-between border-b border-slate-100 pb-3">
          <div>
            <p className="text-[10px] font-semibold uppercase tracking-wide text-slate-400">
              Proveedor
            </p>
            <h2 className="mt-0.5 text-base font-bold uppercase tracking-wide text-slate-800">
              {proveedor.nombre}
            </h2>
          </div>
          <span
            className={
              state?.activo === false
                ? "rounded-full bg-slate-100 px-2.5 py-1 text-[10px] font-bold uppercase text-slate-600 ring-1 ring-slate-200"
                : "rounded-full bg-emerald-50 px-2.5 py-1 text-[10px] font-bold uppercase text-emerald-700 ring-1 ring-emerald-200"
            }
          >
            {state?.activo === false ? "Inactivo" : "Activo"}
          </span>
        </div>

        <div className="grid gap-x-6 gap-y-1 sm:grid-cols-2 lg:grid-cols-3">
          {fields.map(([label, value, Icon]) => (
            <div
              key={label}
              className="flex min-h-14 items-center gap-3 border-b border-slate-100 py-2.5"
            >
              <div className="flex h-8 w-8 shrink-0 items-center justify-center rounded-lg bg-[hsl(var(--nav-bg)/0.08)] text-[hsl(var(--nav-bg))]">
                <Icon className="h-4 w-4 text-[hsl(var(--nav-bg))]" />
              </div>
              <div className="min-w-0">
                <p className="text-[10px] font-semibold uppercase tracking-wide text-slate-400">
                  {label}
                </p>
                <p className="break-words text-sm font-semibold text-slate-800">
                  {value || "—"}
                </p>
              </div>
            </div>
          ))}
        </div>

        <div className="mt-4 rounded-lg bg-slate-50 px-3.5 py-3">
          <p className="text-[10px] font-semibold uppercase tracking-wide text-slate-400">
            Observaciones
          </p>
          <p className="mt-1 whitespace-pre-wrap text-sm leading-6 text-slate-600">
            {proveedor.observacion || "Sin observaciones."}
          </p>
        </div>
      </div>
    </div>
  );
}
