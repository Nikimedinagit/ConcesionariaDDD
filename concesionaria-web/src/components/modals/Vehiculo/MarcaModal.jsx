import { useState } from "react";
import { Tag, Tags } from "lucide-react";
import { ModalCustom } from "../ModalCustom";
import { AppInput } from "@/components/ui/custom/AppInput";
import { marcaSchema } from "@/validations/Vehiculo/marca.validation";

export function MarcaModal({
  isOpen,
  onClose,
  onSave,
  marca = null,
  loading = false,
  serverError = "",
}) {
  const [nombre, setNombre] = useState(marca?.nombre || "");
  const [localError, setLocalError] = useState("");

  const handleSave = () => {
    const result = marcaSchema.safeParse({ nombre });

    if (!result.success) {
      const fieldErrors = result.error.flatten().fieldErrors;
      setLocalError(fieldErrors.nombre?.[0] || "Nombre inválido.");
      return;
    }

    setLocalError("");
    onSave({
      marcaVehiculoId: marca?.marcaVehiculoId,
      nombre: result.data.nombre.toUpperCase(),
    });
  };

  return (
    <ModalCustom
      isOpen={isOpen}
      onClose={onClose}
      onSave={handleSave}
      title={marca ? "Editar Marca" : "Nueva Marca"}
      icon={Tags}
      loading={loading}
      saveText={marca ? "Actualizar" : "Agregar"}
      maxWidth="max-w-lg"
    >
      <AppInput
        label="Nombre *"
        icon={Tag}
        placeholder="Ej: TOYOTA"
        value={nombre}
        onChange={(event) => {
          setNombre(event.target.value.toUpperCase());
          if (localError) setLocalError("");
        }}
        error={localError || serverError}
        autoFocus
      />
    </ModalCustom>
  );
}
