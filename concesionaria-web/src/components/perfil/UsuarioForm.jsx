import { User, Mail } from "lucide-react";

import { Button } from "@/components/ui/button";

import { AppInput } from "@/components/ui/custom/AppInput";

import { Section } from "./Section";

export function PerfilUsuarioSection({ form, updateField, onSave, saving, errors }) {
  
  return (
    <Section title="Perfil de Usuario" icon={User}>
      <div className="space-y-4">
        <AppInput
          label="Nombre Completo"
          value={form.nombreCompleto.toUpperCase()}
          onChange={(e) => updateField("nombreCompleto", e.target.value)}
          icon={User}
        />
        {errors?.nombreCompleto && (
          <p className="text-red-500 text-sm font-medium mt-1">
            {errors.nombreCompleto[0]}
          </p>
        )}

        <AppInput label="Email" value={form.email} disabled icon={Mail} />

        <Button
          onClick={onSave}
          disabled={saving}
          className="w-full rounded-lg bg-[hsl(var(--nav-bg))] text-white font-bold"
        >
          {saving ? "Guardando..." : "Actualizar Usuario"}
        </Button>
      </div>
    </Section>
  );
}
