import { Camera } from "lucide-react";
import { Section } from "./Section";

const AVATARS = Array.from(
  { length: 18 },
  (_, i) => `/avatars/av-${i + 1}-optimized.webp`,
);

export function PerfilAvatarSection({ form, updateField, onSave }) {
  return (
    <Section title="Imagen de Perfil" icon={Camera}>
      <div className="flex flex-wrap gap-6 items-center justify-center p-4">
        {AVATARS.map((src, index) => (
          <button
            key={index}
            type="button"
            onClick={async () => {
              updateField("avatar", src);
              await onSave(src);
            }}
            className={`relative rounded-full p-1 transition-all outline-none ${
              form.avatar === src
                ? "ring-4 ring-[hsl(var(--nav-bg))] ring-offset-2" 
                : "hover:scale-105"
            }`}
          >
            <img 
              src={src} 
              alt={`Avatar ${index + 1}`}
              className="rounded-full w-24 h-24 object-cover" 
            />
          </button>
        ))}
      </div>
    </Section>
  );
}
