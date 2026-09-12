/* eslint-disable react-hooks/set-state-in-effect */
import { useEffect, useState } from "react";
import {
  AlertCircle,
  AtSign,
  ContactRound,
  MapPin,
  MapPinned,
  Phone,
  UserRound,
} from "lucide-react";
import { ModalCustom } from "../ModalCustom";
import { AppInput } from "@/components/ui/custom/AppInput";
import { AppTextarea } from "@/components/ui/custom/AppTextarea";
import { AppSearchSelect } from "@/components/ui/custom/AppSelect";
import { proveedorSchema } from "@/validations/Persona/proveedor.validation";

export function ProveedorModal({
  isOpen,
  onClose,
  onSave,
  proveedor = null,
  localidades = [],
  loading = false,
  serverError = "",
  serverFieldErrors = {},
}) {
  const [form, setForm] = useState({
    nombre: "",
    cuil: "",
    telefono: "",
    email: "",
    domicilio: "",
    servicio: "",
    observacion: "",
    localidadId: "",
  });

  const [localErrors, setLocalErrors] = useState({});

  useEffect(() => {
    setForm({
      nombre: proveedor?.nombre || "",
      cuil: proveedor?.cuil || "",
      telefono: proveedor?.telefono || "",
      email: proveedor?.email || "",
      domicilio: proveedor?.domicilio || "",
      servicio: proveedor?.servicio || "",
      observacion: proveedor?.observacion || "",
      localidadId: proveedor?.localidadId || "",
    });
    setLocalErrors({});
  }, [proveedor, isOpen]);

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
    if (localErrors[field])
      setLocalErrors((current) => ({ ...current, [field]: undefined }));
  };

  const handleSave = () => {
    const result = proveedorSchema.safeParse(form);
    if (!result.success) {
      setLocalErrors(result.error.flatten().fieldErrors);
      return;
    }

    setLocalErrors({});
    onSave({ proveedorId: proveedor?.proveedorId, ...result.data });
  };

  return (
    <ModalCustom
      isOpen={isOpen}
      onClose={onClose}
      onSave={handleSave}
      title={proveedor ? "Editar Proveedor" : "Nuevo Proveedor"}
      icon={ContactRound}
      loading={loading}
      saveText={proveedor ? "Actualizar" : "Guardar"}
      maxWidth="max-w-5xl"
    >
      {serverError && (
        <div className="flex items-start gap-2 rounded-lg border border-red-200 bg-red-50 px-3 py-2 text-sm font-medium text-red-600">
          <AlertCircle className="mt-0.5 h-4 w-4 shrink-0" />
          <span>{serverError}</span>
        </div>
      )}

      <div className="grid grid-cols-1 gap-x-5 gap-y-4 md:grid-cols-3">

        <AppInput
          label="Nombre *"
          icon={UserRound}
          placeholder="Ej: JUAN PÉREZ"
          value={form.nombre}
          onChange={(event) =>
            updateField("nombre", event.target.value.toUpperCase())
          }
          error={localErrors.nombre?.[0]}
          autoFocus
        />

        <AppInput
          label="CUIL *"
          icon={ContactRound}
          placeholder="Ej: 20123456789"
          value={form.cuil}
          onChange={(event) =>
            updateField(
              "cuil",
              event.target.value.replace(/\D/g, "").slice(0, 11),
            )
          }
          error={localErrors.cuil?.[0]}
        />

        <AppInput
          label="Teléfono *"
          icon={Phone}
          placeholder="Ej: 11 1234-5678"
          value={form.telefono}
          onChange={(event) => updateField("telefono", event.target.value)}
          error={localErrors.telefono?.[0]}
        />


        <AppInput
          label="Email *"
          icon={AtSign}
          placeholder="Ej: proveedor@email.com"
          value={form.email}
          onChange={(event) => updateField("email", event.target.value)}
          error={localErrors.email?.[0]}
        />

        <AppInput
          label="Domicilio *"
          icon={MapPinned}
          placeholder="Ej: AV. SAN MARTÍN 123"
          value={form.domicilio}
          onChange={(event) =>
            updateField("domicilio", event.target.value.toUpperCase())
          }
          error={localErrors.domicilio?.[0]}
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
        />

        <div className="md:col-span-3">
          <AppInput
            label="Servicio *"
            icon={MapPin}
            placeholder="Ej: REPUESTOS, MECÁNICA, LUBRICANTES..."
            value={form.servicio}
            onChange={(event) =>
              updateField("servicio", event.target.value.toUpperCase())
            }
            error={localErrors.servicio?.[0]}
          />
        </div>

        <div className="md:col-span-3">
          <AppTextarea
            label="Observaciones"
            icon={MapPin}
            placeholder="Ej: OBSERVACIONES ADICIONALES..."
            value={form.observacion}
            onChange={(event) =>
              updateField("observacion", event.target.value.toUpperCase())
            }
          />
        </div>
      </div>
    </ModalCustom>
  );
}
