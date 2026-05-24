import { Camera } from "lucide-react";

import { Section } from "./Section";

const AVATARS = Array.from(
  { length: 16 },
  (_, i) => `/avatars/av-${i + 1}.png`,
);

export function PerfilAvatarSection({
  form,
  updateField,
}) {
  return (
    <Section title="Imagen de Perfil" icon={Camera}>
      <div className="flex flex-wrap gap-4 items-center justify-center">
        {AVATARS.map((src, index) => (
          <button
            key={index}
            type="button"
            onClick={() =>
              updateField("avatar", src)
            }
            className={`relative rounded-full p-1 transition-all ${
              form.avatar === src
                ? "ring-2 ring-slate-900 ring-offset-2"
                : "hover:scale-105"
            }`}
          >
            <img
              src={src}
              className="rounded-full w-12 h-12 object-cover"
            />
          </button>
        ))}
      </div>
    </Section>
  );
}