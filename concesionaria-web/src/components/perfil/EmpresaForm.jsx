import { Building2, DollarSign, Hash, MapPin, Tag } from "lucide-react";

import { Button } from "@/components/ui/button";

import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";

import { AppInput } from "@/components/ui/custom/AppInput";

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

       <div className="lg:col-span-2 space-y-1"> 
          <AppInput
            label="Nombre Fantasía"
            value={form.nombreFantasia || ""}
            onChange={(e) => updateField("nombreFantasia", e.target.value)}
            icon={Tag}
          />
          {errors?.nombreFantasia && (
            <p className="text-red-500 text-sm font-medium mt-1">
              {errors.nombreFantasia[0]}
            </p>
          )}
        </div>

        <AppInput label="CUIT" value={form.cuit} disabled icon={Hash} />

        <div className="space-y-1.5">
          <label className="text-sm font-medium text-slate-700">
            Localidad
          </label>

          <Select
            value={form.localidadId}
            onValueChange={(v) => updateField("localidadId", v)}
          >
            <SelectTrigger className="h-[40px] rounded-lg border-slate-200">
              <div className="flex items-center gap-2 truncate">
                <MapPin className="w-4 h-4 text-slate-400" />

                <SelectValue placeholder="Seleccione..." />
              </div>
            </SelectTrigger>

            <SelectContent className="bg-white border shadow-lg">
              {localidades.map((loc) => (
                <SelectItem key={loc.id} value={loc.id}>
                  {loc.nombre}
                </SelectItem>
              ))}
            </SelectContent>
          </Select>
        </div>

        <div className="space-y-1.5">
          <label className="text-sm font-medium text-slate-700">Moneda</label>

          <Select
            value={form.moneda}
            onValueChange={(v) => updateField("moneda", v)}
          >
            <SelectTrigger className="h-[40px] rounded-lg border-slate-200">
              <div className="flex items-center gap-2">
                <DollarSign className="w-4 h-4 text-slate-400" />

                <SelectValue />
              </div>
            </SelectTrigger>

            <SelectContent className="bg-white border">
              <SelectItem value="ARG">ARG</SelectItem>
              <SelectItem value="USD">USD</SelectItem>
              <SelectItem value="BRL">BRL</SelectItem>
            </SelectContent>
          </Select>
        </div>

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
