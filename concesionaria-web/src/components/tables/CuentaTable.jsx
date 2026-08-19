import { useMemo, useState } from "react";
import {
  getCoreRowModel,
  getPaginationRowModel,
  useReactTable,
} from "@tanstack/react-table";
import {
  ChevronDown,
  ChevronRight,
  CirclePlus,
  LockKeyhole,
  RotateCcw,
  SquarePen,
  Trash2,
} from "lucide-react";
import DataTableToolbar from "./DataTableToolbar";
import DataTablePagination from "./DataTablePagination";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import { Button } from "@/components/ui/button";
import { Tooltip } from "@/components/ui/custom/TooltipCustom";
import CuentaFiltros, {
  CuentaFiltrosButton,
} from "@/components/filtros/CuentaFiltros";

const getChildren = (cuentas, cuentaId) =>
  cuentas
    .filter(
      (cuenta) =>
        cuenta.cuentaPadreId != null &&
        String(cuenta.cuentaPadreId) === String(cuentaId),
    )
    .sort((a, b) => a.codigo.localeCompare(b.codigo));

const tiposCuenta = {
  1: "ACTIVO",
  2: "PASIVO",
  3: "PATRIMONIO",
  4: "INGRESO",
  5: "EGRESO",
};

const getTipoLabel = (tipo) => tiposCuenta[tipo] ?? tipo;

const tipoBadgeClassName = {
  1: "border-sky-100 bg-sky-50 text-sky-700",
  2: "border-rose-100 bg-rose-50 text-rose-700",
  3: "border-violet-100 bg-violet-50 text-violet-700",
  4: "border-emerald-100 bg-emerald-50 text-emerald-700",
  5: "border-amber-100 bg-amber-50 text-amber-700",
};

const isCuentaBase = (cuenta) =>
  cuenta.nivel === 0 &&
  cuenta.cuentaPadreId === null &&
  (
    cuenta.codigo === "1" && cuenta.nombre === "ACTIVO" && cuenta.tipo === 1 ||
    cuenta.codigo === "2" && cuenta.nombre === "PASIVO" && cuenta.tipo === 2 ||
    cuenta.codigo === "3" && cuenta.nombre === "PATRIMONIO NETO" && cuenta.tipo === 3 ||
    cuenta.codigo === "4" && cuenta.nombre === "INGRESO" && cuenta.tipo === 4 ||
    cuenta.codigo === "5" && cuenta.nombre === "EGRESO" && cuenta.tipo === 5
  );

const hasVisibleDescendant = (cuentas, cuentaId, tipo, search) => {
  const children = getChildren(cuentas, cuentaId).filter((cuenta) =>
    tipo === "activas" ? !cuenta.eliminado : cuenta.eliminado,
  );

  return children.some((child) => {
    const matchesSearch =
      !search ||
      child.nombre.includes(search) ||
      child.codigo.includes(search) ||
      getTipoLabel(child.tipo).includes(search);

    return (
      matchesSearch || hasVisibleDescendant(cuentas, child.cuentaId, tipo, search)
    );
  });
};

const buildVisibleRows = ({
  cuentas,
  expandedIds,
  tipo,
  search,
  tipoCuenta,
  nivel,
}) => {
  const matchesAdvancedFilters = (cuenta) =>
    (tipoCuenta === "todos" || String(cuenta.tipo) === tipoCuenta) &&
    (nivel === "todos" || String(cuenta.nivel) === nivel);

  if (
    tipo === "inactivas" ||
    tipoCuenta !== "todos" ||
    nivel !== "todos"
  ) {
    return cuentas
      .filter(
        (cuenta) =>
          matchesAdvancedFilters(cuenta) &&
          (!search ||
            cuenta.nombre.includes(search) ||
            cuenta.codigo.includes(search) ||
            getTipoLabel(cuenta.tipo).includes(search)),
      )
      .sort((a, b) => a.codigo.localeCompare(b.codigo));
  }

  const rows = [];
  const roots = cuentas
    .filter((cuenta) => cuenta.cuentaPadreId === null)
    .sort((a, b) => a.codigo.localeCompare(b.codigo));

  const visit = (cuenta) => {
    const matchesSearch =
      !search ||
      cuenta.nombre.includes(search) ||
      cuenta.codigo.includes(search) ||
      getTipoLabel(cuenta.tipo).includes(search);
    const keepByChild = hasVisibleDescendant(cuentas, cuenta.cuentaId, tipo, search);

    if ((matchesSearch || keepByChild) && matchesAdvancedFilters(cuenta)) {
      rows.push(cuenta);
    }

    if (
      expandedIds.has(cuenta.cuentaId) ||
      search ||
      tipoCuenta !== "todos" ||
      nivel !== "todos"
    ) {
      getChildren(cuentas, cuenta.cuentaId).forEach(visit);
    }
  };

  roots.forEach(visit);
  return rows;
};

