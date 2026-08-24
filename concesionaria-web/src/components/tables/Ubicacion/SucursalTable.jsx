import { useMemo } from "react";
import DataTable from "../DataTable";
import { ActionButton } from "@/components/ui/custom/ActionButton";
import { Tooltip } from "@/components/ui/custom/TooltipCustom";

const SucursalTable = ({
  data,
  tipo,
  onToggle,
  onSearch,
  onEdit,
  onToggleStatus,
  localidades = [],
}) => {
  const localidadPorId = useMemo(
    () =>
      localidades.reduce((acc, localidad) => {
        acc[localidad.id] = localidad.nombre;
        return acc;
      }, {}),
    [localidades],
  );

  const columns = useMemo(
    () => [
      { accessorKey: "nombre", header: "Nombre" },
      { accessorKey: "direccion", header: "Dirección" },
      {
        accessorKey: "localidadId",
        header: "Localidad",
        cell: ({ row }) =>
          row.original.localidadNombre ||
          row.original.localidad ||
          localidadPorId[row.original.localidadId] ||
          row.original.localidadId,
      },
      {
        accessorKey: "acciones",
        header: "Acciones",
        cell: ({ row }) => (
          <div className="flex justify-end gap-0.5">
            {tipo === "activas" && (
              <>
                <Tooltip text="Editar">
                  <ActionButton
                    type="edit"
                    onClick={() => onEdit(row.original)}
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
    [tipo, onEdit, onToggleStatus, localidadPorId],
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

export default SucursalTable;
