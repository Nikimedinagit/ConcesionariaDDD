import { useState, useEffect } from "react";

import { usePerfil } from "@/hooks/usePerfil";

import { getLocalidades } from "@/services/localidadService";

import { empresaService } from "@/services/empresaService";

import { usuarioService } from "@/services/perfilService";

import { PerfilEmpresaSection } from "./EmpresaForm";

import { PerfilUsuarioSection } from "./UsuarioForm";

import { PerfilSeguridadSection } from "./SeguridadForm";

import { PerfilAvatarSection } from "./AvatarForm";

import { toastService } from "@/services/toastService";

export function PerfilForm() {
  const { perfil, loading } = usePerfil();

  const [localidades, setLocalidades] = useState([]);

  const [savingEmpresa, setSavingEmpresa] = useState(false);

  const [savingUsuario, setSavingUsuario] = useState(false);

  const [savingPassword, setSavingPassword] = useState(false);

  const [form, setForm] = useState({
    razonSocial: "",
    nombreFantasia: "",
    cuit: "",
    localidadId: "",
    moneda: "ARG",
    estado: "Activo",
    nombreCompleto: "",
    email: "",
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
      setForm((prev) => ({
        ...prev,
        empresaId: perfil.id || perfil.empresaId || perfil.EmpresaId,
        razonSocial: perfil.razonSocial,
        nombreFantasia: perfil.nombreFantasia || perfil.nombreFantasia,
        cuit: perfil.cuit,
        localidadId: perfil.localidadId || perfil.localidadId,
        moneda: perfil.moneda || perfil.moneda,
        estado: perfil.estado,
        nombreCompleto: perfil.nombreCompleto || perfil.nombreCompleto,
        email: perfil.email,
        telefono: perfil.telefono,
        avatar: perfil.avatarUrl,
      }));
    }
  }, [perfil]);

  const updateField = (field, value) => {
    setForm((prev) => ({
      ...prev,
      [field]: value,
    }));
  };

  // =========================
  // EMPRESA
  // =========================

  const handleGuardarEmpresa = async () => {
    try {
      setSavingEmpresa(true);

      const payload = {
        empresaId: form.empresaId,
        nombreFantasia: form.nombreFantasia,
        localidadId: form.localidadId,
        moneda: form.moneda,
      };

      await empresaService.updateEmpresa(payload);

      toastService.success("¡Éxito!", {
      description: "La empresa ha sido actualizada correctamente.",
    });
    } catch (error) {
      console.error(error);

      toastService.error("Error al guardar", {
      description: "Ocurrió un problema, intenta nuevamente.",
    });
    } finally {
      setSavingEmpresa(false);
    }
  };

  // =========================
  // USUARIO
  // =========================

  const handleGuardarUsuario = async () => {
    try {
      setSavingUsuario(true);

      const payload = {
        nombreCompleto: form.nombreCompleto,
        telefono: form.telefono,
        avatarUrl: form.avatar,
      };

      console.log(payload);

      await usuarioService.updateUsuario(payload);

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

  // =========================
  // PASSWORD
  // =========================

  const handleCambiarPassword = async () => {
    try {
      setSavingPassword(true);

      const payload = {
        passwordActual: form.passwordActual,
        passwordNueva: form.passwordNueva,
      };

      console.log(payload);

      await usuarioService.cambiarPassword(payload);

      alert("Contraseña actualizada");

      updateField("passwordActual", "");

      updateField("passwordNueva", "");
    } catch (error) {
      console.error(error);

      alert("Error al actualizar contraseña");
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
    <div className="w-full mx-auto max-w-[1400px] py-8 space-y-8">
      <PerfilEmpresaSection
        form={form}
        updateField={updateField}
        localidades={localidades}
        onSave={handleGuardarEmpresa}
        saving={savingEmpresa}
      />

      <div className="grid grid-cols-1 lg:grid-cols-2 gap-8">
        <PerfilUsuarioSection
          form={form}
          updateField={updateField}
          onSave={handleGuardarUsuario}
          saving={savingUsuario}
        />

        <PerfilSeguridadSection
          form={form}
          updateField={updateField}
          onSave={handleCambiarPassword}
          saving={savingPassword}
        />
      </div>

      <PerfilAvatarSection form={form} updateField={updateField} />
    </div>
  );
}
