import { useEffect, useState } from "react";
import { ContactRound } from "lucide-react";
import PageHeader from "@/components/ui/custom/PageHeader";
import AddButton from "@/components/ui/custom/AddButton";
import { TooltipProvider } from "@/components/ui/tooltip";
import ClienteTable from "@/components/tables/Persona/ClienteTable";
import { ClienteModal } from "@/components/modals/Persona/ClienteModal";
import { useClientes } from "@/hooks/Persona/useClientes";
import ClienteService from "@/services/Persona/clienteService";
import { getLocalidades } from "@/services/Ubicacion/localidadService";
import { toastService } from "@/services/toastService";
import { usePermissions } from "@/context/PermissionContext";

export const ClientePage = () => {
  const { can } = usePermissions();
  const [tipo, setTipo] = useState("activas");
  const [filtro, setFiltro] = useState("");
  const [debouncedFiltro, setDebouncedFiltro] = useState("");
  const { data, loading, refetch } = useClientes(tipo, debouncedFiltro);
  const [localidades, setLocalidades] = useState([]);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedCliente, setSelectedCliente] = useState(null);
  const [modalLoading, setModalLoading] = useState(false);
  const [serverError, setServerError] = useState("");
  const [serverFieldErrors, setServerFieldErrors] = useState({});

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

  const openModal = (cliente = null) => {
    setServerError("");
    setServerFieldErrors({});
    setSelectedCliente(cliente);
    setIsModalOpen(true);
  };

  const handleSave = async (payload) => {
    setModalLoading(true);
    setServerError("");
    setServerFieldErrors({});
    try {
      if (payload.clienteId) {
        await ClienteService.actualizar(payload.clienteId, payload);
        toastService.success("Éxito", {
          description: "Cliente actualizado correctamente",
        });
      } else {
        await ClienteService.crear(payload);
        toastService.success("Éxito", {
          description: "Cliente registrado correctamente",
        });
      }
      setIsModalOpen(false);
      refetch();
    } catch (error) {
      const dataError = error.response?.data;
      const validationErrors = dataError?.errors || [];
      const fieldErrors = validationErrors.reduce((result, item) => {
        const message = item?.errorMessage;
        let fieldName = item?.propertyName
          ? item.propertyName.charAt(0).toLowerCase() +
            item.propertyName.slice(1)
          : "";

        if (!fieldName && /dni/i.test(message || "")) fieldName = "dni";
        if (!fieldName && /email/i.test(message || "")) fieldName = "email";
        if (fieldName && message) result[fieldName] = message;
        return result;
      }, {});

      setServerFieldErrors(fieldErrors);
      setServerError(
        Object.keys(fieldErrors).length > 0
          ? ""
          : validationErrors[0]?.errorMessage ||
            dataError?.message ||
            dataError?.mensaje ||
            "Ocurrió un error al guardar",
      );
    } finally {
      setModalLoading(false);
    }
  };

  const handleToggleStatus = async (cliente) => {
    try {
      if (tipo === "activas") {
        await ClienteService.desactivar(cliente.clienteId);
        toastService.success("Éxito", {
          description: "Cliente desactivado correctamente",
        });
      } else {
        await ClienteService.activar(cliente.clienteId);
        toastService.success("Éxito", {
          description: "Cliente activado correctamente",
        });
      }
      refetch();
    } catch (error) {
      const dataError = error.response?.data;
      toastService.error("Error", {
        description:
          dataError?.message ||
          dataError?.mensaje ||
          "No se pudo cambiar el estado del cliente",
      });
    }
  };

  return (
    <TooltipProvider delayDuration={300}>
      <div>
        <PageHeader title="Clientes" icon={ContactRound}>
          {can("CLIENTES_CREAR") && (
            <AddButton onClick={() => openModal()}>Nuevo Cliente</AddButton>
          )}
        </PageHeader>
        {loading && data.length === 0 && !debouncedFiltro ? (
          <div className="flex h-64 items-center justify-center">
            Cargando...
          </div>
        ) : (
          <ClienteTable
            data={data}
            tipo={tipo}
            onToggle={setTipo}
            onSearch={setFiltro}
            onEdit={openModal}
            onToggleStatus={handleToggleStatus}
            localidades={localidades}
            canEdit={can("CLIENTES_EDITAR")}
            canActivate={can("CLIENTES_ACTIVAR")}
            canDeactivate={can("CLIENTES_DESACTIVAR")}
          />
        )}
        {isModalOpen && (
          <ClienteModal
            isOpen={isModalOpen}
            onClose={() => setIsModalOpen(false)}
            onSave={handleSave}
            cliente={selectedCliente}
            localidades={localidades}
            loading={modalLoading}
            serverError={serverError}
            serverFieldErrors={serverFieldErrors}
          />
        )}
      </div>
    </TooltipProvider>
  );
};
