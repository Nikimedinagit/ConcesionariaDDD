import { User, Mail } from "lucide-react";

import { Button } from "@/components/ui/button";

import { AppInput } from "@/components/ui/custom/AppInput";

import { Section } from "./Section";

export function PerfilUsuarioSection({ form, updateField, onSave, saving }) {
  return (
    <Section title="Perfil de Usuario" icon={User}>
      <div className="space-y-4">
        <AppInput
          label="Nombre Completo"
          value={form.nombreCompleto}
          onChange={(e) => updateField("nombreCompleto", e.target.value)}
          icon={User}
        />

        <AppInput label="Email" value={form.email} disabled icon={Mail} />

        <Button
          onClick={onSave}
          disabled={saving}
          className="w-full rounded-lg bg-[hsl(var(--nav-bg))] text-white font-bold"
        >
          {saving ? "Guardando..." : "Guardar Usuario"}
        </Button>
      </div>
    </Section>
  );
}
