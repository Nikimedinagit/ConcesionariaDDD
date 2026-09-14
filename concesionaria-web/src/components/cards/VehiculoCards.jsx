/* eslint-disable react-hooks/set-state-in-effect */
import { useEffect, useMemo, useState } from "react";
import { Search } from "lucide-react";
import { Button } from "@/components/ui/button";
import { AppInput } from "@/components/ui/custom/AppInput";
import { VehiculoCard } from "./VehiculoCard";
import VehiculoFiltros, {
  VehiculoFiltrosButton,
} from "@/components/filtros/Vehiculo/VehiculoFiltros";
import DataTablePagination from "@/components/tables/DataTablePagination";

const PAGE_SIZE = 9;

const filtros = [
  { value: "disponibles", label: "Disponibles" },
  { value: "reservados", label: "Reservados" },
  { value: "vendidos", label: "Vendidos" },
  { value: "servicio", label: "En Servicio" },
];

export function VehiculoCards({
  data,
  loading = false,
  tipo,
  onTipoChange,
  onSearch,
  sucursalFiltro,
  onSucursalChange,
  sucursales = [],
  onEdit,
  onDelete,
}) {
  const [showFilters, setShowFilters] = useState(false);
  const [marca, setMarca] = useState("todos");
  const [modelo, setModelo] = useState("todos");
  const [condicion, setCondicion] = useState("todos");
  const [anio, setAnio] = useState("");
  const [pageIndex, setPageIndex] = useState(0);

  const marcas = useMemo(
    () =>
      [...new Set(data.map((item) => item.marcaNombre).filter(Boolean))].sort(),
    [data],
  );

  const modelos = useMemo(() => {
    const uniqueModels = new Map();

    data
      .filter((item) => marca === "todos" || item.marcaNombre === marca)
      .forEach((item) => {
        uniqueModels.set(item.modeloId, {
          modeloId: item.modeloId,
          modeloNombre: item.modeloNombre,
        });
      });

    return [...uniqueModels.values()].sort((a, b) =>
      a.modeloNombre.localeCompare(b.modeloNombre),
    );
  }, [data, marca]);

  const filteredData = useMemo(
    () =>
      data.filter(
        (item) =>
          (marca === "todos" || item.marcaNombre === marca) &&
          (modelo === "todos" || item.modeloId === modelo) &&
          (condicion === "todos" || String(item.condicion) === condicion) &&
          (!anio || String(item.anio) === anio),
      ),
    [data, marca, modelo, condicion, anio],
  );

  const activeCount =
    [marca, modelo, condicion].filter((value) => value !== "todos")
      .length +
    Number(Boolean(anio)) +
    Number(sucursalFiltro !== "actual");

  const pageCount = Math.max(1, Math.ceil(filteredData.length / PAGE_SIZE));
  const paginatedData = filteredData.slice(
    pageIndex * PAGE_SIZE,
    (pageIndex + 1) * PAGE_SIZE,
  );

  useEffect(() => {
    setPageIndex(0);
  }, [data, tipo, marca, modelo, condicion, anio, sucursalFiltro]);

  const clearFilters = () => {
    setMarca("todos");
    setModelo("todos");
    setCondicion("todos");
    setAnio("");
    onSucursalChange("actual");
  };

  const handleMarcaChange = (value) => {
    setMarca(value);
    setModelo("todos");
  };

  return (
    <div className="overflow-hidden rounded-xl border border-slate-300/80 bg-white shadow-[0_4px_16px_rgba(15,23,42,0.07)]">
      <div className="flex flex-col gap-4 border-b border-slate-200 bg-slate-50/60 px-4 py-4 lg:flex-row lg:items-center lg:justify-between">
        <div className="flex flex-wrap items-center gap-2">
          {filtros.map((filtro) => (
            <Button
              key={filtro.value}
              size="sm"
              onClick={() => {
                setPageIndex(0);
                onTipoChange(filtro.value);
              }}
              className={
                tipo === filtro.value
                  ? "bg-[hsl(var(--nav-bg))] text-white hover:opacity-90"
                  : "border border-slate-200 bg-transparent text-slate-600 hover:bg-slate-50"
              }
            >
              {filtro.label}
            </Button>
          ))}
        </div>

        <div className="flex w-full items-center gap-2 lg:w-auto">
          <AppInput
            icon={Search}
            placeholder="Buscar..."
            onChange={(event) => onSearch(event.target.value.toUpperCase())}
            autoComplete="new-password"
            name="table-filter-value"
            autoCapitalize="none"
            spellCheck={false}
            data-form-type="other"
            data-lpignore="true"
            className="w-full [&_input]:h-8 md:ml-auto md:w-[280px]"
          />
          <VehiculoFiltrosButton
            isOpen={showFilters}
            activeCount={activeCount}
            onToggle={() => setShowFilters((current) => !current)}
          />
        </div>
      </div>

      {showFilters && (
        <VehiculoFiltros
          marca={marca}
          modelo={modelo}
          condicion={condicion}
          anio={anio}
          sucursalId={sucursalFiltro}
          sucursales={sucursales}
          marcas={marcas}
          modelos={modelos}
          onMarcaChange={handleMarcaChange}
          onModeloChange={setModelo}
          onCondicionChange={setCondicion}
          onAnioChange={setAnio}
          onSucursalChange={onSucursalChange}
          showSucursalFilter={false}
          onClear={clearFilters}
        />
      )}

      {loading && data.length === 0 ? (
        <div className="flex h-32 items-center justify-center text-sm text-slate-500">
          Cargando...
        </div>
      ) : paginatedData.length ? (
        <div className="grid gap-3 p-4 sm:grid-cols-2 xl:grid-cols-3">
          {paginatedData.map((vehiculo) => (
            <VehiculoCard
              key={vehiculo.vehiculoId}
              vehiculo={vehiculo}
              onEdit={onEdit}
              onDelete={onDelete}
            />
          ))}
        </div>
      ) : (
        <div className="flex h-32 items-center justify-center text-sm text-slate-500">
          No hay vehículos para mostrar.
        </div>
      )}

      <DataTablePagination
        total={filteredData.length}
        pageIndex={pageIndex}
        pageCount={pageCount}
        onPreviousPage={() => setPageIndex((current) => current - 1)}
        onNextPage={() => setPageIndex((current) => current + 1)}
      />
    </div>
  );
}
