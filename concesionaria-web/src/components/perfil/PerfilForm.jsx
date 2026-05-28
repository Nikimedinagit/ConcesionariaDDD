import { useState, useEffect } from "react";

import { usePerfil } from "@/hooks/usePerfil";

import { getLocalidades } from "@/services/localidadService";

import { usuarioService } from "@/services/perfilService";

import { PerfilEmpresaSection } from "./EmpresaForm";

import { PerfilUsuarioSection } from "./UsuarioForm";

import { PerfilSeguridadSection } from "./SeguridadForm";

import { PerfilAvatarSection } from "./AvatarForm";

import { RecuperacionForm } from "./RecuperacionForm";

import { toastService } from "@/services/toastService";

import { useAuth } from "@/context/AuthContext";

import {
  passwordSchema,
  nombreFantasiaSchema,
  nombreCompletoSchema,
  telefonoSchema,
} from "@/validations/perfil.validation";

export function PerfilForm() {
  const { updateUserData } = useAuth();

  const { perfil, loading } = usePerfil();

  const [localidades, setLocalidades] = useState([]);

  const [savingEmpresa, setSavingEmpresa] = useState(false);

  const [savingUsuario, setSavingUsuario] = useState(false);

  const [savingPassword, setSavingPassword] = useState(false);

  const [passwordErrors, setPasswordErrors] = useState({});

  const [empresaErrors, setEmpresaErrors] = useState({});

  const [usuarioErrors, setUsuarioErrors] = useState({});

  const [telefonoErrors, setTelefonoErrors] = useState({});

  const [form, setForm] = useState({
    razonSocial: "",

    nombreFantasia: "",

    cuit: "",

    localidadId: "",

    moneda: "ARG",

    estado: "Activo",

    nombreCompleto: "",

    email: "",

    codigoPais: "+54",

    codigoArea: "",

    telefono: "",

    avatar: "",

    passwordActual: "",

    passwordNueva: "",
  });

  useEffect(() => {
    getLocalidades().then((data) => setLocalidades(data));
  }, []);

  useEffect(() => {
    if (perfil) {
      const telefonoCompleto = perfil.telefono || "";

      const partes = telefonoCompleto.trim().split(" ");

      const codigoPais = partes[0] || "+54";

      const codigoArea = partes[1] || "";

      const telefono = partes.slice(2).join(" ") || "";

      setForm((prev) => ({
        ...prev,

        empresaId: perfil.id || perfil.empresaId || perfil.EmpresaId,

        razonSocial: perfil.razonSocial || "",

        nombreFantasia: perfil.nombreFantasia || "",

        cuit: perfil.cuit || "",

        localidadId: perfil.localidadId || "",

        moneda: perfil.moneda || "ARG",

        estado: perfil.estado || "Activo",

        nombreCompleto: perfil.nombreCompleto || "",

        email: perfil.email || "",

        codigoPais,

        codigoArea,

        telefono,

        avatar: perfil.avatarUrl || "",
      }));
    }
  }, [perfil]);

  const updateField = (field, value) => {
    setForm((prev) => ({
      ...prev,

      [field]: value,
    }));
  };

  const buildTelefono = () => {
    return `${form.codigoPais} ${form.codigoArea} ${form.telefono}`;
  };

  const handleGuardarEmpresa = async () => {
    const result = nombreFantasiaSchema.safeParse({
      nombreFantasia: form.nombreFantasia,
    });

    if (!result.success) {
      setEmpresaErrors(result.error.flatten().fieldErrors);

      return;
    }

    try {
      setEmpresaErrors({});

      setSavingEmpresa(true);

      const payload = {
        empresaId: form.empresaId,

        nombreFantasia: form.nombreFantasia,

        localidadId: form.localidadId,

        moneda: form.moneda,
      };

      await usuarioService.updateEmpresa(payload);

      toastService.success("¡Éxito!", {
        description: "La empresa ha sido actualizada correctamente.",
      });
    } catch (error) {
      console.error(error);

      toastService.error("Error al guardar empresa", {
        description: "Ocurrió un problema, intenta nuevamente.",
      });
    } finally {
      setSavingEmpresa(false);
    }
  };

  const handleGuardarUsuario = async () => {
    const result = nombreCompletoSchema.safeParse({
      nombreCompleto: form.nombreCompleto,
    });

    if (!result.success) {
      setUsuarioErrors(result.error.flatten().fieldErrors);

      return;
    }

    try {
      setUsuarioErrors({});

      setSavingUsuario(true);

      const telefonoCompleto = buildTelefono();

      const payload = {
        nombreCompleto: form.nombreCompleto,

        telefono: telefonoCompleto,

        avatarUrl: form.avatar,
      };

      const response = await usuarioService.updateUsuario(payload);

      localStorage.setItem("token", response.token);

      updateUserData();

      toastService.success("¡Éxito!", {
        description: "El usuario ha sido actualizado correctamente.",
      });
    } catch (error) {
      console.error(error);

      toastService.error("Error al guardar usuario", {
        description: "Ocurrió un problema, intenta nuevamente.",
      });
    } finally {
      setSavingUsuario(false);
    }
  };

  const handleActualizarAvatar = async (nuevoAvatar) => {
    try {
      const telefonoCompleto = buildTelefono();

      const payload = {
        nombreCompleto: form.nombreCompleto,

        telefono: telefonoCompleto,

        avatarUrl: nuevoAvatar,
      };

      const response = await usuarioService.updateAvatar(payload);

      localStorage.setItem("token", response.token);

      updateUserData();

      toastService.success("¡Éxito!", {
        description: "El avatar ha sido actualizado correctamente.",
      });
    } catch (error) {
      console.error(error);

      toastService.error("Error al actualizar avatar", {
        description: "Ocurrió un problema, intenta nuevamente.",
      });
    }
  };

  const handleActualizarRecuperacion = async () => {
    const result = telefonoSchema.safeParse({
      codigoPais: form.codigoPais,
      codigoArea: form.codigoArea,
      telefono: form.telefono,
    });

    if (!result.success) {
      setTelefonoErrors(result.error.flatten().fieldErrors);

      return;
    }

    try {
      setTelefonoErrors({});

      setSavingUsuario(true);

      const telefonoCompleto = buildTelefono();

      const payload = {
        nombreCompleto: form.nombreCompleto,
        telefono: telefonoCompleto,
        avatarUrl: form.avatar,
      };

      await usuarioService.updateRecuperacion(payload);

      toastService.success("¡Éxito!", {
        description: "La recuperación ha sido actualizada correctamente.",
      });
    } catch (error) {
      console.error(error);

      toastService.error("Error al actualizar recuperación", {
        description: "Ocurrió un problema, intenta nuevamente.",
      });
    } finally {
      setSavingUsuario(false);
    }
  };

  const handleCambiarPassword = async () => {
    const result = passwordSchema.safeParse({
      passwordActual: form.passwordActual,

      passwordNueva: form.passwordNueva,
    });

    if (!result.success) {
      setPasswordErrors(result.error.flatten().fieldErrors);

      return;
    }

    try {
      setPasswordErrors({});

      setSavingPassword(true);

      await usuarioService.updateContraseña({
        passwordActual: form.passwordActual,

        passwordNueva: form.passwordNueva,
      });

      toastService.success("¡Éxito!", {
        description: "La contraseña ha sido actualizada correctamente.",
      });

      updateField("passwordActual", "");

      updateField("passwordNueva", "");
    } catch (error) {
      if (error.response?.data?.message === "Contraseña incorrecta.") {
        setPasswordErrors({
          passwordActual: ["Contraseña incorrecta."],
        });
      } else {
        toastService.error("Error al actualizar contraseña", {
          description: "Ocurrió un problema, intenta nuevamente.",
        });
      }
    } finally {
      setSavingPassword(false);
    }
  };

  if (loading) {
    return (
      <div className="p-8 text-center text-slate-500">Cargando perfil...</div>
    );
  }

  return (
    <div className="w-full mx-auto max-w-[1400px] py-0 space-y-5">
      <PerfilEmpresaSection
        form={form}
        updateField={updateField}
        localidades={localidades}
        onSave={handleGuardarEmpresa}
        saving={savingEmpresa}
        errors={empresaErrors}
      />

      <div className="grid grid-cols-1 lg:grid-cols-2 gap-5">
        <PerfilUsuarioSection
          form={form}
          updateField={updateField}
          onSave={handleGuardarUsuario}
          saving={savingUsuario}
          errors={usuarioErrors}
        />

        <PerfilSeguridadSection
          form={form}
          updateField={updateField}
          onSave={handleCambiarPassword}
          saving={savingPassword}
          errors={passwordErrors}
        />
      </div>

      <RecuperacionForm
        form={form}
        updateField={updateField}
        onSave={handleActualizarRecuperacion}
        saving={savingUsuario}
        errors={telefonoErrors}
      />

      <PerfilAvatarSection
        form={form}
        updateField={updateField}
        onSave={handleActualizarAvatar}
      />
    </div>
  );
}
