import { useMemo } from "react";
import DataTable from "./DataTable";
import { ActionButton } from "@/components/ui/custom/ActionButton";
import { Tooltip } from "@/components/ui/custom/TooltipCustom";

const CategoriaGastoTable = ({ data, tipo, onToggle, onSearch, onEdit }) => {
  const columns = useMemo(
    () => [
      { accessorKey: "nombre", header: "Nombre" },
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
                    // onClick={async () => {
                    //   await CategoriaGastoService.desactivar(
                    //     row.original.id
                    //   );
                    // }}
                  />
                </Tooltip>
              </>
            )}

            {tipo === "inactivas" && (
              <Tooltip text="Activar">
                <ActionButton
                  type="activar"
                  // onClick={async () => {
                  //   await CategoriaGastoService.activar(
                  //     row.original.id
                  //   );
                  // }}
                />
              </Tooltip>
            )}
          </div>
        ),
      },
    ],
    [tipo], 
  );

  return (
    <DataTable columns={columns} data={data} tipo={tipo} onToggle={onToggle} onSearch={onSearch} />
  );
};

export default CategoriaGastoTable;
