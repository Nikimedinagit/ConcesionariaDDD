import { useEffect, useState } from "react";
import { Landmark } from "lucide-react";
import PageHeader from "@/components/ui/custom/PageHeader";
import AddButton from "@/components/ui/custom/AddButton";
import CuentaTable from "@/components/tables/CuentaTable";
import { CuentaModal } from "@/components/modals/CuentaModal";
import { TooltipProvider } from "@/components/ui/tooltip";
import { toastService } from "@/services/toastService";
import CuentaService from "@/services/Contabilidad/cuentaService";
import { useCuentas } from "@/hooks/useCuentas";

const generateChildCode = (cuentas, cuentaPadre) => {
  const childrenCount = cuentas.filter(
    (cuenta) => cuenta.cuentaPadreId === cuentaPadre.cuentaId,
  ).length;

  return `${cuentaPadre.codigo}.${childrenCount + 1}`;
};

const generateRootCode = (cuentas) => {
  const maxRootCode = cuentas
    .filter((cuenta) => cuenta.nivel === 0 && !cuenta.codigo.includes("."))
    .reduce((max, cuenta) => {
      const value = Number(cuenta.codigo);
      return Number.isNaN(value) ? max : Math.max(max, value);
    }, 0);

  return String(maxRootCode + 1);
};

export const CuentaPage = () => {
  const [tipo, setTipo] = useState("activas");
  const [filtro, setFiltro] = useState("");
  const [debouncedFiltro, setDebouncedFiltro] = useState("");
  const [tipoCuenta, setTipoCuenta] = useState("todos");
  const [nivel, setNivel] = useState("todos");
  const { data: cuentas, loading, refetch } = useCuentas(
    tipo,
    debouncedFiltro,
    tipoCuenta,
    nivel,
  );
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedCuenta, setSelectedCuenta] = useState(null);
  const [selectedPadre, setSelectedPadre] = useState(null);
  const [modalLoading, setModalLoading] = useState(false);
  const [serverError, setServerError] = useState("");

  useEffect(() => {
    const handler = setTimeout(() => {
      setDebouncedFiltro(filtro);
    }, 500);

    return () => clearTimeout(handler);
  }, [filtro]);

  const handleOpenRootCreate = () => {
    setServerError("");
    setSelectedCuenta(null);
    setSelectedPadre(null);
    setIsModalOpen(true);
  };

  const handleOpenChildCreate = (cuentaPadre) => {
    setServerError("");
    setSelectedCuenta(null);
    setSelectedPadre(cuentaPadre);
    setIsModalOpen(true);
  };

  const handleOpenEdit = (cuenta) => {
    setServerError("");
    setSelectedCuenta(cuenta);
    setSelectedPadre(
      cuentas.find((item) => item.cuentaId === cuenta.cuentaPadreId) ?? null,
    );
    setIsModalOpen(true);
  };

  const handleSave = async ({ nombre, tipo: tipoCuenta }) => {
    setModalLoading(true);
    setServerError("");

    try {
      if (selectedCuenta) {
        await CuentaService.actualizar(selectedCuenta.cuentaId, {
          cuentaId: selectedCuenta.cuentaId,
          nombre,
        });

        toastService.success("Éxito", {
          description: "Cuenta actualizada correctamente",
        });
      } else {
        const codigo = selectedPadre
          ? generateChildCode(cuentas, selectedPadre)
          : generateRootCode(cuentas);

        await CuentaService.crear({
          nombre,
          codigo,
          nivel: selectedPadre ? selectedPadre.nivel + 1 : 0,
          tipo: selectedPadre ? selectedPadre.tipo : tipoCuenta,
          cuentaPadreId: selectedPadre?.cuentaId ?? null,
        });

        toastService.success("Éxito", {
          description: "Cuenta agregada correctamente",
        });
      }

      setIsModalOpen(false);
      refetch();
    } catch (error) {
      const data = error.response?.data;
      const mensajeError =
        data?.errors?.[0]?.errorMessage ||
        data?.message ||
        data?.mensaje ||
        "Ocurrió un error al guardar";
      setServerError(mensajeError);
    } finally {
      setModalLoading(false);
    }
  };

  const handleToggleStatus = async (cuenta) => {
    try {
      if (tipo === "activas") {
        await CuentaService.desactivar(cuenta.cuentaId);
        toastService.success("Éxito", {
          description: "Cuenta desactivada correctamente",
        });
      } else {
        await CuentaService.activar(cuenta.cuentaId);
        toastService.success("Éxito", {
          description: "Cuenta activada correctamente",
        });
      }

      refetch();
    } catch (error) {
      const mensajeError =
        error.response?.data?.message ||
        error.response?.data?.mensaje ||
        "No se pudo cambiar el estado de la cuenta";

      toastService.error("Error", {
        description: mensajeError,
      });
    }
  };

  return (
    <TooltipProvider delayDuration={300}>
      <div>
        <PageHeader title="Cuentas" icon={Landmark}>
          <AddButton onClick={handleOpenRootCreate}>Nueva Cuenta</AddButton>
        </PageHeader>

        {loading &&
        cuentas.length === 0 &&
        !debouncedFiltro &&
        tipoCuenta === "todos" &&
        nivel === "todos" ? (
          <div className="flex h-64 items-center justify-center">
            Cargando...
          </div>
        ) : (
          <CuentaTable
            data={cuentas}
            tipo={tipo}
            onToggle={setTipo}
            onSearch={setFiltro}
            tipoCuenta={tipoCuenta}
            nivel={nivel}
            onTipoCuentaChange={setTipoCuenta}
            onNivelChange={setNivel}
            onAddChild={handleOpenChildCreate}
            onEdit={handleOpenEdit}
            onToggleStatus={handleToggleStatus}
          />
        )}

        {isModalOpen && (
          <CuentaModal
            isOpen={isModalOpen}
            onClose={() => setIsModalOpen(false)}
            onSave={handleSave}
            cuenta={selectedCuenta}
            cuentaPadre={selectedCuenta ? null : selectedPadre}
            loading={modalLoading}
            serverError={serverError}
          />
        )}
      </div>
    </TooltipProvider>
  );
};
