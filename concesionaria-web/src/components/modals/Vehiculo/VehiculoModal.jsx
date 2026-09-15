/* eslint-disable react-hooks/set-state-in-effect */
import { useEffect, useState } from "react";
import {
  AlertCircle,
  CalendarDays,
  ScanLine,
  CarFront,
  Gauge,
  Palette,
  Settings2,
  Sparkles,
  RectangleHorizontal,
} from "lucide-react";
import { ModalCustom } from "../ModalCustom";
import { AppInput } from "@/components/ui/custom/AppInput";
import { AppSearchSelect, AppSelect } from "@/components/ui/custom/AppSelect";
import { vehiculoSchema } from "@/validations/Vehiculo/vehiculo.validation";
import { VehiculoImagenStep } from "./VehiculoImagenStep";
import VehiculoService from "@/services/Vehiculo/vehiculoService";
import { vehiculoImagesSchema } from "@/validations/Vehiculo/vehiculoImagen.validation";

const condiciones = [
  { id: "1", nombre: "NUEVO" },
  { id: "2", nombre: "USADO" },
  { id: "3", nombre: "CONSIGNACIÓN" },
];

const initialForm = {
  modeloId: "",
  version: "",
  patente: "",
  color: "",
  anio: "",
  kilometraje: "",
  condicion: "",
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
  serverFieldErrors = {},
}) {
  const [form, setForm] = useState(initialForm);
  const [localErrors, setLocalErrors] = useState({});
  const [step, setStep] = useState(1);
  const [images, setImages] = useState([]);
  const [removedImageIds, setRemovedImageIds] = useState([]);
  const [imagesLoading, setImagesLoading] = useState(false);
  const [imagesLoaded, setImagesLoaded] = useState(false);
  const [imageError, setImageError] = useState("");

  useEffect(() => {
    setForm({
      modeloId: vehiculo?.modeloId || "",
      version: vehiculo?.version || "",
      patente: vehiculo?.patente || "",
      color: vehiculo?.color || "",
      anio: vehiculo?.anio?.toString() || "",
      kilometraje: vehiculo?.kilometraje?.toString() || "",
      condicion: vehiculo?.condicion?.toString() || "",
      precioCompra: vehiculo?.precioCompra?.toString() || "",
      precioVenta: vehiculo?.precioVenta?.toString() || "",
    });

    setLocalErrors({});
    setStep(1);
    setImages([]);
    setRemovedImageIds([]);
    setImagesLoaded(false);
    setImageError("");
  }, [vehiculo, isOpen]);

  useEffect(() => {
    if (step !== 2 || !vehiculo?.vehiculoId || imagesLoaded) return;

    let active = true;
    setImagesLoading(true);
    setImageError("");

    VehiculoService.getImagenes(vehiculo.vehiculoId)
      .then((data) => {
        if (!active) return;

        setImages(
          data.map((image) => ({
            id: image.imagenId,
            file: null,
            preview: image.url,
            nombreOriginal: image.nombreOriginal,
            tamanioBytes: image.tamanioBytes,
            isPrincipal: image.esPrincipal,
            isExisting: true,
          })),
        );
        setImagesLoaded(true);
      })
      .catch((error) => {
        if (!active) return;
        setImageError(
          error.response?.data?.message ||
            "No se pudieron cargar las imágenes del vehículo.",
        );
      })
      .finally(() => {
        if (active) setImagesLoading(false);
      });

    return () => {
      active = false;
    };
  }, [step, vehiculo?.vehiculoId, imagesLoaded]);

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

    if (step === 1) {
      setStep(2);
      return;
    }

    const imagesResult = vehiculoImagesSchema.safeParse(images);

    if (!imagesResult.success) {
      setImageError(imagesResult.error.issues[0].message);
      return;
    }

    setImageError("");

    onSave({
      vehiculoId: vehiculo?.vehiculoId,
      ...result.data,
      version: result.data.version.toUpperCase(),
      patente: result.data.patente.toUpperCase(),
      color: result.data.color.toUpperCase(),
    }, {
      images,
      removedImageIds,
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
      saveText={step === 1 ? "Siguiente" : vehiculo ? "Actualizar" : "Guardar"}
      cancelText={step === 1 ? "Cancelar" : "Atrás"}
      onCancel={step === 1 ? onClose : () => setStep(1)}
      maxWidth="max-w-5xl"
    >
      {serverError && (
        <div className="mb-4 flex items-start gap-2 rounded-lg border border-red-200 bg-red-50 px-3 py-2 text-sm font-medium text-red-600">
          <AlertCircle className="mt-0.5 h-4 w-4 shrink-0" />
          <span>{serverError}</span>
        </div>
      )}

      <div className="flex items-center justify-center pb-1">
        <div className="flex w-full max-w-md items-center">
          <StepIndicator number={1} label="Datos del vehículo" active={step === 1} completed={step > 1} />
          <div className={`mx-3 h-px flex-1 ${step > 1 ? "bg-[hsl(var(--nav-bg))]" : "bg-slate-300"}`} />
          <StepIndicator number={2} label="Imágenes" active={step === 2} />
        </div>
      </div>

      {step === 1 ? (
      <div className="grid grid-cols-1 gap-4 md:grid-cols-12">
        <div className="md:col-span-6">
          <AppSearchSelect
            label="Modelo *"
            icon={ScanLine}
            value={form.modeloId}
            onValueChange={(value) => updateField("modeloId", value)}
            options={modelos}
            optionValue="modeloVehiculoId"
            optionLabel={(modelo) =>
              `${modelo.nombre} — ${modelo.marcaVehiculoNombre} — ${modelo.tipoVehiculoNombre}`
            }
            placeholder="SELECCIONE..."
            searchPlaceholder="Buscar modelo, marca o tipo..."
            emptyText="No se encontraron modelos"
            error={localErrors.modeloId?.[0]}
          />
        </div>

        <div className="md:col-span-3">
          <AppInput
            label="Versión *"
            icon={Settings2}
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

        <div className="md:col-span-4">
          <AppInput
            label="Patente *"
            icon={RectangleHorizontal}
            placeholder="Ej: AB123CD"
            value={form.patente}
            onChange={(event) =>
              updateField("patente", event.target.value.toUpperCase())
            }
            error={localErrors.patente?.[0]}
          />
        </div>

        <div className="md:col-span-4">
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

        <div className="md:col-span-4">
          <AppSelect
            label="Condición *"
            icon={Sparkles}
            value={form.condicion}
            onValueChange={handleCondicionChange}
            options={condiciones}
            optionValue="id"
            optionLabel="nombre"
            placeholder="SELECCIONE..."
            error={localErrors.condicion?.[0]}
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
            prefix="$"
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
            prefix="$"
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
      ) : (
        <VehiculoImagenStep
          images={images}
          onChange={(nextImages) => {
            setImages(nextImages);
            setImageError("");
          }}
          onRemoveExisting={(imageId) =>
            setRemovedImageIds((current) => [...current, imageId])
          }
          loading={imagesLoading}
          externalError={imageError}
        />
      )}
    </ModalCustom>
  );
}

function StepIndicator({ number, label, active, completed = false }) {
  return (
    <div className="flex items-center gap-2">
      <span
        className={`flex h-9 w-9 items-center justify-center rounded-full text-sm font-black ring-1 transition-colors ${
          active || completed
            ? "bg-[hsl(var(--nav-bg))] text-white ring-[hsl(var(--nav-bg))]"
            : "bg-white text-slate-400 ring-slate-300"
        }`}
      >
        {number}
      </span>
      <span
        className={`hidden whitespace-nowrap text-sm font-bold sm:block ${
          active || completed ? "text-slate-800" : "text-slate-400"
        }`}
      >
        {label}
      </span>
    </div>
  );
}
