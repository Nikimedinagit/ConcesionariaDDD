import { useMemo } from "react";
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
}) => {
  const localidadPorId = useMemo(
    () =>
      new Map(localidades.map((localidad) => [localidad.id, localidad.nombre])),
    [localidades],
  );
  const columns = useMemo(
    () => [
      { accessorKey: "nombreCompleto", header: "Nombre completo" },
      { accessorKey: "dni", header: "DNI" },
      { accessorKey: "telefono", header: "Teléfono" },
      { accessorKey: "email", header: "Email" },
      { accessorKey: "domicilio", header: "Domicilio" },
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
            ) : (
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

export default ClienteTable;
