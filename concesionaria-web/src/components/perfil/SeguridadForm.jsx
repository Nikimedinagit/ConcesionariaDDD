import { LockKeyhole } from "lucide-react";
import { Button } from "@/components/ui/button";
import { AppInput } from "@/components/ui/custom/AppInput";
import { Section } from "./Section";

export function PerfilSeguridadSection({ form, updateField, onSave, saving, errors }) {
  return (
    <Section title="Seguridad" icon={LockKeyhole}>
      <div className="space-y-4">
        <div className="space-y-1">
          <AppInput
            label="Contraseña Actual"
            type="password"
            placeholder="••••••••"
            value={form.passwordActual || ""}
            onChange={(e) => updateField("passwordActual", e.target.value)}
            icon={LockKeyhole}
          />
          {errors?.passwordActual && (
            <p className="text-red-500 text-sm font-medium mt-1">{errors.passwordActual[0]}</p>
          )}
        </div>

        <div className="space-y-1">
          <AppInput
            label="Nueva Contraseña"
            type="password"
            placeholder="••••••••"
            value={form.passwordNueva || ""}
            onChange={(e) => updateField("passwordNueva", e.target.value)}
            icon={LockKeyhole}
          />
          {errors?.passwordNueva && (
            <p className="text-red-500 text-sm font-medium mt-1">{errors.passwordNueva[0]}</p>
          )}
        </div>

        <Button 
          onClick={onSave}
          disabled={saving}
          className="w-full rounded-lg bg-[hsl(var(--nav-bg))] text-white font-bold hover:opacity-90 transition-opacity"
        >
          {saving ? "Actualizando..." : "Actualizar Contraseña"}
        </Button>
      </div>
    </Section>
  );
}