import { useEffect, useState } from "react";
import { CarFront } from "lucide-react";
import PageHeader from "@/components/ui/custom/PageHeader";
import AddButton from "@/components/ui/custom/AddButton";
import { TipoVehiculoModal } from "@/components/modals/Vehiculo/TipoVehiculoModal";
import TipoVehiculoTable from "@/components/tables/Vehiculo/TipoVehiculoTable";
import { TooltipProvider } from "@/components/ui/tooltip";
import { useTiposVehiculos } from "@/hooks/Vehiculo/useTiposVehiculos";
import TipoVehiculoService from "@/services/Vehiculo/tipoVehiculoService";
import { toastService } from "@/services/toastService";
import { usePermissions } from "@/context/PermissionContext";

export const TipoVehiculoPage = () => {
  const { can } = usePermissions();
  const [tipo, setTipo] = useState("activas");
  const [filtro, setFiltro] = useState("");
  const [debouncedFiltro, setDebouncedFiltro] = useState("");
  const { data, loading, refetch } = useTiposVehiculos(tipo, debouncedFiltro);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedTipoVehiculo, setSelectedTipoVehiculo] = useState(null);
  const [modalLoading, setModalLoading] = useState(false);
  const [serverError, setServerError] = useState("");

  useEffect(() => {
    const handler = setTimeout(() => {
      setDebouncedFiltro(filtro);
    }, 500);

    return () => clearTimeout(handler);
  }, [filtro]);

  const handleOpenCreate = () => {
    setServerError("");
    setSelectedTipoVehiculo(null);
    setIsModalOpen(true);
  };

  const handleOpenEdit = (tipoVehiculo) => {
    setServerError("");
    setSelectedTipoVehiculo(tipoVehiculo);
    setIsModalOpen(true);
  };

  const handleSave = async (payload) => {
    setModalLoading(true);
    setServerError("");

    try {
      if (payload.tipoVehiculoId) {
        await TipoVehiculoService.actualizar(payload.tipoVehiculoId, payload);
        toastService.success("Éxito", {
          description: "Tipo de vehículo actualizado correctamente",
        });
      } else {
        await TipoVehiculoService.crear({ nombre: payload.nombre });
        toastService.success("Éxito", {
          description: "Tipo de vehículo creado correctamente",
        });
      }

      setIsModalOpen(false);
      refetch();
    } catch (error) {
      const dataError = error.response?.data;
      const mensajeError =
        dataError?.errors?.[0]?.errorMessage ||
        dataError?.message ||
        dataError?.mensaje ||
        "Ocurrió un error al guardar";

      setServerError(mensajeError);
    } finally {
      setModalLoading(false);
    }
  };

  const handleToggleStatus = async (tipoVehiculo) => {
    try {
      if (tipo === "activas") {
        await TipoVehiculoService.desactivar(tipoVehiculo.tipoVehiculoId);
        toastService.success("Éxito", {
          description: "Tipo de vehículo desactivado correctamente",
        });
      } else {
        await TipoVehiculoService.activar(tipoVehiculo.tipoVehiculoId);
        toastService.success("Éxito", {
          description: "Tipo de vehículo activado correctamente",
        });
      }

      refetch();
    } catch (error) {
      const dataError = error.response?.data;
      const mensajeError =
        dataError?.errors?.[0]?.errorMessage ||
        dataError?.message ||
        dataError?.mensaje ||
        "No se pudo cambiar el estado del tipo de vehículo";

      toastService.error("No se puede desactivar", {
        description: mensajeError,
      });
    }
  };

  return (
    <TooltipProvider delayDuration={300}>
      <div>
        <PageHeader title="Tipos de Vehículos" icon={CarFront}>
          {can("TIPOS_VEHICULO_CREAR") && (
            <AddButton onClick={handleOpenCreate}>
              Nuevo Tipo de Vehículo
            </AddButton>
          )}
        </PageHeader>

        {loading && data.length === 0 && !debouncedFiltro ? (
          <div className="flex h-64 items-center justify-center">
            Cargando...
          </div>
        ) : (
          <TipoVehiculoTable
            data={data}
            tipo={tipo}
            onToggle={setTipo}
            onSearch={setFiltro}
            onEdit={handleOpenEdit}
            onToggleStatus={handleToggleStatus}
            canEdit={can("TIPOS_VEHICULO_EDITAR")}
            canActivate={can("TIPOS_VEHICULO_ACTIVAR")}
            canDeactivate={can("TIPOS_VEHICULO_DESACTIVAR")}
          />
        )}

        {isModalOpen && (
          <TipoVehiculoModal
            isOpen={isModalOpen}
            onClose={() => setIsModalOpen(false)}
            onSave={handleSave}
            tipoVehiculo={selectedTipoVehiculo}
            loading={modalLoading}
            serverError={serverError}
          />
        )}
      </div>
    </TooltipProvider>
  );
};
