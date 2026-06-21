import { useMemo } from "react";
import DataTable from "./DataTable";

const ProvinciaTable = ({ data, onSearch }) => {
  const columns = useMemo(
    () => [{ accessorKey: "nombre", header: "Nombre" }],
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

export default ProvinciaTable;
