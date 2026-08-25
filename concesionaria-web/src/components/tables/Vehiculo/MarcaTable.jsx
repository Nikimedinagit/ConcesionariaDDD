import { useMemo } from "react";
import DataTable from "../DataTable";
import { ActionButton } from "@/components/ui/custom/ActionButton";
import { Tooltip } from "@/components/ui/custom/TooltipCustom";

const MarcaTable = ({
  data,
  tipo,
  onToggle,
  onSearch,
  onEdit,
  onToggleStatus,
  canEdit = true,
  canActivate = true,
  canDeactivate = true,
}) => {
  const columns = useMemo(
    () => [
      { accessorKey: "nombre", header: "Nombre" },
      {
        accessorKey: "acciones",
        header: "Acciones",
        cell: ({ row }) => (
          <div className="flex justify-end gap-0.5">
            {tipo === "activas" && (canEdit || canDeactivate) && (
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
            )}

            {tipo === "inactivas" && canActivate && (
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
    [tipo, onEdit, onToggleStatus, canEdit, canActivate, canDeactivate],
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

export default MarcaTable;
