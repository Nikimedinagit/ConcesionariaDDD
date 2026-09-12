import {
  CalendarDays,
  Building2,
  CarFront,
  CircleDollarSign,
  Gauge,
  Palette,
  Tag,
} from "lucide-react";
import { ActionButton } from "@/components/ui/custom/ActionButton";
import { Tooltip } from "@/components/ui/custom/TooltipCustom";
import { useAuth } from "@/context/AuthContext";

const estados = {
  1: "DISPONIBLE",
  2: "VENDIDO",
  3: "RESERVADO",
  4: "EN SERVICIO",
};

const condiciones = {
  1: "NUEVO",
  2: "USADO",
  3: "CONSIGNACIÓN",
};

const estadoStyles = {
  1: "bg-emerald-50 text-emerald-700 ring-emerald-200",
  2: "bg-slate-100 text-slate-600 ring-slate-200",
  3: "bg-amber-50 text-amber-700 ring-amber-200",
  4: "bg-rose-50 text-rose-700 ring-rose-200",
};

const formatNumber = (value) => {
  if (value === null || value === undefined) {
    return "—";
  }

  return new Intl.NumberFormat("es-AR", {
    maximumFractionDigits: 2,
  }).format(value);
};

const formatMoney = (value) => {
  if (value === null || value === undefined) return "—";

  return new Intl.NumberFormat("en-US", {
    minimumFractionDigits: 2,
    maximumFractionDigits: 2,
  }).format(value);
};

export function VehiculoCard({ vehiculo, onEdit, onDelete }) {
  const { user } = useAuth();
  const isDisponible = Number(vehiculo.estado) === 1;
  const isSucursalActual =
    String(vehiculo.sucursalId).toLowerCase() ===
    String(user?.sucursalId).toLowerCase();
  const estadoNombre =
    estados[vehiculo.estado] || vehiculo.estado || "SIN ESTADO";

  const condicionNombre =
    condiciones[vehiculo.condicion] || vehiculo.condicion || "—";

  return (
    <article className="group relative overflow-hidden rounded-2xl border border-slate-200 bg-white p-3 shadow-[0_4px_14px_rgba(15,23,42,0.06)] transition-all duration-200 hover:-translate-y-0.5 hover:border-[hsl(var(--nav-bg)/0.28)] hover:shadow-[0_8px_24px_rgba(15,23,42,0.1)]">
      {/* Encabezado */}

      <div className="flex items-start justify-between gap-3">
        <div className="flex min-w-0 items-center gap-3">
          <div className="flex h-10 w-10 shrink-0 items-center justify-center rounded-xl bg-[hsl(var(--nav-bg)/0.1)] text-[hsl(var(--nav-bg))]">
            <CarFront className="h-5 w-5" />
          </div>

          <div className="min-w-0">
            <h3 className="truncate text-sm font-bold uppercase text-slate-900">
              {[vehiculo.marcaNombre, vehiculo.modeloNombre]
                .filter(Boolean)
                .join(" ") || "VEHÍCULO"}
            </h3>

            <p className="mt-0.5 truncate text-xs font-semibold uppercase text-slate-600">
              {vehiculo.version || "SIN VERSIÓN"}
            </p>

            <div className="mt-1 flex items-center gap-1.5 text-xs text-slate-500">
              <Tag className="h-3.5 w-3.5" />

              <span className="font-semibold uppercase">
                {vehiculo.patente || "SIN PATENTE"}
              </span>
            </div>

            <div className="mt-1 flex items-center gap-1.5 text-xs text-slate-500">
              <Building2 className="h-3.5 w-3.5" />
              <span className="truncate font-semibold uppercase">
                {vehiculo.sucursalNombre || "SUCURSAL SIN NOMBRE"}
              </span>
            </div>
          </div>
        </div>

        <div className="flex shrink-0 items-center gap-1.5">
          <span
            className={`rounded-full px-2.5 py-1 text-[10px] font-bold uppercase ring-1 ${
              estadoStyles[vehiculo.estado] ||
              "bg-slate-100 text-slate-600 ring-slate-200"
            }`}
          >
            {estadoNombre}
          </span>

          {isDisponible && isSucursalActual && (
            <>
              <Tooltip text="Editar">
                <ActionButton type="edit" onClick={() => onEdit(vehiculo)} />
              </Tooltip>

              <Tooltip text="Eliminar">
                <ActionButton
                  type="desactivar"
                  onClick={() => onDelete(vehiculo)}
                />
              </Tooltip>
            </>
          )}
        </div>
      </div>

      <div className="mt-3 grid grid-cols-3 gap-x-3 gap-y-3 border-t border-slate-100 pt-3">
        <InfoItem icon={CalendarDays} label="Año" value={vehiculo.anio} />
        <InfoItem icon={CarFront} label="Condición" value={condicionNombre} />
        <InfoItem
          icon={Gauge}
          label="Kilometraje"
          value={`${formatNumber(vehiculo.kilometraje)} KM`}
        />

        <InfoItem icon={Palette} label="Color" value={vehiculo.color} />

        <div className="col-span-2">
          <InfoItem
            icon={CircleDollarSign}
            label="Precio de venta"
            value={`$ ${formatMoney(vehiculo.precioVenta)}`}
            highlighted
          />
        </div>
      </div>
    </article>
  );
}

function InfoItem({ icon: Icon, label, value, highlighted = false }) {
  return (
    <div className="min-w-0">
      <div className="flex items-center gap-1.5 text-[10px] font-semibold uppercase tracking-wide text-slate-400">
        <Icon className="h-3.5 w-3.5 text-[hsl(var(--nav-bg))]" />

        {label}
      </div>

      <p
        className={`mt-0.5 truncate text-xs font-semibold uppercase ${
          highlighted ? "text-emerald-700" : "text-slate-700"
        }`}
      >
        {value ?? "—"}
      </p>
    </div>
  );
}
