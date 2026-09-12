import { useEffect, useState } from "react";
import { Users } from "lucide-react";
import { useUsuarios } from "@/hooks/Acceso/useUsuarios";
import UsuarioService from "@/services/Acceso/usuarioService";
import SucursalService from "@/services/Ubicacion/sucursalService";
import PageHeader from "@/components/ui/custom/PageHeader";
import AddButton from "@/components/ui/custom/AddButton";
import { UsuarioCards } from "@/components/cards/usuarios/UsuarioCards";
import { UsuarioModal } from "@/components/modals/Acceso/UsuarioModal";
import { UsuarioPasswordModal } from "@/components/modals/Acceso/UsuarioPasswordModal";
import { TooltipProvider } from "@/components/ui/tooltip";
import { toastService } from "@/services/toastService";

export const UsuarioPage = () => {
  const [tipo, setTipo] = useState("activas");
  const [filtro, setFiltro] = useState("");
  const [debouncedFiltro, setDebouncedFiltro] = useState("");
  const { data, loading, refetch } = useUsuarios(tipo, debouncedFiltro);
  const [roles, setRoles] = useState([]);
  const [sucursales, setSucursales] = useState([]);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [isPasswordModalOpen, setIsPasswordModalOpen] = useState(false);
  const [selectedUsuario, setSelectedUsuario] = useState(null);
  const [modalLoading, setModalLoading] = useState(false);
  const [passwordLoading, setPasswordLoading] = useState(false);
  const [serverError, setServerError] = useState("");
  const [serverFieldErrors, setServerFieldErrors] = useState({});
  const [passwordServerError, setPasswordServerError] = useState("");

  useEffect(() => {
    const handler = setTimeout(() => {
      setDebouncedFiltro(filtro);
    }, 500);

    return () => clearTimeout(handler);
  }, [filtro]);

  useEffect(() => {
    Promise.all([UsuarioService.getRoles(), SucursalService.getActivas()])
      .then(([rolesData, sucursalesData]) => {
        setRoles(rolesData);
        setSucursales(sucursalesData);
      })
      .catch(() => {
        toastService.error("Error", {
          description: "No se pudieron cargar roles o sucursales",
        });
      });
  }, []);

  const handleOpenCreate = () => {
    setServerError("");
    setServerFieldErrors({});
    setSelectedUsuario(null);
    setIsModalOpen(true);
  };

  const handleOpenEdit = (usuario) => {
    setServerError("");
    setServerFieldErrors({});
    setSelectedUsuario(usuario);
    setIsModalOpen(true);
  };

  const handleOpenPassword = (usuario) => {
    setPasswordServerError("");
    setSelectedUsuario(usuario);
    setIsPasswordModalOpen(true);
  };

  const getErrorMessage = (error, fallback) => {
    const data = error.response?.data;
    return data?.errors?.[0]?.errorMessage || data?.message || fallback;
  };

  const handleSave = async (payload) => {
    setModalLoading(true);
    setServerError("");
    setServerFieldErrors({});

    try {
      if (payload.usuarioId) {
        await UsuarioService.actualizar(payload.usuarioId, payload);
        toastService.success("Éxito", {
          description: "Usuario actualizado correctamente",
        });
      } else {
        await UsuarioService.crear(payload);
        toastService.success("Éxito", {
          description: "Usuario creado correctamente",
        });
      }

      setIsModalOpen(false);
      refetch();
    } catch (error) {
      const data = error.response?.data;
      const validationErrors = data?.errors || [];
      const fieldErrors = validationErrors.reduce((result, item) => {
        if (!item?.propertyName) return result;

        const fieldName =
          item.propertyName.charAt(0).toLowerCase() + item.propertyName.slice(1);
        result[fieldName] = item.errorMessage;
        return result;
      }, {});
      const message = getErrorMessage(error, "Ocurrió un error al guardar");

      if (!fieldErrors.email && /email/i.test(message)) {
        fieldErrors.email = message;
      }

      setServerFieldErrors(fieldErrors);
      setServerError(Object.keys(fieldErrors).length > 0 ? "" : message);
    } finally {
      setModalLoading(false);
    }
  };

  const handleChangePassword = async (payload) => {
    setPasswordLoading(true);
    setPasswordServerError("");

    try {
      await UsuarioService.cambiarPassword(payload.usuarioId, payload);
      toastService.success("Éxito", {
        description: "Contraseña actualizada correctamente",
      });
      setIsPasswordModalOpen(false);
    } catch (error) {
      setPasswordServerError(
        getErrorMessage(error, "Ocurrió un error al actualizar la contraseña"),
      );
    } finally {
      setPasswordLoading(false);
    }
  };

  const handleToggleStatus = async (usuario) => {
    try {
      if (tipo === "activas") {
        await UsuarioService.desactivar(usuario.usuarioId);
        toastService.success("Éxito", {
          description: "Usuario desactivado correctamente",
        });
      } else {
        await UsuarioService.activar(usuario.usuarioId);
        toastService.success("Éxito", {
          description: "Usuario activado correctamente",
        });
      }

      refetch();
    } catch {
      toastService.error("Error", {
        description: "No se pudo cambiar el estado del usuario",
      });
    }
  };

  return (
    <TooltipProvider delayDuration={300}>
      <div>
        <PageHeader title="Usuarios" icon={Users}>
          <AddButton onClick={handleOpenCreate}>Nuevo Usuario</AddButton>
        </PageHeader>

        {loading && data.length === 0 ? (
          <div className="flex h-64 items-center justify-center">
            Cargando...
          </div>
        ) : (
          <UsuarioCards
            data={data}
            tipo={tipo}
            onToggle={setTipo}
            onSearch={setFiltro}
            onEdit={handleOpenEdit}
            onChangePassword={handleOpenPassword}
            onToggleStatus={handleToggleStatus}
          />
        )}

        <UsuarioModal
          isOpen={isModalOpen}
          onClose={() => setIsModalOpen(false)}
          onSave={handleSave}
          usuario={selectedUsuario}
          roles={roles}
          sucursales={sucursales}
          loading={modalLoading}
          serverError={serverError}
          serverFieldErrors={serverFieldErrors}
        />

        <UsuarioPasswordModal
          isOpen={isPasswordModalOpen}
          onClose={() => setIsPasswordModalOpen(false)}
          onSave={handleChangePassword}
          usuario={selectedUsuario}
          loading={passwordLoading}
          serverError={passwordServerError}
        />
      </div>
    </TooltipProvider>
  );
};