const CuentaTable = ({
  data,
  cuentasDisponibles,
  tipo,
  onToggle,
  onSearch,
  tipoCuenta,
  nivel,
  onTipoCuentaChange,
  onNivelChange,
  onAddChild,
  onEdit,
  onToggleStatus,
}) => {
  const [expandedIds, setExpandedIds] = useState(new Set());
  const [searchValue, setSearchValue] = useState("");
  const [filtersOpen, setFiltersOpen] = useState(false);
  const activeFiltersCount =
    Number(tipoCuenta !== "todos") + Number(nivel !== "todos");

  const niveles = useMemo(
    () => [...new Set(cuentasDisponibles.map((cuenta) => cuenta.nivel))].sort((a, b) => a - b),
    [cuentasDisponibles],
  );

  const visibleRows = useMemo(
    () =>
      buildVisibleRows({
        cuentas: data,
        expandedIds,
        tipo,
        search: searchValue,
        tipoCuenta,
        nivel,
      }),
    [data, expandedIds, tipo, searchValue, tipoCuenta, nivel],
  );

  const paginationColumns = useMemo(
    () => [{ accessorKey: "cuentaId" }],
    [],
  );

  const table = useReactTable({
    data: visibleRows,
    columns: paginationColumns,
    getCoreRowModel: getCoreRowModel(),
    getPaginationRowModel: getPaginationRowModel(),
    initialState: {
      pagination: {
        pageSize: 8,
      },
    },
  });

  const handleSearch = (value) => {
    setSearchValue(value);
    onSearch(value);
  };

  const toggleExpanded = (cuentaId) => {
    setExpandedIds((current) => {
      const next = new Set(current);
      if (next.has(cuentaId)) {
        next.delete(cuentaId);
      } else {
        next.add(cuentaId);
      }
      return next;
    });
  };

  return (
    <div className="rounded-xl border border-slate-300/80 bg-white shadow-[0_4px_16px_rgba(15,23,42,0.07)]">
      <DataTableToolbar
        tipo={tipo}
        setTipo={onToggle}
        onSearch={handleSearch}
        actions={
          <CuentaFiltrosButton
            isOpen={filtersOpen}
            activeCount={activeFiltersCount}
            onToggle={() => setFiltersOpen((current) => !current)}
          />
        }
      />

      {filtersOpen && (
        <CuentaFiltros
          tipoCuenta={tipoCuenta}
          nivel={nivel}
          niveles={niveles}
          onTipoCuentaChange={onTipoCuentaChange}
          onNivelChange={onNivelChange}
          onClear={() => {
            onTipoCuentaChange("todos");
            onNivelChange("todos");
          }}
        />
      )}

      <div className="overflow-x-auto">
        <Table className="min-w-[780px]">
          <TableHeader className="bg-[hsl(var(--nav-bg)/0.08)]">
            <TableRow className="border-b border-slate-200/80 hover:bg-transparent">
              <TableHead className="h-10 px-4 text-xs font-bold text-slate-900 sm:text-base">
                <div className="flex items-center gap-2">
                  <div className="h-4 w-[3px] rounded-full bg-[hsl(var(--nav-bg))]" />
                  Cuenta
                </div>
              </TableHead>
              <TableHead className="h-10 px-4 text-xs font-bold text-slate-900 sm:text-base">
                <div className="flex items-center gap-2">
                  <div className="h-4 w-[3px] rounded-full bg-[hsl(var(--nav-bg))]" />
                  Tipo
                </div>
              </TableHead>
              <TableHead className="h-10 px-4 text-xs font-bold text-slate-900 sm:text-base">
                <div className="flex items-center gap-2">
                  <div className="h-4 w-[3px] rounded-full bg-[hsl(var(--nav-bg))]" />
                  Nivel
                </div>
              </TableHead>
              <TableHead className="h-10 w-[180px] px-4 text-right text-xs font-bold text-slate-900 sm:text-base">
                Acciones
              </TableHead>
            </TableRow>
          </TableHeader>

          <TableBody>
            {table.getRowModel().rows.length ? (
              table.getRowModel().rows.map((row) => {
                const cuenta = row.original;
                const childrenCount =
                  tipo === "activas"
                    ? getChildren(data, cuenta.cuentaId).length
                    : 0;
                const isExpanded = expandedIds.has(cuenta.cuentaId) || searchValue;
                const protectedBase = isCuentaBase(cuenta);

                return (
                  <TableRow
                    key={cuenta.cuentaId}
                    onClick={() => {
                      if (tipo === "activas") {
                        toggleExpanded(cuenta.cuentaId);
                      }
                    }}
                    className="border-b border-slate-200/80 bg-white even:bg-slate-50/70 transition-colors hover:bg-[hsl(var(--nav-bg))]/[0.07]"
                  >
                    <TableCell className="px-4 py-0.5 text-sm text-slate-900">
                      <div
                        className="flex items-center gap-2"
                        style={{
                          paddingLeft:
                            tipo === "activas" ? `${cuenta.nivel * 22}px` : 0,
                        }}
                      >
                        {tipo === "activas" && childrenCount > 0 ? (
                          <Button
                            type="button"
                            size="icon"
                            variant="ghost"
                            onClick={(event) => {
                              event.stopPropagation();
                              toggleExpanded(cuenta.cuentaId);
                            }}
                            className="h-7 w-7 rounded-md bg-[hsl(var(--nav-bg)/0.08)] text-[hsl(var(--nav-bg))] hover:bg-[hsl(var(--nav-bg)/0.14)] hover:text-[hsl(var(--nav-bg))]"
                            aria-label={isExpanded ? "Contraer subcuentas" : "Expandir subcuentas"}
                          >
                            {isExpanded ? (
                              <ChevronDown className="h-4 w-4" />
                            ) : (
                              <ChevronRight className="h-4 w-4" />
                            )}
                          </Button>
                        ) : tipo === "activas" ? (
                          <span className="h-7 w-7 shrink-0" aria-hidden="true" />
                        ) : null}

                        <div className="min-w-0">
                          <div className="flex flex-wrap items-center gap-2">
                            <span className="font-bold text-slate-700">
                              {cuenta.codigo}
                            </span>
                            <span className="font-semibold">{cuenta.nombre}</span>
                            {protectedBase && (
                              <span className="inline-flex items-center gap-1 rounded-md bg-slate-100 px-2 py-0.5 text-xs font-semibold text-slate-500">
                                <LockKeyhole className="h-3 w-3" />
                                Base
                              </span>
                            )}
                          </div>
                        </div>
                      </div>
                    </TableCell>

                    <TableCell className="px-4 py-0.5 text-sm text-slate-700">
                      <span
                        className={`inline-flex min-w-[96px] items-center justify-center rounded-md border px-2 py-0.5 text-xs font-bold ${tipoBadgeClassName[cuenta.tipo] ?? "border-slate-200 bg-slate-50 text-slate-700"}`}
                      >
                        {getTipoLabel(cuenta.tipo)}
                      </span>
                    </TableCell>

                    <TableCell className="px-4 py-0.5 text-sm text-slate-700">
                      {cuenta.nivel}
                    </TableCell>

                    <TableCell className="px-4 py-0.5 text-right">
                      <div className="flex justify-end gap-0.5">
                        {tipo === "activas" && (
                          <>
                            <Tooltip text="Agregar">
                              <Button
                                type="button"
                                variant="ghost"
                                size="icon"
                                onClick={(event) => {
                                  event.stopPropagation();
                                  setExpandedIds((current) => new Set(current).add(cuenta.cuentaId));
                                  onAddChild(cuenta);
                                }}
                                className="h-7.5 w-7.5 rounded-lg text-emerald-500 hover:bg-emerald-50 hover:text-emerald-600"
                              >
                                <CirclePlus className="h-[18px] w-[18px]" />
                              </Button>
                            </Tooltip>

                            {!protectedBase && (
                              <>
                                <Tooltip text="Editar">
                                  <Button
                                    type="button"
                                    variant="ghost"
                                    size="icon"
                                    onClick={(event) => {
                                      event.stopPropagation();
                                      onEdit(cuenta);
                                    }}
                                    className="h-7.5 w-7.5 rounded-lg text-sky-500 hover:bg-sky-50 hover:text-sky-600"
                                  >
                                    <SquarePen className="h-[18px] w-[18px]" />
                                  </Button>
                                </Tooltip>

                                <Tooltip text="Desactivar">
                                  <Button
                                    type="button"
                                    variant="ghost"
                                    size="icon"
                                    onClick={(event) => {
                                      event.stopPropagation();
                                      onToggleStatus(cuenta);
                                    }}
                                    className="h-7.5 w-7.5 rounded-lg text-red-500 hover:bg-red-50 hover:text-red-600"
                                  >
                                    <Trash2 className="h-[18px] w-[18px]" />
                                  </Button>
                                </Tooltip>
                              </>
                            )}
                          </>
                        )}

                        {tipo === "inactivas" && !protectedBase && (
                          <Tooltip text="Activar">
                            <Button
                              type="button"
                              variant="ghost"
                              size="icon"
                              onClick={(event) => {
                                event.stopPropagation();
                                onToggleStatus(cuenta);
                              }}
                              className="h-7.5 w-7.5 rounded-lg text-emerald-500 hover:bg-emerald-50 hover:text-emerald-600"
                            >
                              <RotateCcw className="h-[18px] w-[18px]" />
                            </Button>
                          </Tooltip>
                        )}
                      </div>
                    </TableCell>
                  </TableRow>
                );
              })
            ) : (
              <TableRow>
                <TableCell
                  colSpan={4}
                  className="h-32 text-center text-sm text-slate-500"
                >
                  No hay resultados para mostrar.
                </TableCell>
              </TableRow>
            )}
          </TableBody>
        </Table>
      </div>

      <DataTablePagination table={table} total={visibleRows.length} />
    </div>
  );
};

export default CuentaTable;
