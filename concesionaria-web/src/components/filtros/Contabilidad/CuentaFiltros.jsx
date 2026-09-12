import { RotateCcw } from "lucide-react";
import { Button } from "@/components/ui/button";
import { AppSelect } from "@/components/ui/custom/AppSelect";
import { FilterButton } from "@/components/ui/custom/FilterButton";

const tiposCuenta = [
  { value: "todos", label: "TODAS" },
  { value: "1", label: "ACTIVO" },
  { value: "2", label: "PASIVO" },
  { value: "3", label: "PATRIMONIO" },
  { value: "4", label: "INGRESO" },
  { value: "5", label: "EGRESO" },
];

export const CuentaFiltrosButton = FilterButton;

const CuentaFiltros = ({
  tipoCuenta,
  nivel,
  niveles,
  onTipoCuentaChange,
  onNivelChange,
  onClear,
}) => {
  const activeCount =
    Number(tipoCuenta !== "todos") + Number(nivel !== "todos");
  const nivelOptions = [
    { value: "todos", label: "TODOS" },
    ...niveles.map((item) => ({
      value: String(item),
      label: `NIVEL ${item}`,
    })),
  ];

  return (
    <div className="border-b border-slate-200 bg-[hsl(var(--nav-bg)/0.035)] px-4 py-3">
      <div className="flex flex-col items-stretch gap-3 sm:flex-row sm:items-end">
        <div className="grid flex-1 grid-cols-1 gap-3 sm:max-w-2xl sm:grid-cols-2">
          <AppSelect
            label="Tipo de cuenta"
            value={tipoCuenta}
            onValueChange={onTipoCuentaChange}
            options={tiposCuenta}
          />

          <AppSelect
            label="Nivel"
            value={nivel}
            onValueChange={onNivelChange}
            options={nivelOptions}
          />
        </div>

        {activeCount > 0 && (
          <Button
            type="button"
            variant="ghost"
            size="sm"
            onClick={onClear}
            className="h-10 gap-1.5 text-xs text-slate-500 hover:text-[hsl(var(--nav-bg))]"
          >
            <RotateCcw className="h-3.5 w-3.5" />
            Limpiar
          </Button>
        )}
      </div>
    </div>
  );
};

export default CuentaFiltros;
