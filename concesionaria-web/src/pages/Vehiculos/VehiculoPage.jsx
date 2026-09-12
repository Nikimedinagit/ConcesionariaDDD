import { useEffect, useState } from "react";
import { CarFront } from "lucide-react";
import PageHeader from "@/components/ui/custom/PageHeader";
import AddButton from "@/components/ui/custom/AddButton";
import { TooltipProvider } from "@/components/ui/tooltip";
import { VehiculoModal } from "@/components/modals/Vehiculo/VehiculoModal";
import { VehiculoCards } from "@/components/cards/VehiculoCards";
import { useVehiculos } from "@/hooks/Vehiculo/useVehiculos";
import VehiculoService from "@/services/Vehiculo/vehiculoService";
import ModeloVehiculoService from "@/services/Vehiculo/modeloVehiculoService";
import { toastService } from "@/services/toastService";

export const VehiculoPage = () => {
  const [tipo, setTipo] = useState("disponibles");
  const [filtro, setFiltro] = useState("");
  const [debouncedFiltro, setDebouncedFiltro] = useState("");
  const { data, loading, refetch } = useVehiculos(tipo, debouncedFiltro);
  const [modelos, setModelos] = useState([]);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedVehiculo, setSelectedVehiculo] = useState(null);
  const [modalLoading, setModalLoading] = useState(false);
  const [serverError, setServerError] = useState("");
  const [serverFieldErrors, setServerFieldErrors] = useState({});

  useEffect(() => {
    const handler = setTimeout(() => setDebouncedFiltro(filtro), 500);
    return () => clearTimeout(handler);
  }, [filtro]);

  useEffect(() => {
    ModeloVehiculoService.getActivas()
      .then(setModelos)
      .catch(() =>
        toastService.error("Error", {
          description: "No se pudieron cargar los modelos de vehículos",
        }),
      );
  }, []);

  const openModal = (vehiculo = null) => {
    setServerError("");
    setServerFieldErrors({});
    setSelectedVehiculo(vehiculo);
    setIsModalOpen(true);
  };

  const closeModal = () => {
    setIsModalOpen(false);
    setSelectedVehiculo(null);
    setServerError("");
    setServerFieldErrors({});
  };

  const handleSave = async (payload) => {
    setModalLoading(true);
    setServerError("");
    setServerFieldErrors({});

    try {
      if (payload.vehiculoId) {
        await VehiculoService.actualizar(payload.vehiculoId, payload);
        toastService.success("Éxito", {
          description: "Vehículo actualizado correctamente",
        });
      } else {
        await VehiculoService.crear(payload);
        toastService.success("Éxito", {
          description: "Vehículo agregado correctamente",
        });
      }

      closeModal();
      refetch();
    } catch (error) {
      const dataError = error.response?.data;
      const validationErrors = dataError?.errors || [];
      const fieldErrors = validationErrors.reduce((result, item) => {
        if (!item?.propertyName) return result;

        const fieldName =
          item.propertyName.charAt(0).toLowerCase() + item.propertyName.slice(1);
        result[fieldName] = item.errorMessage;
        return result;
      }, {});

      setServerFieldErrors(fieldErrors);
      setServerError(
        Object.keys(fieldErrors).length > 0
          ? "No se pudo guardar el vehículo. Revisá los campos marcados."
          : validationErrors[0]?.errorMessage ||
          dataError?.message ||
          dataError?.mensaje ||
          "Ocurrió un error al guardar el vehículo",
      );
    } finally {
      setModalLoading(false);
    }
  };

  return (
    <TooltipProvider delayDuration={300}>
      <div>
        <PageHeader title="Vehículos" icon={CarFront}>
          <AddButton onClick={() => openModal()}>Nuevo Vehículo</AddButton>
        </PageHeader>

        {loading && data.length === 0 && !debouncedFiltro ? (
          <div className="flex h-64 items-center justify-center">
            Cargando...
          </div>
        ) : (
          <VehiculoCards
            data={data}
            tipo={tipo}
            onTipoChange={setTipo}
            onSearch={setFiltro}
            onEdit={openModal}
          />
        )}

        {isModalOpen && (
          <VehiculoModal
            isOpen={isModalOpen}
            onClose={closeModal}
            onSave={handleSave}
            vehiculo={selectedVehiculo}
            modelos={modelos}
            loading={modalLoading}
            serverError={serverError}
            serverFieldErrors={serverFieldErrors}
          />
        )}
      </div>
    </TooltipProvider>
  );
};
