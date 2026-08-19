/* eslint-disable react-hooks/set-state-in-effect */
import { useEffect, useState } from "react";
import { AtSign, ContactRound, MapPin, MapPinned, Phone, UserRound } from "lucide-react";
import { ModalCustom } from "./ModalCustom";
import { AppInput } from "@/components/ui/custom/AppInput";
import { AppSearchSelect } from "@/components/ui/custom/AppSelect";
import { clienteSchema } from "@/validations/cliente.validation";

export function ClienteModal({ isOpen, onClose, onSave, cliente = null, localidades = [], loading = false, serverError = "" }) {
  const [form, setForm] = useState({ nombreCompleto: "", dni: "", telefono: "", email: "", domicilio: "", localidadId: "" });
  const [localErrors, setLocalErrors] = useState({});

  useEffect(() => {
    setForm({
      nombreCompleto: cliente?.nombreCompleto || "",
      dni: cliente?.dni || "",
      telefono: cliente?.telefono || "",
      email: cliente?.email || "",
      domicilio: cliente?.domicilio || "",
      localidadId: cliente?.localidadId || "",
    });
    setLocalErrors({});
  }, [cliente, isOpen]);

  const updateField = (field, value) => {
    setForm((current) => ({ ...current, [field]: value }));
    if (localErrors[field]) setLocalErrors((current) => ({ ...current, [field]: undefined }));
  };

  const handleSave = () => {
    const result = clienteSchema.safeParse(form);
    if (!result.success) {
      setLocalErrors(result.error.flatten().fieldErrors);
      return;
    }

    setLocalErrors({});
    onSave({ clienteId: cliente?.clienteId, ...result.data });
  };

  return (
    <ModalCustom isOpen={isOpen} onClose={onClose} onSave={handleSave} title={cliente ? "Editar Cliente" : "Nuevo Cliente"} icon={ContactRound} loading={loading} saveText={cliente ? "Actualizar" : "Guardar"} maxWidth="max-w-3xl">
      <div className="grid grid-cols-1 gap-4 md:grid-cols-2">
        <AppInput label="Nombre completo *" icon={UserRound} placeholder="Ej: JUAN PÉREZ" value={form.nombreCompleto} onChange={(event) => updateField("nombreCompleto", event.target.value.toUpperCase())} error={localErrors.nombreCompleto?.[0] || serverError} autoFocus />
        <AppInput label="DNI *" icon={ContactRound} placeholder="Ej: 12345678" value={form.dni} onChange={(event) => updateField("dni", event.target.value.replace(/\D/g, "").slice(0, 8))} error={localErrors.dni?.[0]} />
        <AppInput label="Teléfono *" icon={Phone} placeholder="Ej: 11 1234-5678" value={form.telefono} onChange={(event) => updateField("telefono", event.target.value)} error={localErrors.telefono?.[0]} />
        <AppInput label="Email *" icon={AtSign} placeholder="Ej: cliente@email.com" value={form.email} onChange={(event) => updateField("email", event.target.value)} error={localErrors.email?.[0]} />
        <AppInput label="Domicilio *" icon={MapPinned} placeholder="Ej: AV. SAN MARTÍN 123" value={form.domicilio} onChange={(event) => updateField("domicilio", event.target.value.toUpperCase())} error={localErrors.domicilio?.[0]} />
        <AppSearchSelect label="Localidad *" icon={MapPin} value={form.localidadId} onValueChange={(value) => updateField("localidadId", value)} options={localidades} optionValue="id" optionLabel="nombre" placeholder="SELECCIONE..." searchPlaceholder="Buscar localidad" emptyText="No se encontraron localidades" error={localErrors.localidadId?.[0]} />
      </div>
    </ModalCustom>
  );
}
