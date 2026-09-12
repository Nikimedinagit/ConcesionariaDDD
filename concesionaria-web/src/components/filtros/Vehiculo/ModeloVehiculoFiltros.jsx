import { RotateCcw } from "lucide-react";
import { Button } from "@/components/ui/button";
import { AppSelect } from "@/components/ui/custom/AppSelect";
import { FilterButton } from "@/components/ui/custom/FilterButton";

export const ModeloVehiculoFiltrosButton = FilterButton;

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
