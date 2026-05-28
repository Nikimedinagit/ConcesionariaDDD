import { useState } from "react";
import { Wallet } from "lucide-react";
import { useCategorias } from "@/hooks/useCategorias";
import CategoriaGastoService from "@/services/categoriaGastoService";
import PageHeader from "@/components/ui/custom/PageHeader";
import AddButton from "@/components/ui/custom/AddButton";
import CategoriaGastoTable from "@/components/tables/CategoriaGastoTable";
import { CategoriaGastoModal } from "@/components/modals/CategoriaGastoModal";
import { TooltipProvider } from "@/components/ui/tooltip";
import { toastService } from "@/services/toastService";

export const CategoriaGastoPage = () => {
  const [tipo, setTipo] = useState("activas");
  const { data, loading, refetch } = useCategorias(tipo);

  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedCategoria, setSelectedCategoria] = useState(null);
  const [modalLoading, setModalLoading] = useState(false);
  const [serverError, setServerError] = useState("");

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
        await CategoriaGastoService.actualizar(payload.categoriaGastoId, payload);
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

  return (
    <TooltipProvider delayDuration={300}>
      <div className="min-h-screen">
        <PageHeader title="Categorías de Gasto" icon={Wallet}>
          <AddButton onClick={handleOpenCreate}>Nueva Categoría</AddButton>
        </PageHeader>

        {loading ? (
          <div className="h-64 flex items-center justify-center">
            Cargando...
          </div>
        ) : (
          <CategoriaGastoTable
            data={data}
            tipo={tipo}
            onToggle={setTipo}
            onEdit={handleOpenEdit}
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
