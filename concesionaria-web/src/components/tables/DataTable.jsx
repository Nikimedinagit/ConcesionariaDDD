import {
  flexRender,
  getCoreRowModel,
  getPaginationRowModel,
  useReactTable,
} from "@tanstack/react-table";

import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";

import DataTableToolbar from "./DataTableToolbar";
import DataTablePagination from "./DataTablePagination";

const DataTable = ({ columns, data, tipo, onToggle }) => {
  const table = useReactTable({
  data: data ?? [],
  columns,

  getCoreRowModel:
    getCoreRowModel(),

  getPaginationRowModel:
    getPaginationRowModel(),

  initialState: {
    pagination: {
      pageSize: 10,
    },
  },
});

  return (
    <div
      className="
        overflow-hidden
        rounded-xl
        border border-slate-200/70
        bg-white
        shadow-sm
      "
    >
      <DataTableToolbar tipo={tipo} setTipo={onToggle} />
      <div className="overflow-x-auto">
        <Table className="min-w-[700px]">
          <TableHeader className="bg-[hsl(var(--nav-bg))/0.03]">
            {table.getHeaderGroups().map((headerGroup) => (
              <TableRow
                key={headerGroup.id}
                className="border-b border-slate-200/80 hover:bg-transparent"
              >
                {headerGroup.headers.map((header) => {
                  const isActions = header.column.id === "acciones";
                  return (
                    <TableHead
                      key={header.id}
                      className={`
                        h-11 px-4 text-xs sm:text-base font-bold text-slate-900 whitespace-nowrap
                        ${isActions ? "text-right w-[120px]" : ""}
                      `}
                    >
                      <div
                        className={`flex items-center gap-2 ${isActions ? "justify-end" : ""}`}
                      >
                        {!isActions && (
                          <div className="h-4 w-[3px] rounded-full bg-[hsl(var(--nav-bg))]" />
                        )}
                        <span>
                          {header.isPlaceholder
                            ? null
                            : flexRender(
                                header.column.columnDef.header,
                                header.getContext(),
                              )}
                        </span>
                      </div>
                    </TableHead>
                  );
                })}
              </TableRow>
            ))}
          </TableHeader>

          <TableBody>
            {table.getRowModel().rows?.length ? (
              table.getRowModel().rows.map((row) => (
                <TableRow
                  key={row.id}
                  className="border-b border-slate-100 transition-colors hover:bg-slate-50/80"
                >
                  {row.getVisibleCells().map((cell) => {
                    const isActions = cell.column.id === "acciones";
                    return (
                      <TableCell
                        key={cell.id}
                        className={`
                          px-4 py-0.5 text-sm text-slate-900 font-normal whitespace-nowrap
                          ${isActions ? "text-right w-[120px]" : ""}
                        `}
                      >
                        {flexRender(
                          cell.column.columnDef.cell,
                          cell.getContext(),
                        )}
                      </TableCell>
                    );
                  })}
                </TableRow>
              ))
            ) : (
              <TableRow>
                <TableCell
                  colSpan={columns.length}
                  className="h-32 text-center text-sm text-slate-500"
                >
                  No hay resultados para mostrar.
                </TableCell>
              </TableRow>
            )}
          </TableBody>
        </Table>
      </div>

      <DataTablePagination table={table} />
    </div>
  );
};

export default DataTable;
