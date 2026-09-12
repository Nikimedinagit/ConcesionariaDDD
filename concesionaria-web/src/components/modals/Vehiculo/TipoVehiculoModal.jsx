import { useState } from "react";
import { Car, Shapes  } from "lucide-react";
import { ModalCustom } from "../ModalCustom";
import { AppInput } from "@/components/ui/custom/AppInput";
import { tipoVehiculoSchema } from "@/validations/Vehiculo/tipoVehiculo.validation";

export function TipoVehiculoModal({
  isOpen,
  onClose,
  onSave,
  tipoVehiculo = null,
  loading = false,
  serverError = "",
}) {
  const [nombre, setNombre] = useState(tipoVehiculo?.nombre || "");
  const [localError, setLocalError] = useState("");

  const handleSave = () => {
    const result = tipoVehiculoSchema.safeParse({ nombre });

    if (!result.success) {
      const fieldErrors = result.error.flatten().fieldErrors;
      setLocalError(fieldErrors.nombre?.[0] || "Nombre inválido.");
      return;
    }

    setLocalError("");
    onSave({
      tipoVehiculoId: tipoVehiculo?.tipoVehiculoId,
      nombre: result.data.nombre.toUpperCase(),
    });
  };

  return (
    <ModalCustom
      isOpen={isOpen}
      onClose={onClose}
      onSave={handleSave}
      title={
        tipoVehiculo ? "Editar Tipo de Vehículo" : "Nuevo Tipo de Vehículo"
      }
      icon={Shapes }
      loading={loading}
      saveText={tipoVehiculo ? "Actualizar" : "Agregar"}
      maxWidth="max-w-lg"
    >
      <AppInput
        label="Nombre *"
        icon={Car}
        placeholder="Ej: AUTOMÓVIL"
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
