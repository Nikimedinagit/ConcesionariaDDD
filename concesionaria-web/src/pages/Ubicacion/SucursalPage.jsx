import { useEffect, useState } from "react";
import { Building2 } from "lucide-react";
import { useSucursales } from "@/hooks/useSucursales";
import SucursalService from "@/services/Ubicacion/sucursalService";
import { getLocalidades } from "@/services/Ubicacion/localidadService";
import PageHeader from "@/components/ui/custom/PageHeader";
import AddButton from "@/components/ui/custom/AddButton";
import SucursalTable from "@/components/tables/SucursalTable";
import { SucursalModal } from "@/components/modals/SucursalModal";
import { TooltipProvider } from "@/components/ui/tooltip";
import { toastService } from "@/services/toastService";

export const SucursalPage = () => {
  const [tipo, setTipo] = useState("activas");
  const [filtro, setFiltro] = useState("");
  const [debouncedFiltro, setDebouncedFiltro] = useState("");
  const { data, loading, refetch } = useSucursales(tipo, debouncedFiltro);
  const [localidades, setLocalidades] = useState([]);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedSucursal, setSelectedSucursal] = useState(null);
  const [modalLoading, setModalLoading] = useState(false);
  const [serverError, setServerError] = useState("");

  useEffect(() => {
    const handler = setTimeout(() => {
      setDebouncedFiltro(filtro);
    }, 500);

    return () => clearTimeout(handler);
  }, [filtro]);

  useEffect(() => {
    getLocalidades()
      .then((data) => setLocalidades(data))
      .catch(() => {
        toastService.error("Error", {
          description: "No se pudieron cargar las localidades",
        });
      });
  }, []);

  const handleOpenCreate = () => {
    setServerError("");
    setSelectedSucursal(null);
    setIsModalOpen(true);
  };

  const handleOpenEdit = (sucursal) => {
    setServerError("");
    setSelectedSucursal(sucursal);
    setIsModalOpen(true);
  };

  const handleSave = async (payload) => {
    setModalLoading(true);
    setServerError("");

    try {
      if (payload.sucursalId) {
        await SucursalService.actualizar(payload.sucursalId, payload);
        toastService.success("Éxito", {
          description: "Sucursal actualizada correctamente",
        });
      } else {
        await SucursalService.crear(payload);
        toastService.success("Éxito", {
          description: "Sucursal creada correctamente",
        });
      }

      setIsModalOpen(false);
      refetch();
    } catch (error) {
      const data = error.response?.data;
      const mensajeError =
        data?.errors?.[0]?.errorMessage ||
        data?.message ||
        "Ocurrió un error al guardar";
      setServerError(mensajeError);
    } finally {
      setModalLoading(false);
    }
  };

  const handleToggleStatus = async (sucursal) => {
    try {
      if (tipo === "activas") {
        await SucursalService.desactivar(sucursal.sucursalId);
        toastService.success("Éxito", {
          description: "Sucursal desactivada correctamente",
        });
      } else {
        await SucursalService.activar(sucursal.sucursalId);
        toastService.success("Éxito", {
          description: "Sucursal activada correctamente",
        });
      }

      refetch();
    } catch {
      toastService.error("Error", {
        description: "No se pudo cambiar el estado de la sucursal",
      });
    }
  };

  return (
    <TooltipProvider delayDuration={300}>
      <div>
        <PageHeader title="Sucursales" icon={Building2}>
          <AddButton onClick={handleOpenCreate}>Nueva Sucursal</AddButton>
        </PageHeader>

        {loading && data.length === 0 ? (
          <div className="flex h-64 items-center justify-center">
            Cargando...
          </div>
        ) : (
          <SucursalTable
            data={data}
            tipo={tipo}
            onToggle={setTipo}
            onSearch={setFiltro}
            onEdit={handleOpenEdit}
            onToggleStatus={handleToggleStatus}
            localidades={localidades}
          />
        )}

        <SucursalModal
          isOpen={isModalOpen}
          onClose={() => setIsModalOpen(false)}
          onSave={handleSave}
          sucursal={selectedSucursal}
          localidades={localidades}
          loading={modalLoading}
          serverError={serverError}
        />
      </div>
    </TooltipProvider>
  );
};
