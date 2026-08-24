import { ChevronDown, Filter, RotateCcw } from "lucide-react";
import { Button } from "@/components/ui/button";
import { AppSelect } from "@/components/ui/custom/AppSelect";

export const ModeloVehiculoFiltrosButton = ({ isOpen, activeCount, onToggle }) => (
  <Button
    type="button"
    size="sm"
    variant="outline"
    onClick={onToggle}
    className={`h-8 gap-2 border-slate-300 bg-white text-slate-600 shadow-sm hover:bg-slate-100 hover:text-slate-900 ${
      activeCount ? "border-[hsl(var(--nav-bg)/0.35)] text-[hsl(var(--nav-bg))]" : ""
    }`}
    aria-expanded={isOpen}
  >
    <Filter className="h-3.5 w-3.5" />
    Filtros
    {activeCount > 0 && (
      <span className="flex h-5 min-w-5 items-center justify-center rounded-full bg-[hsl(var(--nav-bg))] px-1 text-[11px] font-bold text-white">
        {activeCount}
      </span>
    )}
    <ChevronDown className={`h-3.5 w-3.5 transition-transform ${isOpen ? "rotate-180" : ""}`} />
  </Button>
);

const ModeloVehiculoFiltros = ({
  marcaVehiculoId,
  tipoVehiculoId,
  marcas,
  tiposVehiculos,
  onMarcaChange,
  onTipoChange,
  onClear,
}) => {
  const activeCount = Number(marcaVehiculoId !== "todos") + Number(tipoVehiculoId !== "todos");
  const marcaOptions = [
    { value: "todos", label: "TODAS" },
    ...marcas.map((marca) => ({ value: marca.marcaVehiculoId, label: marca.nombre })),
  ];
  const tipoOptions = [
    { value: "todos", label: "TODOS" },
    ...tiposVehiculos.map((tipo) => ({ value: tipo.tipoVehiculoId, label: tipo.nombre })),
  ];

  return (
    <div className="border-b border-slate-200 bg-[hsl(var(--nav-bg)/0.035)] px-4 py-3">
      <div className="flex flex-col items-stretch gap-3 sm:flex-row sm:items-end">
        <div className="grid flex-1 grid-cols-1 gap-3 sm:max-w-2xl sm:grid-cols-2">
          <AppSelect label="Marca" value={marcaVehiculoId} onValueChange={onMarcaChange} options={marcaOptions} />
          <AppSelect label="Tipo de vehículo" value={tipoVehiculoId} onValueChange={onTipoChange} options={tipoOptions} />
        </div>

        {activeCount > 0 && (
          <Button type="button" variant="ghost" size="sm" onClick={onClear} className="h-10 gap-1.5 text-xs text-slate-500 hover:text-[hsl(var(--nav-bg))]">
            <RotateCcw className="h-3.5 w-3.5" />
            Limpiar
          </Button>
        )}
      </div>
    </div>
  );
};

export default ModeloVehiculoFiltros;
