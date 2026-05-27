import { useMemo } from "react";
import DataTable from "./DataTable";
import { ActionButton } from "@/components/ui/custom/ActionButton";

const CategoriaGastoTable = ({ data, tipo, onToggle }) => {
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
                <ActionButton
                  type="edit"
                  // onClick={() =>
                  //   console.log("Editar", row.original.id)
                  // }
                />

                <ActionButton
                  type="desactivar"
                  // onClick={async () => {
                  //   await CategoriaGastoService.desactivar(
                  //     row.original.id
                  //   );
                  // }}
                />
              </>
            )}

            {tipo === "inactivas" && (
              <ActionButton
                type="activar"
                // onClick={async () => {
                //   await CategoriaGastoService.activar(
                //     row.original.id
                //   );
                // }}
              />
            )}
          </div>
        ),
      },
    ],
    [tipo], // Dependencia importante: se vuelve a renderizar si cambia el tipo
  );

  return (
    <DataTable columns={columns} data={data} tipo={tipo} onToggle={onToggle} />
  );
};

export default CategoriaGastoTable;
