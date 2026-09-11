import { CalendarDays, Mail, MapPin, ShieldCheck, UserRound } from "lucide-react";
import { ActionButton } from "@/components/ui/custom/ActionButton";
import { Tooltip } from "@/components/ui/custom/TooltipCustom";

const formatDate = (value) => {
  if (!value) return "—";
  return new Intl.DateTimeFormat("es-AR", {
    day: "2-digit",
    month: "2-digit",
    year: "numeric",
  }).format(new Date(value));
};

const statusStyles = {
  activo: "bg-emerald-50 text-emerald-700 ring-emerald-200",
  inactivo: "bg-slate-100 text-slate-600 ring-slate-200",
  bloqueado: "bg-rose-50 text-rose-700 ring-rose-200",
};

export function UserCard({
  usuario,
  tipo,
  onEdit,
  onChangePassword,
  onToggleStatus,
}) {
  const estado = usuario.estado?.toLowerCase();

  return (
    <article className="group relative overflow-hidden rounded-2xl border border-slate-200 bg-white p-4 shadow-[0_4px_14px_rgba(15,23,42,0.06)] transition-all duration-200 hover:-translate-y-0.5 hover:border-[hsl(var(--nav-bg)/0.28)] hover:shadow-[0_8px_24px_rgba(15,23,42,0.1)]">
      <div className="flex items-start justify-between gap-3">
        <div className="flex min-w-0 items-center gap-3">
          <div className="flex h-10 w-10 shrink-0 items-center justify-center rounded-xl bg-[hsl(var(--nav-bg)/0.1)] text-[hsl(var(--nav-bg))]">
            <UserRound className="h-5 w-5" />
          </div>
          <div className="min-w-0">
            <h3 className="truncate text-sm font-bold uppercase text-slate-900">{usuario.nombreCompleto}</h3>
            <p className="mt-0.5 truncate text-xs text-slate-500">{usuario.email?.toLowerCase()}</p>
          </div>
        </div>
        <span className={`shrink-0 rounded-full px-2.5 py-1 text-[10px] font-bold uppercase ring-1 ${statusStyles[estado] || statusStyles.inactivo}`}>
          {usuario.estado || "—"}
        </span>
      </div>

      <div className="mt-4 grid grid-cols-2 gap-2 border-t border-slate-100 pt-3">
        <InfoItem icon={ShieldCheck} label="Rol" value={usuario.rolNombre} />
        <InfoItem icon={MapPin} label="Sucursal" value={usuario.sucursalNombre} />
        <InfoItem icon={CalendarDays} label="Alta" value={formatDate(usuario.fechaAlta)} />
        <InfoItem icon={Mail} label="Último acceso" value={formatDate(usuario.ultimoAcceso)} />
      </div>

      <div className="mt-4 flex justify-end gap-1 border-t border-slate-100 pt-3">
        {tipo === "activas" ? (
          <>
            <Tooltip text="Editar"><ActionButton type="edit" onClick={() => onEdit(usuario)} /></Tooltip>
            <Tooltip text="Cambiar contraseña"><ActionButton type="password" onClick={() => onChangePassword(usuario)} /></Tooltip>
            <Tooltip text="Desactivar"><ActionButton type="desactivar" onClick={() => onToggleStatus(usuario)} /></Tooltip>
          </>
        ) : (
          <Tooltip text="Activar"><ActionButton type="activar" onClick={() => onToggleStatus(usuario)} /></Tooltip>
        )}
      </div>
    </article>
  );
}

function InfoItem({ icon: Icon, label, value }) {
  return (
    <div className="min-w-0">
      <div className="flex items-center gap-1.5 text-[10px] font-semibold uppercase tracking-wide text-slate-400">
        <Icon className="h-3.5 w-3.5 text-[hsl(var(--nav-bg))]" />
        {label}
      </div>
      <p className="mt-0.5 truncate text-xs font-semibold text-slate-700">{value || "—"}</p>
    </div>
  );
}
