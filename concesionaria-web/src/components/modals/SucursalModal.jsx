/* eslint-disable react-hooks/set-state-in-effect */
import { useEffect, useState } from "react";
import { Building2, MapPin, Navigation } from "lucide-react";
import { sucursalSchema } from "@/validations/sucursal.validation";
import { ModalCustom } from "./ModalCustom";
import { AppInput } from "@/components/ui/custom/AppInput";
import { AppSearchSelect } from "@/components/ui/custom/AppSelect";

export function SucursalModal({
  isOpen,
  onClose,
  onSave,
  sucursal = null,
  localidades = [],
  loading = false,
  serverError = "",
}) {
  const [form, setForm] = useState({
    nombre: "",
    direccion: "",
    localidadId: "",
  });
  const [localErrors, setLocalErrors] = useState({});

  useEffect(() => {
    setForm({
      nombre: sucursal?.nombre || "",
      direccion: sucursal?.direccion || "",
      localidadId: sucursal?.localidadId || "",
    });
    setLocalErrors({});
  }, [sucursal, isOpen]);

  const updateField = (field, value) => {
    setForm((current) => ({ ...current, [field]: value }));

    if (localErrors[field] || localErrors.general) {
      setLocalErrors((current) => ({
        ...current,
        [field]: undefined,
        general: undefined,
      }));
    }
  };

  const handleSave = async () => {
    const result = sucursalSchema.safeParse(form);

    if (!result.success) {
      const fieldErrors = result.error.flatten().fieldErrors;
      setLocalErrors(fieldErrors);
      return;
    }

    setLocalErrors({});
    onSave({
      sucursalId: sucursal?.sucursalId,
      nombre: form.nombre,
      direccion: form.direccion,
      localidadId: form.localidadId,
    });
  };

  return (
    <ModalCustom
      isOpen={isOpen}
      onClose={onClose}
      onSave={handleSave}
      title={sucursal ? "Editar Sucursal" : "Nueva Sucursal"}
      icon={Building2}
      loading={loading}
      saveText={sucursal ? "Actualizar" : "Guardar"}
      maxWidth="max-w-2xl"
    >
      <div className="grid grid-cols-1 gap-4 md:grid-cols-2">
        <AppInput
          label="Nombre *"
          icon={Building2}
          placeholder="Ej: CASA CENTRAL"
          value={form.nombre}
          onChange={(e) => updateField("nombre", e.target.value.toUpperCase())}
          error={localErrors.nombre?.[0] || serverError}
          autoFocus
        />

        <AppInput
          label="Dirección *"
          icon={Navigation}
          placeholder="Ej: AV. SAN MARTÍN 123"
          value={form.direccion}
          onChange={(e) =>
            updateField("direccion", e.target.value.toUpperCase())
          }
          error={localErrors.direccion?.[0]}
        />

        <AppSearchSelect
          label="Localidad *"
          icon={MapPin}
          value={form.localidadId}
          onValueChange={(value) => updateField("localidadId", value)}
          options={localidades}
          optionValue="id"
          optionLabel="nombre"
          placeholder="SELECCIONE..."
          searchPlaceholder="Buscar localidad"
          emptyText="No se encontraron localidades"
          error={localErrors.localidadId?.[0]}
          className="md:col-span-2"
        />

      </div>
    </ModalCustom>
  );
}
