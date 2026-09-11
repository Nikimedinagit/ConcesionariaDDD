/* eslint-disable react-hooks/set-state-in-effect */
import { useEffect, useState } from "react";
import {
  CalendarDays,
  CarFront,
  CircleDollarSign,
  Gauge,
  Palette,
  Settings,
  Tag,
} from "lucide-react";
import { ModalCustom } from "../ModalCustom";
import { AppInput } from "@/components/ui/custom/AppInput";
import { AppSearchSelect } from "@/components/ui/custom/AppSelect";
import { vehiculoSchema } from "@/validations/Vehiculo/vehiculo.validation";

const condiciones = [
  { id: "1", nombre: "NUEVO" },
  { id: "2", nombre: "USADO" },
];

const estados = [
  { id: "1", nombre: "DISPONIBLE" },
  { id: "2", nombre: "RESERVADO" },
  { id: "3", nombre: "VENDIDO" },
  { id: "4", nombre: "EN SERVICIO" },
];

export function VehiculoModal({
  isOpen,
  onClose,
  onSave,
  vehiculo = null,
  modelos = [],
  loading = false,
  serverError = "",
}) {
  const [form, setForm] = useState({
    modeloId: "",
    version: "",
    patente: "",
    color: "",
    anio: "",
    kilometraje: "",
    condicion: "",
    estado: "",
    precioCompra: "",
    precioVenta: "",
  });

  const [localErrors, setLocalErrors] = useState({});

  useEffect(() => {
    setForm({
      modeloId: vehiculo?.modeloId || "",
      version: vehiculo?.version || "",
      patente: vehiculo?.patente || "",
      color: vehiculo?.color || "",
      anio: vehiculo?.anio?.toString() || "",
      kilometraje: vehiculo?.kilometraje?.toString() || "",
      condicion: vehiculo?.condicion?.toString() || "",
      estado: vehiculo?.estado?.toString() || "",
      precioCompra: vehiculo?.precioCompra?.toString() || "",
      precioVenta: vehiculo?.precioVenta?.toString() || "",
    });

    setLocalErrors({});
  }, [vehiculo, isOpen]);

  const updateField = (field, value) => {
    setForm((current) => ({
      ...current,
      [field]: value,
    }));

    if (localErrors[field]) {
      setLocalErrors((current) => ({
        ...current,
        [field]: undefined,
      }));
    }
  };

  const handleCondicionChange = (value) => {
    setForm((current) => ({
      ...current,
      condicion: value,
      kilometraje: value === "1" ? "0" : current.kilometraje,
    }));

    setLocalErrors((current) => ({
      ...current,
      condicion: undefined,
      kilometraje: undefined,
    }));
  };

  const handleSave = () => {
    const result = vehiculoSchema.safeParse(form);

    if (!result.success) {
      setLocalErrors(result.error.flatten().fieldErrors);
      return;
    }

    setLocalErrors({});

    onSave({
      vehiculoId: vehiculo?.vehiculoId,
      ...result.data,
      version: result.data.version.toUpperCase(),
      patente: result.data.patente.toUpperCase(),
      color: result.data.color.toUpperCase(),
    });
  };

  return (
    <ModalCustom
      isOpen={isOpen}
      onClose={onClose}
      onSave={handleSave}
      title={vehiculo ? "Editar Vehículo" : "Nuevo Vehículo"}
      icon={CarFront}
      loading={loading}
      saveText={vehiculo ? "Actualizar" : "Guardar"}
      maxWidth="max-w-4xl"
    >
      <div className="grid grid-cols-1 gap-4 md:grid-cols-2">
        <AppSearchSelect
          label="Modelo *"
          icon={CarFront}
          value={form.modeloId}
          onValueChange={(value) => updateField("modeloId", value)}
          options={modelos}
          optionValue="modeloVehiculoId"
          optionLabel="nombre"
          placeholder="SELECCIONE..."
          searchPlaceholder="Buscar modelo..."
          emptyText="No se encontraron modelos"
          error={localErrors.modeloId?.[0]}
        />

        <AppInput
          label="Versión *"
          icon={Settings}
          placeholder="Ej: XEI 2.0"
          value={form.version}
          onChange={(event) =>
            updateField("version", event.target.value.toUpperCase())
          }
          error={localErrors.version?.[0]}
          autoFocus
        />

        <AppInput
          label="Patente *"
          icon={Tag}
          placeholder="Ej: AB123CD"
          value={form.patente}
          onChange={(event) =>
            updateField("patente", event.target.value.toUpperCase())
          }
          error={localErrors.patente?.[0] || serverError}
        />

        <AppInput
          label="Color *"
          icon={Palette}
          placeholder="Ej: BLANCO"
          value={form.color}
          onChange={(event) =>
            updateField("color", event.target.value.toUpperCase())
          }
          error={localErrors.color?.[0]}
        />

        <AppInput
          label="Año *"
          icon={CalendarDays}
          type="number"
          placeholder="Ej: 2025"
          value={form.anio}
          onChange={(event) => updateField("anio", event.target.value)}
          error={localErrors.anio?.[0]}
        />

        <AppSearchSelect
          label="Condición *"
          icon={CarFront}
          value={form.condicion}
          onValueChange={handleCondicionChange}
          options={condiciones}
          optionValue="id"
          optionLabel="nombre"
          placeholder="SELECCIONE..."
          searchPlaceholder="Buscar condición..."
          error={localErrors.condicion?.[0]}
        />

        <AppInput
          label="Kilometraje *"
          icon={Gauge}
          type="number"
          placeholder="Ej: 50000"
          value={form.kilometraje}
          disabled={form.condicion === "1"}
          onChange={(event) => updateField("kilometraje", event.target.value)}
          error={localErrors.kilometraje?.[0]}
        />

        <AppSearchSelect
          label="Estado *"
          icon={CarFront}
          value={form.estado}
          onValueChange={(value) => updateField("estado", value)}
          options={estados}
          optionValue="id"
          optionLabel="nombre"
          placeholder="SELECCIONE..."
          searchPlaceholder="Buscar estado..."
          error={localErrors.estado?.[0]}
        />

        <AppInput
          label="Precio de compra *"
          icon={CircleDollarSign}
          type="number"
          placeholder="Ej: 15000000"
          value={form.precioCompra}
          onChange={(event) => updateField("precioCompra", event.target.value)}
          error={localErrors.precioCompra?.[0]}
        />

        <AppInput
          label="Precio de venta *"
          icon={CircleDollarSign}
          type="number"
          placeholder="Ej: 18000000"
          value={form.precioVenta}
          onChange={(event) => updateField("precioVenta", event.target.value)}
          error={localErrors.precioVenta?.[0]}
        />
      </div>
    </ModalCustom>
  );
}
