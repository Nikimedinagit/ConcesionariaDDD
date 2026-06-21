import { useMemo } from "react";
import DataTable from "./DataTable";
import { ActionButton } from "@/components/ui/custom/ActionButton";
import { Tooltip } from "@/components/ui/custom/TooltipCustom";

const formatDate = (value) => {
  if (!value) return "-";

  return new Intl.DateTimeFormat("es-AR", {
    day: "2-digit",
    month: "2-digit",
    year: "numeric",
  }).format(new Date(value));
};

const EstadoBadge = ({ estado }) => {
  const normalized = estado?.toLowerCase();
  const styles = {
    activo: "bg-emerald-50 text-emerald-700 ring-emerald-200",
    inactivo: "bg-slate-100 text-slate-700 ring-slate-200",
    bloqueado: "bg-red-50 text-red-700 ring-red-200",
  };

  return (
    <span
      className={`inline-flex h-6 items-center rounded-full px-2.5 text-xs font-bold ring-1 ${
        styles[normalized] || "bg-slate-100 text-slate-700 ring-slate-200"
      }`}
    >
      {estado}
    </span>
  );
};

const UsuarioTable = ({
  data,
  tipo,
  onToggle,
  onSearch,
  onEdit,
  onChangePassword,
  onToggleStatus,
}) => {
  const columns = useMemo(
    () => [
      { accessorKey: "nombreCompleto", header: "Nombre" },
      { accessorKey: "email", header: "Email" },
      { accessorKey: "rolNombre", header: "Rol" },
      { accessorKey: "sucursalNombre", header: "Sucursal" },
      {
        accessorKey: "fechaAlta",
        header: "Fecha Alta",
        cell: ({ row }) => formatDate(row.original.fechaAlta),
      },
      {
        accessorKey: "ultimoAcceso",
        header: "Último Acceso",
        cell: ({ row }) => formatDate(row.original.ultimoAcceso),
      },
      {
        accessorKey: "estado",
        header: "Estado",
        cell: ({ row }) => <EstadoBadge estado={row.original.estado} />,
      },
      {
        accessorKey: "acciones",
        header: "Acciones",
        cell: ({ row }) => (
          <div className="flex justify-end gap-0.5">
            {tipo === "activas" && (
              <>
                <Tooltip text="Editar">
                  <ActionButton type="edit" onClick={() => onEdit(row.original)} />
                </Tooltip>

                <Tooltip text="Cambiar contraseña">
                  <ActionButton
                    type="password"
                    onClick={() => onChangePassword(row.original)}
                  />
                </Tooltip>

                <Tooltip text="Desactivar">
                  <ActionButton
                    type="desactivar"
                    onClick={() => onToggleStatus(row.original)}
                  />
                </Tooltip>
              </>
            )}

            {tipo === "inactivas" && (
              <Tooltip text="Activar">
                <ActionButton
                  type="activar"
                  onClick={() => onToggleStatus(row.original)}
                />
              </Tooltip>
            )}
          </div>
        ),
      },
    ],
    [tipo, onEdit, onChangePassword, onToggleStatus],
  );

  return (
    <DataTable
      columns={columns}
      data={data}
      tipo={tipo}
      onToggle={onToggle}
      onSearch={onSearch}
    />
  );
};

export default UsuarioTable;
