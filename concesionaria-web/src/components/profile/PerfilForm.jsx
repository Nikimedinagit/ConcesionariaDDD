import { useState } from "react";
import {
  Building2,
  User,
  LockKeyhole,
  Phone,
  Camera,
  Mail,
  DollarSign,
  Hash,
  MapPin,
  Tag,
} from "lucide-react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";

// 16 Avatares
const AVATARS = Array.from(
  { length: 16 },
  (_, i) => `/avatars/av-${i + 1}.png`,
);

export function PerfilForm() {
  const [form, setForm] = useState({
    razonSocial: "Concesionaria Santa Fe",
    nombreFantasia: "Santa Fe Motors",
    cuit: "20-12345678-9",
    localidadId: "Santa Fe",
    moneda: "ARS",
    estado: "Activo",
    nombreCompleto: "Ignacio Medina",
    email: "ignacio@concesionaria.com",
    paisCode: "+54",
    area: "",
    telefono: "",
    avatar: AVATARS[0],
  });

  const [localidadSearch, setLocalidadSearch] = useState("");
  const localidades = [
    { id: "Santa Fe", nombre: "Santa Fe" },
    { id: "Rosario", nombre: "Rosario" },
  ];

  const updateField = (field, value) =>
    setForm((prev) => ({ ...prev, [field]: value }));

  return (
    <div className="w-full mx-auto max-w-[1400px] py-8 space-y-8">
      {/* 1. EMPRESA */}
      <Section
        title="Información de la Empresa"
        icon={Building2}
        badge={form.estado}
      >
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 items-end">
          <div className="lg:col-span-2">
            <InputField
              label="Razón Social"
              value={form.razonSocial}
              disabled
              icon={Building2}
            />
          </div>
          <div className="lg:col-span-2">
            <InputField
              label="Nombre Fantasía"
              value={form.nombreFantasia}
              onChange={(v) => updateField("nombreFantasia", v)}
              icon={Tag}
            />
          </div>

          <InputField label="CUIT" value={form.cuit} disabled icon={Hash} />

          <div className="space-y-1.5">
            <label className="text-sm font-medium text-slate-700">
              Localidad
            </label>
            <Select
              value={form.localidadId}
              onValueChange={(v) => updateField("localidadId", v)}
            >
              <SelectTrigger className="h-[40px] rounded-lg border-slate-200 focus:ring-0 focus:ring-offset-0">
                <div className="flex items-center gap-2 truncate">
                  <MapPin className="w-4 h-4 text-slate-400" /> <SelectValue />
                </div>
              </SelectTrigger>
              <SelectContent className="bg-white border border-slate-200 shadow-lg">
                <div className="px-2 pb-2">
                  <Input
                    placeholder="Buscar..."
                    onChange={(e) => setLocalidadSearch(e.target.value)}
                    className="h-8"
                  />
                </div>
                {localidades
                  .filter((l) =>
                    l.nombre
                      .toLowerCase()
                      .includes(localidadSearch.toLowerCase()),
                  )
                  .map((l) => (
                    <SelectItem key={l.id} value={l.id}>
                      {l.nombre}
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
              <SelectTrigger className="h-[40px] rounded-lg border-slate-200 focus:ring-0 focus:ring-offset-0">
                <div className="flex items-center gap-2">
                  <DollarSign className="w-4 h-4 text-slate-400" /> <SelectValue />
                </div>
              </SelectTrigger>
              <SelectContent className="bg-white border border-slate-200 text-slate-900">
                <SelectItem value="ARS">ARS</SelectItem>
                <SelectItem value="USD">USD</SelectItem>
                <SelectItem value="BRL">BRL</SelectItem>
              </SelectContent>
            </Select>
          </div>
            <Button
              style={{ background: "hsl(var(--nav-bg))" }}
              className="h-[40px] rounded-lg text-white font-bold px-8"
            >
              Guardar Cambios de Empresa
            </Button>
          </div>
      </Section>

      {/* 2. USUARIO Y SEGURIDAD */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-8">
        <Section title="Perfil de Usuario" icon={User}>
          <div className="space-y-4">
            <InputField
              label="Nombre Completo"
              value={form.nombreCompleto}
              onChange={(v) => updateField("nombreCompleto", v)}
              icon={User}
            />
            <InputField label="Email" value={form.email} disabled icon={Mail} />
            <Button
              style={{ background: "hsl(var(--nav-bg))" }}
              className="w-full rounded-lg text-white font-bold"
            >
              Guardar Datos de Usuario
            </Button>
          </div>
        </Section>

        <Section title="Seguridad" icon={LockKeyhole}>
          <div className="space-y-4">
            <InputField
              label="Contraseña Actual"
              type="password"
              placeholder="••••••••"
              icon={LockKeyhole}
            />
            <InputField
              label="Nueva Contraseña"
              type="password"
              placeholder="••••••••"
              icon={LockKeyhole}
            />
            <Button
              style={{ background: "hsl(var(--nav-bg))" }}
              className="w-full rounded-lg text-white font-bold"
            >
              Actualizar Contraseña
            </Button>
          </div>
        </Section>
      </div>

      {/* 3. RECUPERACIÓN */}
      <Section title="Recuperación de Acceso" icon={Phone}>
        <div className="grid grid-cols-1 md:grid-cols-4 gap-6 items-end">
          <div className="space-y-1.5">
            <label className="text-sm font-medium text-slate-700">País</label>
            <Select
              value={form.paisCode}
              onValueChange={(v) => updateField("paisCode", v)}
            >
              <SelectTrigger className="h-[44px] rounded-lg border-slate-200 focus:ring-0 focus:ring-offset-0">
                <SelectValue />
              </SelectTrigger>
              <SelectContent className="bg-white border-slate-200 shadow-lg">
                <SelectItem value="+54">+54 (Argentina)</SelectItem>
              </SelectContent>
            </Select>
          </div>
          <InputField
            label="Cód. Área (sin 0)"
            placeholder="342"
            onChange={(v) => updateField("area", v)}
            icon={Hash}
          />
          <InputField
            label="Número (sin 15)"
            placeholder="1234567"
            onChange={(v) => updateField("telefono", v)}
            icon={Phone}
          />
          <Button
            style={{ background: "hsl(var(--nav-bg))" }}
            className="h-[44px] rounded-lg text-white font-bold"
          >
            Guardar Teléfono
          </Button>
        </div>
      </Section>

      {/* 4. AVATAR */}
      <Section title="Imagen de Perfil" icon={Camera}>
        <div className="flex flex-wrap gap-4 d-flex items-center justify-center">
          {AVATARS.map((src, index) => (
            <button
              key={index}
              type="button"
              onClick={() => updateField("avatar", src)}
              className={`relative rounded-full p-1 transition-all ${form.avatar === src ? "ring-2 ring-slate-900 ring-offset-2" : "hover:scale-105"}`}
            >
              <img src={src} className="rounded-full w-12 h-12 object-cover" />
            </button>
          ))}
        </div>
      </Section>
    </div>
  );
}

function Section({ title, icon: Icon, children, badge }) {
  return (
    <div className="bg-white p-6 rounded-xl border border-slate-200 shadow-sm">
      <div className="flex justify-between items-center mb-6">
        <h3 className="text-lg font-bold text-slate-900 flex items-center gap-2 uppercase tracking-tight">
          <Icon className="w-5 h-5 text-slate-400" /> {title}
        </h3>
        {badge && (
          <span className="flex items-center gap-1.5 px-3 py-1 rounded-full bg-emerald-50 text-emerald-700 text-xs font-bold uppercase tracking-wide border border-emerald-200">
            <span className="w-1.5 h-1.5 rounded-full bg-emerald-500 animate-pulse"></span>
            {badge}
          </span>
        )}
      </div>
      {children}
    </div>
  );
}

function InputField({
  label,
  value,
  onChange,
  disabled,
  type = "text",
  placeholder,
  icon: Icon,
  className,
}) {
  return (
    <label className="block text-sm font-medium text-slate-700">
      {label}
      <div className="relative mt-2">
        <Icon className="absolute left-3 top-3.5 w-4 h-4 text-slate-400" />
        <Input
          type={type}
          value={value}
          disabled={disabled}
          onChange={onChange ? (e) => onChange(e.target.value) : undefined}
          placeholder={placeholder}
          className={`h-[40px] w-full rounded-lg border border-slate-200 bg-white pl-10 pr-4 focus-visible:ring-1 focus-visible:ring-slate-900 ${disabled ? "bg-slate-50 text-slate-500 cursor-not-allowed" : ""} ${className}`}
        />
      </div>
    </label>
  );
}
