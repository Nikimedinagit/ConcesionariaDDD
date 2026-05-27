import {
  ChevronLeft,
  ChevronRight,
} from "lucide-react";

import { Button } from "@/components/ui/button";

const DataTablePagination = ({
  table,
}) => {
  return (
    <div
      className="
        flex items-center justify-between
        border-t border-slate-100
        px-4 py-3
      "
    >
      <p className="text-sm text-slate-500">
        Página{" "}
        {table.getState().pagination.pageIndex + 1}
      </p>

      <div className="flex items-center gap-2">
        <Button
          size="icon"
          variant="outline"
          onClick={() => table.previousPage()}
          disabled={!table.getCanPreviousPage()}
          className="border-slate-200"
        >
          <ChevronLeft className="h-6 w-6 font-bold" />
        </Button>

        <Button
          size="icon"
          variant="outline"
          onClick={() => table.nextPage()}
          disabled={!table.getCanNextPage()}
          className="border-slate-200"
        >
          <ChevronRight className="h-6 w-6 font-bold" />
        </Button>
      </div>
    </div>
  );
};

export default DataTablePagination;