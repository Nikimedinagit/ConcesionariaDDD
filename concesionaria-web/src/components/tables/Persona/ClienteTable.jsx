import { useMemo } from "react";
import { useNavigate } from "react-router-dom";
import DataTable from "../DataTable";
import { ActionButton } from "@/components/ui/custom/ActionButton";
import { Tooltip } from "@/components/ui/custom/TooltipCustom";

const ClienteTable = ({
  data,
  tipo,
  onToggle,
  onSearch,
  onEdit,
  onToggleStatus,
  localidades = [],
  canEdit = true,
  canActivate = true,
  canDeactivate = true,
}) => {
  const navigate = useNavigate();
  const localidadPorId = useMemo(
    () =>
      new Map(localidades.map((localidad) => [localidad.id, localidad.nombre])),
    [localidades],
  );
  const columns = useMemo(
    () => [
      {
        accessorKey: "nombreCompleto",
        header: "Nombre completo",
        cell: ({ row }) => (
          <button
            type="button"
            onClick={() =>
              navigate(`/layout/clientes/${row.original.clienteId}`, {
                state: {
                  cliente: row.original,
                  localidadNombre: localidadPorId.get(row.original.localidadId),
                  activo: tipo === "activas",
                },
              })
            }
            className="cursor-pointer font-medium text-slate-700 transition-colors hover:text-[hsl(var(--nav-bg))] hover:underline hover:underline-offset-4"
          >
            {row.original.nombreCompleto}
          </button>
        ),
      },
      { accessorKey: "dni", header: "DNI" },
      { accessorKey: "telefono", header: "Teléfono" },
      {
        accessorKey: "email",
        header: "Email",
        cell: ({ row }) => row.original.email?.toLowerCase() || "—",
      },
      {
        accessorKey: "localidadId",
        header: "Localidad",
        cell: ({ row }) => localidadPorId.get(row.original.localidadId) || "—",
      },
      {
        id: "acciones",
        header: "Acciones",
        cell: ({ row }) => (
          <div className="flex justify-end gap-0.5">
            {tipo === "activas" ? (
              <>
                {canEdit && <Tooltip text="Editar">
                  <ActionButton
                    type="edit"
                    onClick={() => onEdit(row.original)}
                  />
                </Tooltip>}
                {canDeactivate && <Tooltip text="Desactivar">
                  <ActionButton
                    type="desactivar"
                    onClick={() => onToggleStatus(row.original)}
                  />
                </Tooltip>}
              </>
            ) : canActivate && (
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
    [tipo, onEdit, onToggleStatus, localidadPorId, navigate, canEdit, canActivate, canDeactivate],
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

export default ClienteTable;
