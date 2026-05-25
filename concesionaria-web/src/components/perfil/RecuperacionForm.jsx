import { Phone, ShieldCheck } from "lucide-react";
import { Section } from "./Section";
import { AppInput } from "@/components/ui/custom/AppInput";
import { Button } from "@/components/ui/button";

export function RecuperacionForm({ form, updateField, onSave, saving, errors }) {
  return (
    <Section title="Recuperación de Cuenta" icon={ShieldCheck}>
      <div className="space-y-6">
        <div className="space-y-2">
          <p className="text-md text-slate-600">
            Tu número de teléfono es esencial para recuperar el acceso a tu cuenta si olvidas tu contraseña.
          </p>
        </div>

        <div className="flex items-start gap-3">
          <div className="flex-grow">
            <AppInput
              label="Número de Teléfono"
              placeholder="Ej: +54 3562 123456"
              value={form.telefono || ""}
              onChange={(e) => updateField("telefono", e.target.value)}
              icon={Phone}
            />
            <div className="h-5"> 
              {errors?.telefono && (
                <p className="text-red-500 text-sm font-medium mt-1">
                  {errors.telefono[0]}
                </p>
              )}
            </div>
          </div>

          <Button
            onClick={onSave}
            disabled={saving}
            className="h-[40px] mt-[22px] rounded-lg bg-[hsl(var(--nav-bg))] text-white font-bold px-16"
          >
            {saving ? "Guardando..." : "Actualizar Recuperación"}
          </Button>
        </div>
      </div>
    </Section>
  );
}