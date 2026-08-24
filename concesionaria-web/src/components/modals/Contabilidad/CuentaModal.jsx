import { useState } from "react";
import {
  GitBranch,
  Info,
  Landmark,
  Layers,
  PencilLine,
  Tag,
} from "lucide-react";
import { ModalCustom } from "../ModalCustom";
import { AppInput } from "@/components/ui/custom/AppInput";
import { AppSelect } from "@/components/ui/custom/AppSelect";
import {
  cuentaRaizSchema,
  cuentaSchema,
} from "@/validations/Contabilidad/cuenta.validation";

const tiposCuenta = [
  { value: "1", label: "ACTIVO" },
  { value: "2", label: "PASIVO" },
  { value: "3", label: "PATRIMONIO" },
  { value: "4", label: "INGRESO" },
  { value: "5", label: "EGRESO" },
];

const getTipoCuentaLabel = (tipo) =>
  tiposCuenta.find((item) => item.value === String(tipo))?.label ?? tipo;

export function CuentaModal({
  isOpen,
  onClose,
  onSave,
  cuenta = null,
  cuentaPadre = null,
  loading = false,
  serverError = "",
}) {
  const [nombre, setNombre] = useState(cuenta ? cuenta.nombre : "");
  const [tipo, setTipo] = useState(cuenta ? String(cuenta.tipo) : "");
  const [tipoError, setTipoError] = useState("");
  const [localError, setLocalError] = useState("");

  const isEdit = Boolean(cuenta);
  const isRootCreate = !isEdit && !cuentaPadre;
  const errorToShow = localError || serverError;

  const handleSave = () => {
    const schema = isRootCreate ? cuentaRaizSchema : cuentaSchema;
    const result = schema.safeParse({ nombre, tipo });

    if (!result.success) {
      const fieldErrors = result.error.flatten().fieldErrors;
      setLocalError(fieldErrors.nombre?.[0] || "");
      setTipoError(fieldErrors.tipo?.[0] || "");
      return;
    }

    setLocalError("");
    setTipoError("");
    onSave({
      nombre: result.data.nombre.toUpperCase(),
      tipo: Number(tipo),
    });
  };

  return (
    <ModalCustom
      isOpen={isOpen}
      onClose={onClose}
      onSave={handleSave}
      title={isEdit ? "Editar Cuenta" : "Nueva Cuenta"}
      icon={Landmark}
      loading={loading}
      saveText={isEdit ? "Actualizar" : "Agregar"}
      maxWidth="max-w-xl"
    >
      {isRootCreate && (
        <div className="overflow-hidden rounded-xl border border-[hsl(var(--nav-bg)/0.18)] bg-[hsl(var(--nav-bg)/0.05)]">
          <div className="flex items-center gap-3 px-3 py-3">
            <div className="flex h-9 w-9 shrink-0 items-center justify-center rounded-lg bg-[hsl(var(--nav-bg)/0.11)] text-[hsl(var(--nav-bg))]">
              <Landmark className="h-4 w-4" />
            </div>

            <div className="min-w-0 flex-1">
              <p className="text-[11px] font-bold uppercase tracking-wide text-[hsl(var(--nav-bg))]">
                Cuenta principal
              </p>
              <p className="mt-0.5 text-sm font-semibold text-slate-800">
                Se creará en el nivel raíz del plan de cuentas
              </p>
            </div>

            <div className="shrink-0 rounded-lg border border-slate-200 bg-white px-3 py-1.5 text-center shadow-sm">
              <p className="text-[10px] font-semibold uppercase text-slate-500">
                Nivel
              </p>
              <p className="text-base font-bold leading-tight text-slate-800">
                0
              </p>
            </div>
          </div>

          <div className="flex items-center gap-2 border-t border-[hsl(var(--nav-bg)/0.12)] bg-white/55 px-3 py-1.5 text-xs font-medium text-slate-600">
            <Info className="h-3.5 w-3.5 shrink-0 text-[hsl(var(--nav-bg))]" />
            El código se asignará automáticamente al guardar.
          </div>
        </div>
      )}

      {cuentaPadre && (
        <div className="overflow-hidden rounded-xl border border-[hsl(var(--nav-bg)/0.18)] bg-[hsl(var(--nav-bg)/0.05)]">
          <div className="flex items-center gap-3 px-3 py-3">
            <div className="flex h-9 w-9 shrink-0 items-center justify-center rounded-lg bg-[hsl(var(--nav-bg)/0.11)] text-[hsl(var(--nav-bg))]">
              <GitBranch className="h-4 w-4" />
            </div>

            <div className="min-w-0 flex-1">
              <p className="text-[11px] font-bold uppercase tracking-wide text-[hsl(var(--nav-bg))]">
                Cuenta padre
              </p>
              <div className="mt-1 flex min-w-0 items-center gap-2">
                <span className="rounded-md border border-[hsl(var(--nav-bg)/0.16)] bg-white px-2 py-0.5 text-xs font-bold text-[hsl(var(--nav-bg))]">
                  {cuentaPadre.codigo}
                </span>
                <span className="truncate text-sm font-bold text-slate-800">
                  {cuentaPadre.nombre}
                </span>
              </div>
            </div>

            <div className="shrink-0 rounded-lg border border-slate-200 bg-white px-3 py-1.5 text-center shadow-sm">
              <p className="text-[10px] font-semibold uppercase text-slate-500">
                Nivel padre
              </p>
              <p className="text-base font-bold leading-tight text-slate-800">
                {cuentaPadre.nivel}
              </p>
            </div>
          </div>

          <div className="border-t border-[hsl(var(--nav-bg)/0.12)] bg-white/55 px-3 py-1.5 text-xs font-medium text-slate-600">
            La nueva cuenta se creará en el nivel{" "}
            <span className="font-bold text-[hsl(var(--nav-bg))]">
              {Number(cuentaPadre.nivel) + 1}
            </span>
          </div>
        </div>
      )}

      {cuenta && (
        <div className="overflow-hidden rounded-xl border border-[hsl(var(--nav-bg)/0.18)] bg-[hsl(var(--nav-bg)/0.05)]">
          <div className="flex items-center gap-3 px-3 py-3">
            <div className="flex h-9 w-9 shrink-0 items-center justify-center rounded-lg bg-[hsl(var(--nav-bg)/0.11)] text-[hsl(var(--nav-bg))]">
              <PencilLine className="h-4 w-4" />
            </div>

            <div className="min-w-0 flex-1">
              <p className="text-[11px] font-bold uppercase tracking-wide text-[hsl(var(--nav-bg))]">
                Cuenta a editar
              </p>
              <div className="mt-1 flex min-w-0 items-center gap-2">
                <span className="rounded-md border border-[hsl(var(--nav-bg)/0.16)] bg-white px-2 py-0.5 text-xs font-bold text-[hsl(var(--nav-bg))]">
                  {cuenta.codigo}
                </span>
                <span className="truncate text-sm font-bold text-slate-800">
                  {getTipoCuentaLabel(cuenta.tipo)}
                </span>
              </div>
            </div>

            <div className="shrink-0 rounded-lg border border-slate-200 bg-white px-3 py-1.5 text-center shadow-sm">
              <p className="text-[10px] font-semibold uppercase text-slate-500">
                Nivel
              </p>
              <p className="text-base font-bold leading-tight text-slate-800">
                {cuenta.nivel}
              </p>
            </div>
          </div>

          <div className="flex items-center gap-2 border-t border-[hsl(var(--nav-bg)/0.12)] bg-white/55 px-3 py-1.5 text-xs font-medium text-slate-600">
            <Info className="h-3.5 w-3.5 shrink-0 text-[hsl(var(--nav-bg))]" />
            Solo se modificará el nombre de la cuenta.
          </div>
        </div>
      )}

      {isRootCreate && (
        <AppSelect
          label="Tipo *"
          icon={Layers}
          value={tipo}
          onValueChange={(value) => {
            setTipo(value);
            setTipoError("");
          }}
          options={tiposCuenta}
          placeholder="SELECCIONE..."
          error={tipoError}
        />
      )}

      <AppInput
        label="Nombre *"
        icon={Tag}
        placeholder="Ej: BANCO NACIÓN"
        value={nombre}
        onChange={(e) => {
          setNombre(e.target.value.toUpperCase());
          if (localError) setLocalError("");
        }}
        error={errorToShow}
        autoFocus
      />
    </ModalCustom>
  );
}
