import { LockKeyhole } from "lucide-react";

import { Button } from "@/components/ui/button";

import { AppInput } from "@/components/ui/custom/AppInput";

import { Section } from "./Section";

export function PerfilSeguridadSection() {
  return (
    <Section title="Seguridad" icon={LockKeyhole}>
      <div className="space-y-4">
        <AppInput
          label="Contraseña Actual"
          type="password"
          placeholder="••••••••"
          icon={LockKeyhole}
        />

        <AppInput
          label="Nueva Contraseña"
          type="password"
          placeholder="••••••••"
          icon={LockKeyhole}
        />

        <Button className="w-full rounded-lg bg-[hsl(var(--nav-bg))] text-white font-bold">
          Actualizar Contraseña
        </Button>
      </div>
    </Section>
  );
}