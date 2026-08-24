import { useEffect, useState } from "react";
import { ContactRound } from "lucide-react";
import PageHeader from "@/components/ui/custom/PageHeader";
import AddButton from "@/components/ui/custom/AddButton";
import { TooltipProvider } from "@/components/ui/tooltip";
import ProveedorTable from "@/components/tables/Persona/ProveedorTable";
import { ProveedorModal } from "@/components/modals/Persona/ProveedorModal";
import { useProveedor } from "@/hooks/Persona/useProveedor";
import ProveedorService from "@/services/Persona/proveedorService";
import { getLocalidades } from "@/services/Ubicacion/localidadService";
import { toastService } from "@/services/toastService";

export const ProveedorPage = () => {
  const [tipo, setTipo] = useState("activas");
  const [filtro, setFiltro] = useState("");
  const [debouncedFiltro, setDebouncedFiltro] = useState("");
  const { data, loading, refetch } = useProveedor(tipo, debouncedFiltro);
  const [localidades, setLocalidades] = useState([]);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedProveedor, setSelectedProveedor] = useState(null);
  const [modalLoading, setModalLoading] = useState(false);
  const [serverError, setServerError] = useState("");

  useEffect(() => {
    const handler = setTimeout(() => setDebouncedFiltro(filtro), 500);
    return () => clearTimeout(handler);
  }, [filtro]);
  useEffect(() => {
    getLocalidades()
      .then(setLocalidades)
      .catch(() =>
        toastService.error("Error", {
          description: "No se pudieron cargar las localidades",
        }),
      );
  }, []);

  const openModal = (proveedor = null) => {
    setServerError("");
    setSelectedProveedor(proveedor);
    setIsModalOpen(true);
  };

  const handleSave = async (payload) => {
    setModalLoading(true);
    setServerError("");
    try {
      if (payload.proveedorId) {
        await ProveedorService.actualizar(payload.proveedorId, payload);
        toastService.success("Éxito", {
          description: "Proveedor actualizado correctamente",
        });
      } else {
        await ProveedorService.crear(payload);
        toastService.success("Éxito", {
          description: "Proveedor registrado correctamente",
        });
      }
      setIsModalOpen(false);
      refetch();
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

  const handleToggleStatus = async (proveedor) => {
    try {
      if (tipo === "activas") {
        await ProveedorService.desactivar(proveedor.proveedorId);
        toastService.success("Éxito", {
          description: "Proveedor desactivado correctamente",
        });
      } else {
        await ProveedorService.activar(proveedor.proveedorId);
        toastService.success("Éxito", {
          description: "Proveedor activado correctamente",
        });
      }
      refetch();
    } catch (error) {
      const dataError = error.response?.data;
      toastService.error("Error", {
        description:
          dataError?.message ||
          dataError?.mensaje ||
          "No se pudo cambiar el estado del proveedor",
      });
    }
  };

  return (
    <TooltipProvider delayDuration={300}>
      <div>
        <PageHeader title="Proveedores" icon={ContactRound}>
          <AddButton onClick={() => openModal()}>Nuevo Proveedor</AddButton>
        </PageHeader>
        {loading && data.length === 0 && !debouncedFiltro ? (
          <div className="flex h-64 items-center justify-center">
            Cargando...
          </div>
        ) : (
          <ProveedorTable
            data={data}
            tipo={tipo}
            onToggle={setTipo}
            onSearch={setFiltro}
            onEdit={openModal}
            onToggleStatus={handleToggleStatus}
            localidades={localidades}
          />
        )}
        {isModalOpen && (
          <ProveedorModal
            isOpen={isModalOpen}
            onClose={() => setIsModalOpen(false)}
            onSave={handleSave}
            proveedor={selectedProveedor}
            localidades={localidades}
            loading={modalLoading}
            serverError={serverError}
          />
        )}
      </div>
    </TooltipProvider>
  );
};
