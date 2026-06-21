import { useMemo } from "react";
import DataTable from "./DataTable";

const LocalidadTable = ({ data, onSearch }) => {
  const columns = useMemo(
    () => [
      { accessorKey: "nombre", header: "Nombre" },
      { accessorKey: "provinciaNombre", header: "Provincia" },
      { accessorKey: "codigoPostal", header: "Código Postal" },
    ],
    [],
  );

  return (
    <DataTable
      columns={columns}
      data={data}
      onSearch={onSearch}
      showStatusFilter={false}
    />
  );
};

export default LocalidadTable;
