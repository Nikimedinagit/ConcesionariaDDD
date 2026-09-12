import {
  ChevronLeft,
  ChevronRight,
} from "lucide-react";

import { Button } from "@/components/ui/button";

const DataTablePagination = ({
  table,
  total = 0,
  pageIndex = 0,
  pageCount,
  onPreviousPage,
  onNextPage,
}) => {

  const currentPage =
    table ? table.getState().pagination.pageIndex + 1 : pageIndex + 1;

  const totalPages =
    table ? table.getPageCount() : pageCount;

  const canPreviousPage = table
    ? table.getCanPreviousPage()
    : pageIndex > 0;
  const canNextPage = table
    ? table.getCanNextPage()
    : pageIndex + 1 < pageCount;

  return (
    <div
      className="
        flex items-center justify-between
        border-t border-slate-100
        px-4 py-3
      "
    >

      {/* IZQUIERDA */}
      <div className="flex items-center gap-4">

        {/* TOTAL */}
        <div className="flex items-center gap-2">

          <div
            className="
              h-2.5 w-2.5
              rounded-full
              bg-[hsl(var(--nav-bg))]
            "
          />

          <p
            className="
              text-sm
              font-medium
              text-slate-600
            "
          >
            Total:
            <span
              className="
                ml-1
                font-bold
                text-slate-900
              "
            >
              {total}
            </span>
          </p>

        </div>

        {/* PAGINA */}
        <p
          className="
            text-sm
            text-slate-500
          "
        >
          Página{" "}
          <span className="font-semibold text-slate-900">
            {currentPage}
          </span>

          {" "}de{" "}

          <span className="font-semibold text-slate-900">
            {totalPages}
          </span>
        </p>

      </div>

      {/* DERECHA */}
      <div className="flex items-center gap-2">

        <Button
          size="icon"
          variant="outline"
          onClick={() => table ? table.previousPage() : onPreviousPage()}
          disabled={!canPreviousPage}
          className="border-slate-200"
        >
          <ChevronLeft className="h-6 w-6 font-bold" />
        </Button>

        <Button
          size="icon"
          variant="outline"
          onClick={() => table ? table.nextPage() : onNextPage()}
          disabled={!canNextPage}
          className="border-slate-200"
        >
          <ChevronRight className="h-6 w-6 font-bold" />
        </Button>

      </div>

    </div>
  );
};

export default DataTablePagination;
