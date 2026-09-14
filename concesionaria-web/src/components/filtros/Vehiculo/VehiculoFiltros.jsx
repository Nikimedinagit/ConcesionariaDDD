import {RotateCcw } from "lucide-react";
import { Button } from "@/components/ui/button";
import { AppInput } from "@/components/ui/custom/AppInput";
import { AppSelect } from "@/components/ui/custom/AppSelect";
import { FilterButton } from "@/components/ui/custom/FilterButton";
import { useAuth } from "@/context/AuthContext";

const condiciones = [
  { value: "todos", label: "TODAS" },
  { value: "1", label: "NUEVO" },
  { value: "2", label: "USADO" },
  { value: "3", label: "CONSIGNACIÓN" },
];

export const VehiculoFiltrosButton = FilterButton;

const VehiculoFiltros = ({
  marca,
  modelo,
  condicion,
  anio,
  sucursalId,
  marcas,
  modelos,
  sucursales,
  onMarcaChange,
  onModeloChange,
  onCondicionChange,
  onAnioChange,
  onSucursalChange,
  onClear,
  showSucursalFilter = true,
}) => {
  const { user } = useAuth();
  const activeCount =
    [marca, modelo, condicion].filter((value) => value !== "todos")
      .length +
    Number(Boolean(anio)) +
    Number(showSucursalFilter && sucursalId !== "actual");

  const sucursalOptions = [
    {
      value: "actual",
      label: user?.sucursalNombre || "SUCURSAL ACTUAL",
    },
    ...sucursales
      .filter(
        (sucursal) =>
          String(sucursal.sucursalId).toLowerCase() !==
          String(user?.sucursalId).toLowerCase(),
      )
      .map((sucursal) => ({
        value: sucursal.sucursalId,
        label: sucursal.nombre,
      })),
  ];

  const marcaOptions = [
    { value: "todos", label: "TODAS" },
    ...marcas.map((item) => ({ value: item, label: item })),
  ];

  const modeloOptions = [
    { value: "todos", label: "TODOS" },
    ...modelos.map((item) => ({
      value: item.modeloId,
      label: item.modeloNombre,
    })),
  ];

  return (
    <div className="border-b border-slate-200 bg-[hsl(var(--nav-bg)/0.035)] px-4 py-3">
      <div className="flex flex-col items-stretch gap-3 xl:flex-row xl:items-end">
        <div className="grid flex-1 grid-cols-1 gap-3 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-5">
          {showSucursalFilter && (
            <AppSelect
              label="Sucursal"
              value={sucursalId}
              onValueChange={onSucursalChange}
              options={sucursalOptions}
            />
          )}
          <AppSelect
            label="Marca"
            value={marca}
            onValueChange={onMarcaChange}
            options={marcaOptions}
          />
          <AppSelect
            label="Modelo"
            value={modelo}
            onValueChange={onModeloChange}
            options={modeloOptions}
          />
          <AppSelect
            label="Condición"
            value={condicion}
            onValueChange={onCondicionChange}
            options={condiciones}
          />
          <AppInput
            label="Año"
            type="number"
            min="1900"
            max={new Date().getFullYear()}
            placeholder="Ej: 2024"
            value={anio}
            onChange={(event) =>
              onAnioChange(event.target.value.replace(/\D/g, "").slice(0, 4))
            }
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

export default VehiculoFiltros;
