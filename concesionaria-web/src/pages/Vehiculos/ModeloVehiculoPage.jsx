import { useEffect, useState } from "react";
import { CarFront } from "lucide-react";
import PageHeader from "@/components/ui/custom/PageHeader";
import AddButton from "@/components/ui/custom/AddButton";
import { TooltipProvider } from "@/components/ui/tooltip";
import { ModeloVehiculoModal } from "@/components/modals/Vehiculo/ModeloVehiculoModal";
import ModeloVehiculoTable from "@/components/tables/Vehiculo/ModeloVehiculoTable";
import { useModelosVehiculos } from "@/hooks/Vehiculo/useModelosVehiculos";
import { useMarcas } from "@/hooks/Vehiculo/useMarcas";
import { useTiposVehiculos } from "@/hooks/Vehiculo/useTiposVehiculos";
import ModeloVehiculoService from "@/services/Vehiculo/modeloVehiculoService";
import { toastService } from "@/services/toastService";

export const ModeloVehiculoPage = () => {
  const [tipo, setTipo] = useState("activas");
  const [filtro, setFiltro] = useState("");
  const [debouncedFiltro, setDebouncedFiltro] = useState("");
  const [marcaVehiculoId, setMarcaVehiculoId] = useState("todos");
  const [tipoVehiculoId, setTipoVehiculoId] = useState("todos");
  const { data, loading, refetch } = useModelosVehiculos(
    tipo,
    debouncedFiltro,
    marcaVehiculoId,
    tipoVehiculoId,
  );
  const { data: modelosDisponibles, refetch: refetchModelosDisponibles } =
    useModelosVehiculos(tipo);
  const { data: marcas } = useMarcas("activas");
  const { data: tiposVehiculos } = useTiposVehiculos("activas");
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedModelo, setSelectedModelo] = useState(null);
  const [modalLoading, setModalLoading] = useState(false);
  const [serverError, setServerError] = useState("");
  useEffect(() => {
    const timeout = setTimeout(() => setDebouncedFiltro(filtro), 500);
    return () => clearTimeout(timeout);
  }, [filtro]);
  const handleSave = async (payload) => {
    setModalLoading(true);
    setServerError("");
    try {
      if (payload.modeloVehiculoId) {
        await ModeloVehiculoService.actualizar(
          payload.modeloVehiculoId,
          payload,
        );
        toastService.success("Éxito", {
          description: "Modelo actualizado correctamente",
        });
      } else {
        await ModeloVehiculoService.crear(payload);
        toastService.success("Éxito", {
          description: "Modelo creado correctamente",
        });
      }
      setIsModalOpen(false);
      refetch();
      refetchModelosDisponibles();
    } catch (error) {
      const dataError = error.response?.data;
      setServerError(
        dataError?.errors?.[0]?.errorMessage ||
          dataError?.message ||
          dataError?.mensaje ||
          "Ocurrió un error al guardar",
      );
    } finally {
      setModalLoading(false);
    }
  };
  const handleToggleStatus = async (modelo) => {
    try {
      if (tipo === "activas") {
        await ModeloVehiculoService.desactivar(modelo.modeloVehiculoId);
        toastService.success("Éxito", {
          description: "Modelo desactivado correctamente",
        });
      } else {
        await ModeloVehiculoService.activar(modelo.modeloVehiculoId);
        toastService.success("Éxito", {
          description: "Modelo activado correctamente",
        });
      }
      refetch();
      refetchModelosDisponibles();
    } catch (error) {
      const dataError = error.response?.data;
      toastService.error("Error", {
        description:
          dataError?.message ||
          dataError?.mensaje ||
          "No se pudo cambiar el estado del modelo",
      });
    }
  };
  const openModal = (modelo = null) => {
    setServerError("");
    setSelectedModelo(modelo);
    setIsModalOpen(true);
  };
  return (
    <TooltipProvider delayDuration={300}>
      <div>
        <PageHeader title="Modelos" icon={CarFront}>
          <AddButton onClick={() => openModal()}>Nuevo Modelo</AddButton>
        </PageHeader>
        {loading &&
        data.length === 0 &&
        !debouncedFiltro &&
        marcaVehiculoId === "todos" &&
        tipoVehiculoId === "todos" ? (
          <div className="flex h-64 items-center justify-center">
            Cargando...
          </div>
        ) : (
          <ModeloVehiculoTable
            data={data}
            modelosDisponibles={modelosDisponibles}
            tipo={tipo}
            onToggle={setTipo}
            onSearch={setFiltro}
            onEdit={openModal}
            onToggleStatus={handleToggleStatus}
            marcas={marcas}
            tiposVehiculos={tiposVehiculos}
            marcaVehiculoId={marcaVehiculoId}
            tipoVehiculoId={tipoVehiculoId}
            onMarcaChange={setMarcaVehiculoId}
            onTipoChange={setTipoVehiculoId}
          />
        )}
        {isModalOpen && (
          <ModeloVehiculoModal
            isOpen={isModalOpen}
            onClose={() => setIsModalOpen(false)}
            onSave={handleSave}
            modelo={selectedModelo}
            marcas={marcas}
            tiposVehiculos={tiposVehiculos}
            loading={modalLoading}
            serverError={serverError}
          />
        )}
      </div>
    </TooltipProvider>
  );
};
