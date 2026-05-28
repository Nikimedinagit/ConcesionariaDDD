import { ShieldCheck, Globe, MapPinned, Smartphone } from "lucide-react";

import { Section } from "./Section";
import { AppInput } from "@/components/ui/custom/AppInput";
import { Button } from "@/components/ui/button";

import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";

export function RecuperacionForm({
  form,
  updateField,
  onSave,
  saving,
  errors,
}) {
  return (
    <Section title="Recuperación de Cuenta" icon={ShieldCheck}>
      <div className="space-y-6">
        <div className="space-y-2">
          <p className="text-md text-slate-600">
            Tu número de teléfono es esencial para recuperar el acceso a tu
            cuenta si olvidas tu contraseña.
          </p>
        </div>

        <div className="grid grid-cols-1 md:grid-cols-[160px_180px_1fr_auto] gap-3 items-start">
          <div className="space-y-1">
            <label className="text-sm font-medium text-slate-700">País</label>

            <Select
              value={form.codigoPais || "+54"}
              onValueChange={(value) => updateField("codigoPais", value)}
            >
              <SelectTrigger className="w-full">
                <div className="flex items-center gap-2">
                  <Globe className="h-4 w-4 text-slate-400" />
                  <SelectValue placeholder="Código" />
                </div>
              </SelectTrigger>

              <SelectContent className="bg-white border border-slate-200 text-slate-900">
                <SelectItem value="+54">🇦🇷 +54</SelectItem>
                <SelectItem value="+55">🇧🇷 +55</SelectItem>
                <SelectItem value="+56">🇨🇱 +56</SelectItem>
                <SelectItem value="+57">🇨🇴 +57</SelectItem>
                <SelectItem value="+58">🇻🇪 +58</SelectItem>
                <SelectItem value="+1">🇺🇸 +1</SelectItem>
                <SelectItem value="+34">🇪🇸 +34</SelectItem>
              </SelectContent>
            </Select>
          </div>

          {/* CODIGO AREA */}
          <div className="space-y-1">
            <AppInput
              label="Cod. Área (sin 0)"
              placeholder="351"
              value={form.codigoArea || ""}
              onChange={(e) =>
                updateField("codigoArea", e.target.value.replace(/\D/g, ""))
              }
              icon={MapPinned}
              maxLength={4}
            />
          </div>

          {/* TELEFONO */}
          <div className="space-y-1">
            <AppInput
              label="Teléfono (sin 15)"
              placeholder="1234567"
              value={form.telefono || ""}
              onChange={(e) =>
                updateField("telefono", e.target.value.replace(/\D/g, ""))
              }
              icon={Smartphone}
              maxLength={10}
            />

            <div className="h-5">
              {(errors?.telefono ||
                errors?.codigoArea ||
                errors?.codigoPais) && (
                <p className="text-red-500 text-sm font-medium mt-1">
                  {errors?.telefono?.[0] ||
                    errors?.codigoArea?.[0] ||
                    errors?.codigoPais?.[0]}
                </p>
              )}
            </div>
          </div>

          <Button
            onClick={onSave}
            disabled={saving}
            className="h-[40px] mt-[29px] rounded-lg bg-[hsl(var(--nav-bg))] text-white font-bold px-10 whitespace-nowrap"
          >
            {saving ? "Guardando..." : "Actualizar Recuperación"}
          </Button>
        </div>
      </div>
    </Section>
  );
}
