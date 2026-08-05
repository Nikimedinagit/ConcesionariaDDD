import { useState } from "react";
import { Landmark, Layers, Tag } from "lucide-react";
import { ModalCustom } from "./ModalCustom";
import { AppInput } from "@/components/ui/custom/AppInput";
import { AppSelect } from "@/components/ui/custom/AppSelect";

const tiposCuenta = [
  { value: "1", label: "ACTIVO" },
  { value: "2", label: "PASIVO" },
  { value: "3", label: "PATRIMONIO" },
  { value: "4", label: "INGRESO" },
  { value: "5", label: "EGRESO" },
];

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
  const [tipo, setTipo] = useState(cuenta ? String(cuenta.tipo) : "1");
  const [localError, setLocalError] = useState("");

  const isEdit = Boolean(cuenta);
  const isRootCreate = !isEdit && !cuentaPadre;
  const errorToShow = localError || serverError;

  const handleSave = () => {
    const nombreNormalizado = nombre.trim().toUpperCase();

    if (!nombreNormalizado) {
      setLocalError("El nombre es obligatorio.");
      return;
    }

    if (nombreNormalizado.length < 3) {
      setLocalError("El nombre debe tener al menos 3 caracteres.");
      return;
    }

    setLocalError("");
    onSave({
      nombre: nombreNormalizado,
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
        <div className="rounded-lg border border-slate-200 bg-slate-50 px-3 py-2">
          <p className="text-xs font-semibold uppercase text-slate-500">
            Nueva cuenta nivel 0
          </p>
          <p className="mt-1 text-sm text-slate-700">
            El código se asigna automáticamente al guardar.
          </p>
        </div>
      )}

      {cuentaPadre && (
        <div className="rounded-lg border border-slate-200 bg-slate-50 px-3 py-2">
          <p className="text-xs font-semibold uppercase text-slate-500">
            Cuenta padre
          </p>
          <div className="mt-1 flex flex-wrap items-center gap-2 text-sm text-slate-800">
            <span className="font-bold">{cuentaPadre.codigo}</span>
            <span>{cuentaPadre.nombre}</span>
            <span className="rounded-md bg-white px-2 py-0.5 text-xs font-semibold text-slate-500">
              Nivel {cuentaPadre.nivel}
            </span>
          </div>
        </div>
      )}

      {cuenta && (
        <div className="rounded-lg border border-slate-200 bg-slate-50 px-3 py-2">
          <p className="text-xs font-semibold uppercase text-slate-500">
            Cuenta
          </p>
          <div className="mt-1 flex flex-wrap items-center gap-2 text-sm text-slate-800">
            <span className="font-bold">{cuenta.codigo}</span>
            <span className="rounded-md bg-white px-2 py-0.5 text-xs font-semibold text-slate-500">
              {cuenta.tipo}
            </span>
          </div>
        </div>
      )}

      {isRootCreate && (
        <AppSelect
          label="Tipo *"
          icon={Layers}
          value={tipo}
          onValueChange={setTipo}
          options={tiposCuenta}
          placeholder="Seleccione un tipo"
        />
      )}

      <AppInput
        label="Nombre *"
        icon={Tag}
        placeholder="Ej: BANCO NACION, CAJA CHICA..."
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
