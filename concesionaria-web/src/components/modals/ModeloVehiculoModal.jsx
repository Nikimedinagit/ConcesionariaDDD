import { useState } from "react";
import { CarFront, Tag, Tags } from "lucide-react";
import { ModalCustom } from "./ModalCustom";
import { AppInput } from "@/components/ui/custom/AppInput";
import { AppSearchSelect } from "@/components/ui/custom/AppSelect";
import { modeloVehiculoSchema } from "@/validations/modeloVehiculo.validation";

export function ModeloVehiculoModal({ isOpen, onClose, onSave, modelo = null, marcas, tiposVehiculos, loading = false, serverError = "" }) {
  const [nombre, setNombre] = useState(modelo?.nombre || "");
  const [marcaVehiculoId, setMarcaVehiculoId] = useState(modelo?.marcaVehiculoId || "");
  const [tipoVehiculoId, setTipoVehiculoId] = useState(modelo?.tipoVehiculoId || "");
  const [errors, setErrors] = useState({});

  const clearError = (field) => setErrors((current) => ({ ...current, [field]: "" }));
  const handleSave = () => {
    const result = modeloVehiculoSchema.safeParse({ nombre, marcaVehiculoId, tipoVehiculoId });
    if (!result.success) {
      setErrors(result.error.flatten().fieldErrors);
      return;
    }
    setErrors({});
    onSave({ modeloVehiculoId: modelo?.modeloVehiculoId, ...result.data, nombre: result.data.nombre.toUpperCase() });
  };

  return (
    <ModalCustom isOpen={isOpen} onClose={onClose} onSave={handleSave} title={modelo ? "Editar Modelo" : "Nuevo Modelo"} icon={CarFront} loading={loading} saveText={modelo ? "Actualizar" : "Agregar"} maxWidth="max-w-lg">
      <div className="space-y-4">
        <AppInput label="Nombre *" icon={Tag} placeholder="Ej: COROLLA" value={nombre} onChange={(event) => { setNombre(event.target.value.toUpperCase()); clearError("nombre"); }} error={errors.nombre?.[0] || serverError} autoFocus />
        <AppSearchSelect label="Marca *" icon={Tags} value={marcaVehiculoId} onValueChange={(value) => { setMarcaVehiculoId(value); clearError("marcaVehiculoId"); }} options={marcas} optionValue="marcaVehiculoId" optionLabel="nombre" placeholder="Seleccione una marca" searchPlaceholder="Buscar marca..." error={errors.marcaVehiculoId?.[0]} />
        <AppSearchSelect label="Tipo de vehículo *" icon={CarFront} value={tipoVehiculoId} onValueChange={(value) => { setTipoVehiculoId(value); clearError("tipoVehiculoId"); }} options={tiposVehiculos} optionValue="tipoVehiculoId" optionLabel="nombre" placeholder="Seleccione un tipo" searchPlaceholder="Buscar tipo..." error={errors.tipoVehiculoId?.[0]} />
      </div>
    </ModalCustom>
  );
}
