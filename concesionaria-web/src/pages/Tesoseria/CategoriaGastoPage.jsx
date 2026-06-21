import { useState, useEffect } from "react"; // 1. Agregamos useEffect
import { Wallet } from "lucide-react";
import { useCategorias } from "@/hooks/useCategorias";
import CategoriaGastoService from "@/services/Tesoreria/categoriaGastoService";
import PageHeader from "@/components/ui/custom/PageHeader";
import AddButton from "@/components/ui/custom/AddButton";
import CategoriaGastoTable from "@/components/tables/CategoriaGastoTable";
import { CategoriaGastoModal } from "@/components/modals/CategoriaGastoModal";
import { TooltipProvider } from "@/components/ui/tooltip";
import { toastService } from "@/services/toastService";

export const CategoriaGastoPage = () => {
  const [tipo, setTipo] = useState("activas");
  const [filtro, setFiltro] = useState("");
  const [debouncedFiltro, setDebouncedFiltro] = useState("");
  const { data, loading, refetch } = useCategorias(tipo, debouncedFiltro);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedCategoria, setSelectedCategoria] = useState(null);
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
    setSelectedCategoria(null);
    setIsModalOpen(true);
  };

  const handleOpenEdit = (categoria) => {
    setServerError("");
    setSelectedCategoria(categoria);
    setIsModalOpen(true);
  };

  const handleSave = async (payload) => {
    setModalLoading(true);
    setServerError("");

    try {
      if (payload.categoriaGastoId) {
        await CategoriaGastoService.actualizar(
          payload.categoriaGastoId,
          payload,
        );
        toastService.success("Éxito", {
          description: "Categoría actualizada correctamente",
        });
      } else {
        await CategoriaGastoService.crear(payload);
        toastService.success("Éxito", {
          description: "Categoría creada correctamente",
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

  const handleToggleStatus = async (categoria) => {
  try {
    if (tipo === "activas") {
      await CategoriaGastoService.desactivar(categoria.categoriaGastoId);
      toastService.success("Éxito", { description: "Categoría desactivada correctamente" });
    } else {
      await CategoriaGastoService.activar(categoria.categoriaGastoId);
      toastService.success("Éxito", { description: "Categoría activada correctamente" });
    }
    refetch(); 
  } catch {
    toastService.error("Error", { 
      description: "No se pudo cambiar el estado de la categoría" 
    });
  }
};


  return (
    <TooltipProvider delayDuration={300}>
      <div>
        <PageHeader title="Categorías de Gastos" icon={Wallet}>
          <AddButton onClick={handleOpenCreate}>Nueva Categoría</AddButton>
        </PageHeader>

        {loading && data.length === 0 ? (
          <div className="h-64 flex items-center justify-center">
            Cargando...
          </div>
        ) : (
          <CategoriaGastoTable
            data={data}
            tipo={tipo}
            onToggle={setTipo}
            onSearch={setFiltro}
            onEdit={handleOpenEdit}
            onToggleStatus={handleToggleStatus}
          />
        )}

        <CategoriaGastoModal
          isOpen={isModalOpen}
          onClose={() => setIsModalOpen(false)}
          onSave={handleSave}
          categoria={selectedCategoria}
          loading={modalLoading}
          serverError={serverError}
        />
      </div>
    </TooltipProvider>
  );
};
