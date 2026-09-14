import { Building2, DollarSign, Hash, MapPin, Tag } from "lucide-react";

import { Button } from "@/components/ui/button";

import { AppInput } from "@/components/ui/custom/AppInput";
import { AppSelect, AppSearchSelect } from "@/components/ui/custom/AppSelect";

import { Section } from "./Section";

export function PerfilEmpresaSection({
  form,
  updateField,
  localidades,
  onSave,
  saving,
  errors,
}) {
  return (
    <Section
      title="Información de la Empresa"
      icon={Building2}
      badge={form.estado}
    >
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 items-end">
        <div className="lg:col-span-2">
          <AppInput
            label="Razón Social"
            value={form.razonSocial}
            disabled
            icon={Building2}
          />
        </div>

        <AppInput
          className="lg:col-span-2"
          label="Nombre Fantasía"
          value={form.nombreFantasia || ""}
          onChange={(e) =>
            updateField("nombreFantasia", e.target.value.toUpperCase())
          }
          icon={Tag}
          error={errors?.nombreFantasia?.[0]}
        />

        <AppInput label="CUIT" value={form.cuit} disabled icon={Hash} />

        <AppSearchSelect
          label="Localidad"
          icon={MapPin}
          value={form.localidadId}
          onValueChange={(v) => updateField("localidadId", v)}
          options={localidades}
          optionValue="id"
          optionLabel="nombre"
          placeholder="Seleccione..."
          searchPlaceholder="Buscar localidad"
          emptyText="No se encontraron localidades"
        />

        <AppSelect
          label="Moneda"
          icon={DollarSign}
          value={form.moneda}
          onValueChange={(v) => updateField("moneda", v)}
          options={[
            { id: "ARG", nombre: "ARG" },
            { id: "USD", nombre: "USD" },
            { id: "BRL", nombre: "BRL" },
          ]}
          optionValue="id"
          optionLabel="nombre"
        />

        <Button
          onClick={onSave}
          disabled={saving}
          className="h-[40px] rounded-lg bg-[hsl(var(--nav-bg))] text-white font-bold px-8"
        >
          {saving ? "Guardando..." : "Actualizar Empresa"}
        </Button>
      </div>
    </Section>
  );
}
