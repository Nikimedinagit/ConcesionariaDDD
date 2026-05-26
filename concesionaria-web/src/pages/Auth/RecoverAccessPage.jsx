import { useState } from "react";
import { Link } from "react-router-dom";
import {
  ArrowRight,
  Mail,
  Smartphone,
  KeyRound,
  LockKeyhole,
} from "lucide-react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { recuperarAccesoSchema } from "@/validations/authSchemas";
import {
  solicitarCodigo,
  validarCodigo,
  cambiarPassword,
} from "@/services/authService";

const MIN_PASSWORD_LENGTH = 6;

export default function RecoverAccessPage() {
  const [step, setStep] = useState("REQUEST"); 
  const [contact, setContact] = useState("");
  const [code, setCode] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");

  const [loading, setLoading] = useState(false);
  const [apiError, setApiError] = useState("");
  const [successMessage, setSuccessMessage] = useState("");

  const passwordError =
    password.length > 0 && password.length < MIN_PASSWORD_LENGTH;
  const confirmPasswordError =
    confirmPassword.length > 0 && password !== confirmPassword;

  const isResetValid =
    password.length >= MIN_PASSWORD_LENGTH &&
    confirmPassword.length >= MIN_PASSWORD_LENGTH &&
    password === confirmPassword;

  async function handleSubmit(event) {
    event.preventDefault();
    setApiError("");
    setSuccessMessage("");
    setLoading(true);

    try {
      if (step === "REQUEST") {
        const result = recuperarAccesoSchema.safeParse({ contact });
        if (!result.success) {
          setApiError("Ingresá un email o número de teléfono válido.");
          setLoading(false);
          return;
        }

        await solicitarCodigo({ contacto: contact });
        setSuccessMessage("Código enviado. Revisá tu Email o SMS.");
        setStep("VERIFY");
      } else if (step === "VERIFY") {
        const token = await validarCodigo({ contacto: contact, codigo: code });
        localStorage.setItem("resetToken", token);
        setSuccessMessage("Código confirmado. Elegí tu nueva contraseña.");
        setStep("RESET");
      } else if (step === "RESET") {
        if (!isResetValid) {
          setApiError(
            "Verificá que las contraseñas coincidan y tengan al menos 6 caracteres.",
          );
          setLoading(false);
          return;
        }

        await cambiarPassword({
          token: localStorage.getItem("resetToken"),
          password: password,
          contacto: contact,
        });
        setSuccessMessage("¡Contraseña actualizada con éxito!");
        setTimeout(() => (window.location.href = "/"), 2000);
      }
    } catch (err) {
      setApiError(
        err?.response?.data?.message || "No pudimos procesar la solicitud.",
      );
    } finally {
      setLoading(false);
    }
  }

  return (
    <div className="rounded-[2rem] border border-slate-200 bg-white p-6 sm:p-8 shadow-sm w-full max-w-[500px] mx-auto">
      <div className="mb-5">
        <p className="text-sm uppercase tracking-[0.24em] text-slate-500">
          Recuperación
        </p>
        <h2 className="mt-4 text-3xl font-black tracking-tight text-slate-900">
          {step === "REQUEST"
            ? "Ayuda y recuperación"
            : step === "VERIFY"
              ? "Ingresá el código"
              : "Nueva contraseña"}
        </h2>
        <p className="mt-3 text-sm leading-relaxed text-slate-500">
          {step === "REQUEST"
            ? "Ingresá tu correo o número registrado y te enviaremos un código de verificación."
            : step === "VERIFY"
              ? "Escribí el código que recibiste para validar tu identidad."
              : "Escribí tu nueva contraseña de acceso."}
        </p>
      </div>

      {apiError && (
        <div className="mb-4 rounded-2xl border border-rose-200 bg-rose-50 px-4 py-3 text-sm text-rose-700">
          {apiError}
        </div>
      )}

      {successMessage && (
        <div className="mb-4 rounded-2xl border border-emerald-200 bg-emerald-50 px-4 py-3 text-sm text-emerald-700">
          {successMessage}
        </div>
      )}

      <form className="space-y-4" onSubmit={handleSubmit}>
        <div className="space-y-1">
          <label className="text-sm font-medium text-slate-700">
            {step === "REQUEST"
              ? "Email o número de teléfono"
              : step === "VERIFY"
                ? "Código de verificación"
                : "Nueva contraseña"}
          </label>

          <div className="relative space-y-4">
            {step === "REQUEST" ? (
              <>
                <div className="relative">
                  <Smartphone className="absolute left-4 top-3.5 w-4 h-4 text-slate-400" />
                  <Input
                    value={contact}
                    onChange={(e) => setContact(e.target.value)}
                    placeholder="correo@empresa.com o +54..."
                    className="h-11 rounded-xl border border-slate-200 pl-11"
                  />
                </div>
              </>
            ) : step === "VERIFY" ? (
              <div className="relative">
                <KeyRound className="absolute left-4 top-3.5 w-4 h-4 text-slate-400" />
                <Input
                  value={code}
                  onChange={(e) => setCode(e.target.value)}
                  placeholder="******"
                  className="h-11 rounded-xl border border-slate-200 pl-11"
                />
              </div>
            ) : (
              <>
                <div className="relative">
                  <LockKeyhole className="absolute left-4 top-3.5 w-4 h-4 text-slate-400" />
                  <Input
                    type="password"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    placeholder="Nueva contraseña"
                    className="h-11 rounded-xl border border-slate-200 pl-11"
                  />
                  {passwordError && (
                    <p className="mt-1 text-sm text-rose-500">
                      Mínimo 6 caracteres.
                    </p>
                  )}
                </div>
                <div className="relative">
                  <LockKeyhole className="absolute left-4 top-3.5 w-4 h-4 text-slate-400" />
                  <Input
                    type="password"
                    value={confirmPassword}
                    onChange={(e) => setConfirmPassword(e.target.value)}
                    placeholder="Confirmar contraseña"
                    className="h-11 rounded-xl border border-slate-200 pl-11"
                  />
                  {confirmPasswordError && (
                    <p className="mt-1 text-sm text-rose-500">
                      Las contraseñas no coinciden.
                    </p>
                  )}
                </div>
              </>
            )}
          </div>
        </div>

        <Button
          type="submit"
          disabled={loading || (step === "RESET" && !isResetValid)}
          className="w-full h-11 rounded-2xl font-semibold text-white cursor-pointer mt-4 disabled:opacity-50"
          style={{ background: "hsl(var(--nav-bg))" }}
        >
          {loading
            ? "Procesando..."
            : step === "REQUEST"
              ? "Enviar código"
              : step === "VERIFY"
                ? "Validar acceso"
                : "Cambiar contraseña"}
          <ArrowRight className="ml-2 w-4 h-4" />
        </Button>

        {step === "VERIFY" && (
          <button
            type="button"
            onClick={() => {
              setStep("REQUEST");
              setApiError("");
              setSuccessMessage("");
              setCode("");
            }}
            className="w-full py-2 text-sm text-slate-500 hover:text-slate-900 transition-colors"
          >
            ¿No te llegó el código? <strong>Volver a solicitar</strong>
          </button>
        )}

        <div className="rounded-3xl border border-slate-200 bg-slate-50 p-4 text-sm text-slate-600 mt-2">
          <p className="font-semibold text-slate-900">¿Necesitás ayuda?</p>
          <div className="mt-3 grid gap-2 sm:grid-cols-2">
            <Button
              asChild
              variant="outline"
              className="rounded-2xl border-red-200 text-red-600 hover:bg-red-50 hover:text-red-700"
            >
              <a href="mailto:ignaciomedina333@gmail.com">
                <Mail className="mr-2 h-4 w-4" /> Email
              </a>
            </Button>
            <Button
              asChild
              variant="outline"
              className="rounded-2xl border-emerald-200 text-emerald-600 hover:bg-emerald-50 hover:text-emerald-700"
            >
              <a
                href="https://wa.me/543562443758"
                target="_blank"
                rel="noreferrer"
              >
                <Smartphone className="mr-2 h-4 w-4" /> WhatsApp
              </a>
            </Button>
          </div>
        </div>

        <p className="text-center text-sm text-slate-500 pt-2">
          Volver a{" "}
          <Link to="/" className="font-semibold text-slate-900 hover:underline">
            Ingresar
          </Link>
        </p>
      </form>
    </div>
  );
}
