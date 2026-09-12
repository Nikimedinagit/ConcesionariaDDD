/* eslint-disable react-hooks/set-state-in-effect */
import { useEffect, useState } from "react";
import { AtSign, Building2, KeyRound, Shield, User } from "lucide-react";
import {
  usuarioCreateSchema,
  usuarioSchema,
} from "@/validations/Acceso/usuario.validation";
import { ModalCustom } from "../ModalCustom";
import { AppInput } from "@/components/ui/custom/AppInput";
import { AppSearchSelect } from "@/components/ui/custom/AppSelect";

export function UsuarioModal({
  isOpen,
  onClose,
  onSave,
  usuario = null,
  roles = [],
  sucursales = [],
  loading = false,
  serverError = "",
  serverFieldErrors = {},
}) {
  const [form, setForm] = useState({
    nombreCompleto: "",
    email: "",
    password: "",
    rolId: "",
    sucursalId: "",
  });
  const [localErrors, setLocalErrors] = useState({});

  useEffect(() => {
    setForm({
      nombreCompleto: usuario?.nombreCompleto || "",
      email: usuario?.email || "",
      password: "",
      rolId: usuario?.rolId || "",
      sucursalId: usuario?.sucursalId || "",
    });
    setLocalErrors({});
  }, [usuario, isOpen]);

  useEffect(() => {
    if (Object.keys(serverFieldErrors).length === 0) return;

    setLocalErrors((current) => ({
      ...current,
      ...Object.fromEntries(
        Object.entries(serverFieldErrors).map(([field, message]) => [
          field,
          [message],
        ]),
      ),
    }));
  }, [serverFieldErrors]);

  const updateField = (field, value) => {
    setForm((current) => ({ ...current, [field]: value }));

    if (localErrors[field]) {
      setLocalErrors((current) => ({
        ...current,
        [field]: undefined,
      }));
    }
  };

  const handleSave = async () => {
    const schema = usuario ? usuarioSchema : usuarioCreateSchema;
    const result = schema.safeParse(form);

    if (!result.success) {
      setLocalErrors(result.error.flatten().fieldErrors);
      return;
    }

    setLocalErrors({});

    const payload = {
      usuarioId: usuario?.usuarioId,
      nombreCompleto: form.nombreCompleto,
      email: form.email,
      rolId: form.rolId,
      sucursalId: form.sucursalId,
    };

    if (!usuario) {
      payload.password = form.password;
    }

    onSave(payload);
  };

  return (
    <ModalCustom
      isOpen={isOpen}
      onClose={onClose}
      onSave={handleSave}
      title={usuario ? "Editar Usuario" : "Nuevo Usuario"}
      icon={User}
      loading={loading}
      saveText={usuario ? "Actualizar" : "Guardar"}
      maxWidth="max-w-2xl"
    >
      <div className="grid grid-cols-1 gap-4 md:grid-cols-2">
        <AppInput
          label="Nombre Completo *"
          icon={User}
          placeholder="Ej: JUAN PEREZ"
          value={form.nombreCompleto}
          onChange={(e) =>
            updateField("nombreCompleto", e.target.value.toUpperCase())
          }
          error={localErrors.nombreCompleto?.[0]}
          autoFocus
        />

        <AppInput
          label="Email *"
          icon={AtSign}
          placeholder="usuario@empresa.com"
          value={form.email}
          onChange={(e) => updateField("email", e.target.value.toLowerCase())}
          error={localErrors.email?.[0]}
          disabled={Boolean(usuario)}
        />

        {!usuario && (
          <AppInput
            label="Contraseña *"
            icon={KeyRound}
            type="password"
            placeholder="Mínimo 6 caracteres"
            value={form.password}
            onChange={(e) => updateField("password", e.target.value)}
            error={localErrors.password?.[0]}
          />
        )}

        <AppSearchSelect
          label="Rol *"
          icon={Shield}
          value={form.rolId}
          onValueChange={(value) => updateField("rolId", value)}
          options={roles}
          optionValue="rolId"
          optionLabel="nombre"
          placeholder="Seleccione..."
          searchPlaceholder="Buscar rol"
          emptyText="No se encontraron roles"
          error={localErrors.rolId?.[0]}
        />

        <AppSearchSelect
          label="Sucursal *"
          icon={Building2}
          value={form.sucursalId}
          onValueChange={(value) => updateField("sucursalId", value)}
          options={sucursales}
          optionValue="sucursalId"
          optionLabel="nombre"
          placeholder="Seleccione..."
          searchPlaceholder="Buscar sucursal"
          emptyText="No se encontraron sucursales"
          error={localErrors.sucursalId?.[0]}
          className="md:col-span-2"
        />

        {serverError && (
          <p className="md:col-span-2 text-sm font-medium text-red-500">
            {serverError}
          </p>
        )}
      </div>
    </ModalCustom>
  );
}
