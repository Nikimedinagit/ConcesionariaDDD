import { useEffect, useState } from "react";
import { Tags } from "lucide-react";
import PageHeader from "@/components/ui/custom/PageHeader";
import AddButton from "@/components/ui/custom/AddButton";
import { MarcaModal } from "@/components/modals/Vehiculo/MarcaModal";
import MarcaTable from "@/components/tables/Vehiculo/MarcaTable";
import { TooltipProvider } from "@/components/ui/tooltip";
import { useMarcas } from "@/hooks/Vehiculo/useMarcas";
import MarcaService from "@/services/Vehiculo/marcaService";
import { toastService } from "@/services/toastService";

export const MarcaPage = () => {
  const [tipo, setTipo] = useState("activas");
  const [filtro, setFiltro] = useState("");
  const [debouncedFiltro, setDebouncedFiltro] = useState("");
  const { data, loading, refetch } = useMarcas(tipo, debouncedFiltro);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedMarca, setSelectedMarca] = useState(null);
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
    setSelectedMarca(null);
    setIsModalOpen(true);
  };

  const handleOpenEdit = (marca) => {
    setServerError("");
    setSelectedMarca(marca);
    setIsModalOpen(true);
  };

  const handleSave = async (payload) => {
    setModalLoading(true);
    setServerError("");

    try {
      if (payload.marcaVehiculoId) {
        await MarcaService.actualizar(payload.marcaVehiculoId, payload);
        toastService.success("Éxito", {
          description: "Marca actualizada correctamente",
        });
      } else {
        await MarcaService.crear({ nombre: payload.nombre });
        toastService.success("Éxito", {
          description: "Marca creada correctamente",
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

  const handleToggleStatus = async (marca) => {
    try {
      if (tipo === "activas") {
        await MarcaService.desactivar(marca.marcaVehiculoId);
        toastService.success("Éxito", {
          description: "Marca desactivada correctamente",
        });
      } else {
        await MarcaService.activar(marca.marcaVehiculoId);
        toastService.success("Éxito", {
          description: "Marca activada correctamente",
        });
      }

      refetch();
    } catch (error) {
      const dataError = error.response?.data;
      const mensajeError =
        dataError?.errors?.[0]?.errorMessage ||
        dataError?.message ||
        dataError?.mensaje ||
        "No se pudo cambiar el estado de la marca";

      toastService.error("No se puede desactivar", {
        description: mensajeError,
      });
    }
  };

  return (
    <TooltipProvider delayDuration={300}>
      <div>
        <PageHeader title="Marcas" icon={Tags}>
          <AddButton onClick={handleOpenCreate}>Nueva Marca</AddButton>
        </PageHeader>

        {loading && data.length === 0 && !debouncedFiltro ? (
          <div className="flex h-64 items-center justify-center">
            Cargando...
          </div>
        ) : (
          <MarcaTable
            data={data}
            tipo={tipo}
            onToggle={setTipo}
            onSearch={setFiltro}
            onEdit={handleOpenEdit}
            onToggleStatus={handleToggleStatus}
          />
        )}

        {isModalOpen && (
          <MarcaModal
            isOpen={isModalOpen}
            onClose={() => setIsModalOpen(false)}
            onSave={handleSave}
            marca={selectedMarca}
            loading={modalLoading}
            serverError={serverError}
          />
        )}
      </div>
    </TooltipProvider>
  );
};
