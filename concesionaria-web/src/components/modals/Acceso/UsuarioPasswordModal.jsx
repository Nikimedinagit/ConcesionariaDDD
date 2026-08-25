/* eslint-disable react-hooks/set-state-in-effect */
import { useEffect, useState } from "react";
import { KeyRound } from "lucide-react";
import { usuarioPasswordSchema } from "@/validations/Acceso/usuario.validation";
import { ModalCustom } from "../ModalCustom";
import { AppInput } from "@/components/ui/custom/AppInput";

export function UsuarioPasswordModal({
  isOpen,
  onClose,
  onSave,
  usuario = null,
  loading = false,
  serverError = "",
}) {
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [localError, setLocalError] = useState("");

  useEffect(() => {
    setPassword("");
    setConfirmPassword("");
    setLocalError("");
  }, [usuario, isOpen]);

  const handleSave = async () => {
    const result = usuarioPasswordSchema.safeParse({ password });

    if (!result.success) {
      const errors = result.error.flatten().fieldErrors;
      setLocalError(errors.password?.[0] || "Error en la contraseña");
      return;
    }

    if (password !== confirmPassword) {
      setLocalError("Las contraseñas no coinciden.");
      return;
    }

    setLocalError("");
    onSave({
      usuarioId: usuario?.usuarioId,
      password,
    });
  };

  return (
    <ModalCustom
      isOpen={isOpen}
      onClose={onClose}
      onSave={handleSave}
      title="Cambiar Contraseña"
      icon={KeyRound}
      loading={loading}
      saveText="Actualizar"
    >
      <div className="space-y-3">
        <AppInput
          label="Nueva Contraseña *"
          icon={KeyRound}
          type="password"
          placeholder="Mínimo 6 caracteres"
          value={password}
          passwordToggle
          onChange={(e) => {
            setPassword(e.target.value);
            if (localError) setLocalError("");
          }}
          error={localError || serverError}
          autoFocus
        />
        <AppInput
          label="Confirmar Contraseña *"
          icon={KeyRound}
          type="password"
          placeholder="Repetí la nueva contraseña"
          value={confirmPassword}
          onChange={(e) => {
            setConfirmPassword(e.target.value);
            if (localError) setLocalError("");
          }}
          passwordToggle
          error={
            password !== confirmPassword && confirmPassword
              ? "Las contraseñas no coinciden."
              : undefined
          }
        />
      </div>
    </ModalCustom>
  );
}
