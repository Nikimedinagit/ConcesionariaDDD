import { useMemo, useState } from "react";
import DataTable from "../DataTable";
import { ActionButton } from "@/components/ui/custom/ActionButton";
import { Tooltip } from "@/components/ui/custom/TooltipCustom";
import ModeloVehiculoFiltros, {
  ModeloVehiculoFiltrosButton,
} from "@/components/filtros/Vehiculo/ModeloVehiculoFiltros";

const ModeloVehiculoTable = ({
  data,
  modelosDisponibles,
  tipo,
  onToggle,
  onSearch,
  onEdit,
  onToggleStatus,
  marcas,
  tiposVehiculos,
  marcaVehiculoId,
  tipoVehiculoId,
  onMarcaChange,
  onTipoChange,
  canEdit = true,
  canActivate = true,
  canDeactivate = true,
}) => {
  const [filtersOpen, setFiltersOpen] = useState(false);
  const activeFiltersCount =
    Number(marcaVehiculoId !== "todos") + Number(tipoVehiculoId !== "todos");
    
  const marcasPorId = useMemo(
    () => new Map(marcas.map((marca) => [marca.marcaVehiculoId, marca.nombre])),
    [marcas],
  );

  const tiposPorId = useMemo(
    () =>
      new Map(tiposVehiculos.map((item) => [item.tipoVehiculoId, item.nombre])),
    [tiposVehiculos],
  );
  const marcasDisponibles = useMemo(() => {
    const ids = new Set(
      modelosDisponibles.map((modelo) => modelo.marcaVehiculoId),
    );
    if (marcaVehiculoId !== "todos") ids.add(marcaVehiculoId);
    return marcas.filter((marca) => ids.has(marca.marcaVehiculoId));
  }, [modelosDisponibles, marcas, marcaVehiculoId]);
  const tiposDisponibles = useMemo(() => {
    const ids = new Set(
      modelosDisponibles.map((modelo) => modelo.tipoVehiculoId),
    );
    if (tipoVehiculoId !== "todos") ids.add(tipoVehiculoId);
    return tiposVehiculos.filter((tipoVehiculo) =>
      ids.has(tipoVehiculo.tipoVehiculoId),
    );
  }, [modelosDisponibles, tiposVehiculos, tipoVehiculoId]);
  const columns = useMemo(
    () => [
      { accessorKey: "nombre", header: "Nombre" },
      {
        id: "marca",
        header: "Marca",
        cell: ({ row }) => marcasPorId.get(row.original.marcaVehiculoId) || "—",
      },
      {
        id: "tipoVehiculo",
        header: "Tipo de vehículo",
        cell: ({ row }) => tiposPorId.get(row.original.tipoVehiculoId) || "—",
      },
      {
        id: "acciones",
        header: "Acciones",
        cell: ({ row }) => (
          <div className="flex justify-end gap-0.5">
            {tipo === "activas" ? (
              <>
                {canEdit && (
                  <Tooltip text="Editar">
                    <ActionButton
                      type="edit"
                      onClick={() => onEdit(row.original)}
                    />
                  </Tooltip>
                )}
                {canDeactivate && (
                  <Tooltip text="Desactivar">
                    <ActionButton
                      type="desactivar"
                      onClick={() => onToggleStatus(row.original)}
                    />
                  </Tooltip>
                )}
              </>
            ) : (
              canActivate && (
                <Tooltip text="Activar">
                  <ActionButton
                    type="activar"
                    onClick={() => onToggleStatus(row.original)}
                  />
                </Tooltip>
              )
            )}
          </div>
        ),
      },
    ],
    [
      tipo,
      onEdit,
      onToggleStatus,
      marcasPorId,
      tiposPorId,
      canEdit,
      canActivate,
      canDeactivate,
    ],
  );
  return (
    <DataTable
      columns={columns}
      data={data}
      tipo={tipo}
      onToggle={onToggle}
      onSearch={onSearch}
      toolbarActions={
        <ModeloVehiculoFiltrosButton
          isOpen={filtersOpen}
          activeCount={activeFiltersCount}
          onToggle={() => setFiltersOpen((current) => !current)}
        />
      }
      filters={
        filtersOpen && (
          <ModeloVehiculoFiltros
            marcaVehiculoId={marcaVehiculoId}
            tipoVehiculoId={tipoVehiculoId}
            marcas={marcasDisponibles}
            tiposVehiculos={tiposDisponibles}
            onMarcaChange={onMarcaChange}
            onTipoChange={onTipoChange}
            onClear={() => {
              onMarcaChange("todos");
              onTipoChange("todos");
            }}
          />
        )
      }
    />
  );
};

export default ModeloVehiculoTable;
