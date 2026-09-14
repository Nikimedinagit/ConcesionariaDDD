import React, { useState, useEffect } from "react";
import { categoriaSchema } from "@/validations/Tesoreria/categoria.validation";
import { ModalCustom } from "../ModalCustom";
import { AppInput } from "@/components/ui/custom/AppInput";
import { Tag } from "lucide-react";

export function CategoriaGastoModal({
  isOpen,
  onClose,
  onSave,
  categoria = null,
  loading = false,
  serverError = "",
}) {
  const [nombre, setNombre] = useState("");
  const [localError, setLocalError] = useState("");

  const errorToShow = localError || serverError;

  useEffect(() => {
    setNombre(categoria ? categoria.nombre : "");
    setLocalError("");
  }, [categoria, isOpen]);

  const handleSave = async () => {
    const result = categoriaSchema.safeParse({ nombre });

    if (!result.success) {
      const fieldErrors = result.error.flatten().fieldErrors;
      const firstErrorMessage =
        fieldErrors.nombre?.[0] || "Error en el campo nombre";
      setLocalError(firstErrorMessage);
      return;
    }

    setLocalError("");

    onSave({
      categoriaGastoId: categoria?.categoriaGastoId, 
      nombre,
    });
  };

  return (
    <ModalCustom
      isOpen={isOpen}
      onClose={onClose}
      onSave={handleSave}
      title={categoria ? "Editar Categoría" : "Nueva Categoría"}
      icon={Tag}
      loading={loading}
      saveText={categoria ? "Actualizar" : "Guardar"}
    >
      <div className="space-y-2">
        <AppInput
          label="Nombre *"
          icon={Tag}
          placeholder="Ej: GASTOS DE LIMPIEZA"
          value={nombre}
          onChange={(e) => {
            setNombre(e.target.value.toUpperCase());
            if (localError) setLocalError("");
          }}
          error={errorToShow}
          autoFocus
        />
      </div>
    </ModalCustom>
  );
}
