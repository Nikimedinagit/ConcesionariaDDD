/* eslint-disable react-hooks/set-state-in-effect */
import { useEffect, useState } from "react";
import {
  AlertCircle,
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
import { AppSearchSelect, AppSelect } from "@/components/ui/custom/AppSelect";
import { vehiculoSchema } from "@/validations/Vehiculo/vehiculo.validation";

const condiciones = [
  { id: "1", nombre: "NUEVO" },
  { id: "2", nombre: "USADO" },
  { id: "3", nombre: "CONSIGNACIÓN" },
];

const estados = [
  { id: "1", nombre: "DISPONIBLE" },
  { id: "2", nombre: "VENDIDO" },
  { id: "3", nombre: "RESERVADO" },
  { id: "4", nombre: "EN SERVICIO" },
];

const initialForm = {
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
};

const formatPrice = (value) => {
  if (value === "" || value === null || value === undefined) return "";

  const rawValue = String(value);
  const [integerPart, decimalPart] = rawValue.split(".");
  const formattedInteger = new Intl.NumberFormat("en-US").format(
    Number(integerPart || 0),
  );

  if (!rawValue.includes(".")) return formattedInteger;

  return `${formattedInteger}.${decimalPart ?? ""}`;
};

const getRawPrice = (value) => {
  const normalized = value.replace(/,/g, "").replace(/[^\d.]/g, "");

  const [integerPart, ...decimalParts] = normalized.split(".");
  const decimalPart = decimalParts.join("").slice(0, 2);

  if (!normalized.includes(".")) return integerPart;

  return `${integerPart}.${decimalPart}`;
};

const formatKilometraje = (value) => {
  if (value === "" || value === null || value === undefined) return "";

  const digits = String(value).replace(/\D/g, "");
  if (!digits) return "";

  return new Intl.NumberFormat("es-AR").format(Number(digits));
};

const getRawKilometraje = (value) => value.replace(/\D/g, "");

export function VehiculoModal({
  isOpen,
  onClose,
  onSave,
  vehiculo = null,
  modelos = [],
  loading = false,
  serverError = "",
}) {
  const [form, setForm] = useState(initialForm);
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
      maxWidth="max-w-5xl"
    >
      {serverError && (
        <div className="mb-4 flex items-start gap-2 rounded-lg border border-red-200 bg-red-50 px-3 py-2 text-sm font-medium text-red-600">
          <AlertCircle className="mt-0.5 h-4 w-4 shrink-0" />
          <span>{serverError}</span>
        </div>
      )}

      <div className="grid grid-cols-1 gap-4 md:grid-cols-12">
        <div className="md:col-span-6">
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
        </div>

        <div className="md:col-span-3">
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
        </div>

        <div className="md:col-span-3">
          <AppInput
            label="Año *"
            icon={CalendarDays}
            type="number"
            min="1900"
            max={new Date().getFullYear()}
            placeholder="Ej: 2025"
            value={form.anio}
            onChange={(event) => updateField("anio", event.target.value)}
            error={localErrors.anio?.[0]}
          />
        </div>

        <div className="md:col-span-3">
          <AppInput
            label="Patente *"
            icon={Tag}
            placeholder="Ej: AB123CD"
            value={form.patente}
            onChange={(event) =>
              updateField("patente", event.target.value.toUpperCase())
            }
            error={localErrors.patente?.[0]}
          />
        </div>

        <div className="md:col-span-3">
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
        </div>

        <div className="md:col-span-3">
          <AppSelect
            label="Condición *"
            icon={CarFront}
            value={form.condicion}
            onValueChange={handleCondicionChange}
            options={condiciones}
            optionValue="id"
            optionLabel="nombre"
            placeholder="SELECCIONE..."
            error={localErrors.condicion?.[0]}
          />
        </div>

        <div className="md:col-span-3">
          <AppSelect
            label="Estado *"
            icon={CarFront}
            value={form.estado}
            onValueChange={(value) => updateField("estado", value)}
            options={estados}
            optionValue="id"
            optionLabel="nombre"
            placeholder="SELECCIONE..."
            error={localErrors.estado?.[0]}
          />
        </div>

        <div className="md:col-span-4">
          <AppInput
            label="Kilometraje *"
            icon={Gauge}
            type="text"
            inputMode="numeric"
            placeholder="Ej: 50.000"
            value={formatKilometraje(form.kilometraje)}
            disabled={form.condicion === "1"}
            onChange={(event) =>
              updateField("kilometraje", getRawKilometraje(event.target.value))
            }
            error={localErrors.kilometraje?.[0]}
            className="[&_input]:text-right [&_input]:font-semibold"
          />
        </div>

        <div className="md:col-span-4">
          <AppInput
            label="Precio de compra *"
            icon={CircleDollarSign}
            type="text"
            inputMode="numeric"
            placeholder="Ej: 15,000,000.50"
            value={formatPrice(form.precioCompra)}
            onChange={(event) =>
              updateField("precioCompra", getRawPrice(event.target.value))
            }
            error={localErrors.precioCompra?.[0]}
            className="[&_input]:text-right [&_input]:font-semibold"
          />
        </div>

        <div className="md:col-span-4">
          <AppInput
            label="Precio de venta *"
            icon={CircleDollarSign}
            type="text"
            inputMode="numeric"
            placeholder="Ej: 18,000,000.50"
            value={formatPrice(form.precioVenta)}
            onChange={(event) =>
              updateField("precioVenta", getRawPrice(event.target.value))
            }
            error={localErrors.precioVenta?.[0]}
            className="[&_input]:text-right [&_input]:font-semibold"
          />
        </div>
      </div>
    </ModalCustom>
  );
}
