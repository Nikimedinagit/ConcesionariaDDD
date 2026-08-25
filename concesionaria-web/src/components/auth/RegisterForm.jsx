import { useEffect, useState } from "react";
import { Link } from "react-router-dom";

import {
  ArrowRight,
  Building2,
  DollarSign,
  Hash,
  LockKeyhole,
  MapPin,
  Mail,
  Tag,
  User,
} from "lucide-react";

import { Button } from "@/components/ui/button";
import { AppInput } from "@/components/ui/custom/AppInput";
import { AppSearchSelect, AppSelect } from "@/components/ui/custom/AppSelect";
import { getLocalidades } from "@/services/Ubicacion/localidadService";
import { registerRequest } from "@/services/Auth/authService";
import { empresaSchema, usuarioSchema } from "@/validations/Auth/authSchemas";

const stepLabels = ["Empresa", "Personal", "Confirmación"];

export function RegisterForm() {
  const [step, setStep] = useState(1);
  const [form, setForm] = useState({
    razonSocial: "",
    nombreFantasia: "",
    cuit: "",
    localidadId: "",
    moneda: "",
    nombreCompleto: "",
    email: "",
    password: "",
    confirmPassword: "",
    aceptoTerminos: false,
  });
  const [localidades, setLocalidades] = useState([]);
  const [apiError, setApiError] = useState("");
  const [successMessage, setSuccessMessage] = useState("");
  const [isSubmitting, setIsSubmitting] = useState(false);

  const empresaData = {
    razonSocial: form.razonSocial,
    nombreFantasia: form.nombreFantasia,
    cuit: form.cuit,
    localidadId: form.localidadId,
    moneda: form.moneda,
  };

  const usuarioData = {
    nombre: form.nombreCompleto,
    email: form.email,
    password: form.password,
  };

  const cuitDigits = form.cuit.replace(/\D/g, "");
  const cuitError = form.cuit.length > 0 && !/^\d{11}$/.test(cuitDigits);
  const emailError =
    form.email.length > 0 &&
    !usuarioSchema.shape.email.safeParse(form.email).success;
  const passwordError = form.password.length > 0 && form.password.length < 6;
  const confirmPasswordError =
    form.confirmPassword.length > 0 && form.password !== form.confirmPassword;

  const stepOneValid = empresaSchema.safeParse(empresaData).success;
  const stepTwoValid =
    usuarioSchema.safeParse(usuarioData).success &&
    form.password === form.confirmPassword &&
    form.aceptoTerminos;

  function updateField(field, value) {
    setForm((current) => ({ ...current, [field]: value }));
  }

  useEffect(() => {
    getLocalidades()
      .then((data) => setLocalidades(data))
      .catch((error) => {
        console.error("Error cargando localidades:", error);
      });
  }, []);
  function handleNext(event) {
    event.preventDefault();
    setApiError("");

    if (!stepOneValid) {
      setApiError(
        "Completa todos los datos de empresa y selecciona una localidad válida.",
      );
      return;
    }

    setStep((current) => Math.min(3, current + 1));
  }

  function handleBack(event) {
    event.preventDefault();
    setApiError("");
    setStep((current) => Math.max(1, current - 1));
  }

  async function handleSubmit(event) {
    event.preventDefault();
    setApiError("");
    setSuccessMessage("");

    if (!stepTwoValid) {
      setApiError("Completa todos los datos de usuario y acepta los términos.");
      return;
    }

    setIsSubmitting(true);

    try {
      const response = await registerRequest({
        razonSocial: form.razonSocial,
        nombreFantasia: form.nombreFantasia,
        cuit: cuitDigits,
        localidadId: form.localidadId,
        moneda: form.moneda,
        nombreCompleto: form.nombreCompleto,
        email: form.email,
        password: form.password,
      });

      setSuccessMessage(response.message ?? "Registro enviado correctamente.");
      setStep(3);
    } catch (error) {
      const responseData = error?.response?.data;
      const backendMessage = responseData?.message;
      const errorsArray = responseData?.errors;
      let message = "Ocurrió un error al registrar.";

      if (errorsArray?.length) {
        message = errorsArray.join(" ");
      } else if (typeof responseData === "object" && responseData !== null) {
        const modelStateErrors = Object.values(responseData)
          .flat()
          .filter((item) => typeof item === "string");
        if (modelStateErrors.length) {
          message = modelStateErrors.join(" ");
        }
      }

      const duplicateMessage =
        backendMessage === "Ya existe un usuario con ese email." ||
        backendMessage === "El email o CUIT ya está en uso.";

      if (duplicateMessage) {
        message = "Ya existe una cuenta con ese email o CUIT.";
      } else if (backendMessage && typeof backendMessage === "string") {
        message = backendMessage;
      }

      setApiError(message);
    } finally {
      setIsSubmitting(false);
    }
  }

  return (
    <div className="w-full max-w-full rounded-[2rem] border border-slate-200 bg-white p-6 sm:p-8 shadow-sm lg:max-w-[1080px] mx-auto">
      <div className="mb-5">
        <p className="text-sm uppercase tracking-[0.24em] text-slate-500">
          Registro
        </p>

        <h2 className="mt-4 text-3xl font-black tracking-tight text-slate-900 sm:text-4xl">
          Crear tu espacio de gestión
        </h2>

        <p className="mt-3 text-slate-500">
          Completá los datos de tu empresa y tu usuario en dos pasos.
        </p>
      </div>

      <div className="mb-5 flex flex-wrap justify-center gap-2 text-sm font-semibold text-slate-500">
        {stepLabels.map((label, index) => (
          <span
            key={label}
            className={`rounded-full px-3 py-1.5 transition ${
              step === index + 1 ? "bg-slate-900 text-white" : "bg-slate-100"
            }`}
          >
            {index + 1}. {label}
          </span>
        ))}
      </div>

      {apiError ? (
        <div className="mb-4 rounded-2xl border border-rose-200 bg-rose-50 px-4 py-3 text-sm text-rose-700">
          {apiError}
        </div>
      ) : null}
      {successMessage ? (
        <div className="mb-4 rounded-2xl border border-emerald-200 bg-emerald-50 px-4 py-3 text-sm text-emerald-700">
          {successMessage}
        </div>
      ) : null}

      <form
        className="space-y-3"
        onSubmit={step === 2 ? handleSubmit : handleNext}
      >
        {step === 1 && (
          <div className="space-y-3">
            <AppInput label="Razón Social *" icon={Building2} value={form.razonSocial} onChange={(event) => updateField("razonSocial", event.target.value)} placeholder="Ej. Concesionaria Santa Fe" />

            <div className="grid gap-1 sm:grid-cols-2">
              <AppInput label="Nombre Fantasía *" icon={Tag} value={form.nombreFantasia} onChange={(event) => updateField("nombreFantasia", event.target.value)} placeholder="Ej. Santa Fe Motors" />

              <AppInput label="CUIT *" icon={Hash} value={form.cuit} onChange={(event) => updateField("cuit", event.target.value)} placeholder="20-12345678-9" error={cuitError ? "CUIT debe tener 11 dígitos." : undefined} />

              <AppSearchSelect label="Localidad *" icon={MapPin} value={form.localidadId} onValueChange={(value) => updateField("localidadId", value)} options={localidades} optionValue="id" optionLabel="nombre" placeholder="Localidad" searchPlaceholder="Buscar localidad" />

              <AppSelect label="Moneda Principal *" icon={DollarSign} value={form.moneda} onValueChange={(value) => updateField("moneda", value)} options={[{ value: "ARS", label: "ARS" }, { value: "USD", label: "USD" }, { value: "BRL", label: "BRL" }]} placeholder="Moneda Principal" />
            </div>
          </div>
        )}

        {step === 2 && (
          <div className="space-y-4">
            <div className="grid gap-2 sm:grid-cols-2">
              <AppInput label="Nombre Completo *" icon={User} value={form.nombreCompleto} onChange={(event) => updateField("nombreCompleto", event.target.value)} placeholder="Ej. Juan Pérez" />

              <div>
                <AppInput
                  label="Email *"
                  icon={Mail}
                  value={form.email}
                  onChange={(event) =>
                    updateField("email", event.target.value.toLowerCase())
                  }
                  placeholder="correo@empresa.com"
                  error={emailError ? "Email debe contener @ y ." : undefined}
                />
              </div>
            </div>

            <div className="grid gap-2 sm:grid-cols-2">
              <AppInput label="Contraseña *" icon={LockKeyhole} type="password" value={form.password} onChange={(event) => updateField("password", event.target.value)} placeholder="••••••••" error={passwordError ? "Contraseña mínimo 6 caracteres." : undefined} />

              <AppInput label="Confirmar contraseña *" icon={LockKeyhole} type="password" value={form.confirmPassword} onChange={(event) => updateField("confirmPassword", event.target.value)} placeholder="••••••••" error={confirmPasswordError ? "Las contraseñas no coinciden." : undefined} />
            </div>

            <label className="flex cursor-pointer items-center gap-3 rounded-2xl border border-slate-200 bg-white px-4 py-4 text-sm text-slate-700">
              <input
                type="checkbox"
                checked={form.aceptoTerminos}
                onChange={(event) =>
                  updateField("aceptoTerminos", event.target.checked)
                }
                className="h-4 w-4 rounded border-slate-300 text-slate-900 focus:ring-slate-900"
              />
              Acepto términos y condiciones.
            </label>
          </div>
        )}

        {step === 3 && (
          <div className="space-y-5 text-slate-700">
            <div className="flex items-center gap-3 text-slate-900">
              <span className="inline-flex h-3 w-3 rounded-full bg-slate-900" />
              <h3 className="text-lg font-semibold">¡Solicitud recibida!</h3>
            </div>

            <p className="text-sm leading-relaxed text-slate-600">
              Gracias por registrarte. Ya recibimos los datos de tu empresa y tu
              usuario. Te avisaremos cuando el pago sea recibido o si
              necesitamos información adicional.
            </p>

            <div className="grid gap-3 sm:grid-cols-2 text-sm text-slate-600">
              <div>
                <p className="font-semibold text-slate-900">Empresa</p>
                <p>{form.razonSocial || "-"}</p>
                <p>{form.nombreFantasia || "-"}</p>
              </div>
              <div>
                <p className="font-semibold text-slate-900">Contacto</p>
                <p>{form.nombreCompleto || "-"}</p>
                <p>{form.email || "-"}</p>
              </div>
            </div>

            <Button
              asChild
              className="w-full h-10 rounded-2xl font-semibold text-white"
              style={{ background: "hsl(var(--nav-bg))" }}
            >
              <Link to="/">Ir a Ingresar</Link>
            </Button>
          </div>
        )}

        {step !== 3 && (
          <div className="flex flex-wrap justify-center gap-3 pt-2">
            {step > 1 ? (
              <button
                onClick={handleBack}
                className="inline-flex h-10 items-center justify-center rounded-2xl border border-slate-200 bg-white px-5 text-sm font-semibold text-slate-800 transition hover:bg-slate-50 cursor-pointer hover:shadow-sm transition-shadow"
              >
                Volver
              </button>
            ) : null}

            <Button
              type="submit"
              disabled={
                step === 1 ? !stepOneValid : !stepTwoValid || isSubmitting
              }
              className="h-10 rounded-2xl px-6 font-semibold text-white cursor-pointer hover:shadow-lg transition-shadow disabled:cursor-not-allowed disabled:opacity-50"
              style={{ background: "hsl(var(--nav-bg))" }}
            >
              {step === 1 ? "Siguiente" : isSubmitting ? "Creando..." : "Crear"}
              <ArrowRight className="ml-2 w-4 h-4" />
            </Button>
          </div>
        )}

        {step !== 3 && (
          <p className="text-center text-sm text-slate-500">
            ¿Ya tenés cuenta?{" "}
            <Link
              to="/"
              className="font-semibold text-slate-900 hover:text-slate-700 transition-colors underline-offset-4 hover:underline"
            >
              Ingresar
            </Link>
          </p>
        )}
      </form>
    </div>
  );
}
