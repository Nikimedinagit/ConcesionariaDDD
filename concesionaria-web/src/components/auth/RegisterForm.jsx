import { useState } from "react"
import { Link } from "react-router-dom"

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
} from "lucide-react"

import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select"

const stepLabels = ["Empresa", "Personal", "Confirmación"]
const localidadOptions = ["Morteros", "Córdoba", "Rosario", "Villa María"]

export function RegisterForm() {
  const [step, setStep] = useState(1)
  const [form, setForm] = useState({
    razonSocial: "",
    nombreFantasia: "",
    cuit: "",
    localidad: "",
    moneda: "",
    nombreCompleto: "",
    email: "",
    password: "",
    confirmPassword: "",
    aceptoTerminos: false,
  })
  const [localidadSearch, setLocalidadSearch] = useState("")

  function updateField(field, value) {
    setForm((current) => ({ ...current, [field]: value }))
  }

  function handleNext(event) {
    event.preventDefault()
    setStep((current) => Math.min(3, current + 1))
  }

  function handleBack(event) {
    event.preventDefault()
    setStep((current) => Math.max(1, current - 1))
  }

  function handleSubmit(event) {
    event.preventDefault()
    setStep(3)
  }

  return (
    <div className="w-full max-w-full rounded-[2rem] border border-slate-200 bg-white p-6 sm:p-8 shadow-sm lg:max-w-[1080px] mx-auto max-h-[calc(100vh-20vh)] overflow-hidden min-h-0">
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

      <form className="space-y-3" onSubmit={step === 2 ? handleSubmit : handleNext}>
        {step === 1 && (
          <div className="space-y-3">
            <label className="block text-sm font-medium text-slate-700">
              Razón Social
              <div className="relative mt-1">
                <Building2 className="pointer-events-none absolute left-4 top-1/2 h-4 w-4 -translate-y-1/2 text-slate-400" />
                <Input
                  value={form.razonSocial}
                  onChange={(event) => updateField("razonSocial", event.target.value)}
                  placeholder="Ej. Concesionaria Santa Fe"
                  className="h-10 w-full rounded-xl border border-slate-200 bg-white pl-11 pr-4 text-slate-900 placeholder:text-slate-400"
                />
              </div>
            </label>

            <div className="grid gap-2 sm:grid-cols-2">
              <label className="block text-sm font-medium text-slate-700">
                Nombre Fantasía
                <div className="relative mt-1">
                  <Tag className="pointer-events-none absolute left-4 top-1/2 h-4 w-4 -translate-y-1/2 text-slate-400" />
                  <Input
                    value={form.nombreFantasia}
                    onChange={(event) => updateField("nombreFantasia", event.target.value)}
                    placeholder="Ej. Santa Fe Motors"
                    className="h-10 w-full rounded-xl border border-slate-200 bg-white pl-11 pr-4 text-slate-900 placeholder:text-slate-400"
                  />
                </div>
              </label>

              <label className="block text-sm font-medium text-slate-700">
                CUIT
                <div className="relative mt-1">
                  <Hash className="pointer-events-none absolute left-4 top-1/2 h-4 w-4 -translate-y-1/2 text-slate-400" />
                  <Input
                    value={form.cuit}
                    onChange={(event) => updateField("cuit", event.target.value)}
                    placeholder="20-12345678-9"
                    className="h-10 w-full rounded-xl border border-slate-200 bg-white pl-11 pr-4 text-slate-900 placeholder:text-slate-400"
                  />
                </div>
              </label>
            </div>

            <div className="grid gap-2 sm:grid-cols-2">
              <label className="block text-sm font-medium text-slate-700">
                Localidad
                <div className="relative mt-1">
                  <MapPin className="pointer-events-none absolute left-4 top-1/2 h-4 w-4 -translate-y-1/2 text-slate-400" />
                  <Select
                    value={form.localidad}
                    onValueChange={(value) => {
                      updateField("localidad", value)
                      setLocalidadSearch("")
                    }}
                    onOpenChange={(open) => {
                      if (!open) setLocalidadSearch("")
                    }}
                  >
                    <SelectTrigger className="w-full h-10 rounded-xl border border-slate-200 bg-white text-slate-900 pl-11 pr-4">
                      <SelectValue placeholder="Localidad" />
                    </SelectTrigger>

                    <SelectContent className="bg-white border border-slate-200 text-slate-900">
                      <div className="px-3 pt-3">
                        <Input
                          value={localidadSearch}
                          onChange={(event) => setLocalidadSearch(event.target.value)}
                          placeholder="Buscar localidad"
                          className="mb-2 h-10 w-full rounded-xl border border-slate-200 bg-slate-50 px-4 text-slate-900 placeholder:text-slate-400"
                        />
                      </div>
                      {localidadOptions
                        .filter((location) =>
                          location.toLowerCase().includes(localidadSearch.toLowerCase())
                        )
                        .map((location) => (
                          <SelectItem key={location} value={location}>
                            {location}
                          </SelectItem>
                        ))}
                    </SelectContent>
                  </Select>
                </div>
              </label>

              <label className="block text-sm font-medium text-slate-700">
                Moneda Principal
                <div className="relative mt-1">
                  <DollarSign className="pointer-events-none absolute left-4 top-1/2 h-4 w-4 -translate-y-1/2 text-slate-400" />
                  <Select
                    value={form.moneda}
                    onValueChange={(value) => updateField("moneda", value)}
                  >
                    <SelectTrigger className="w-full h-10 rounded-xl border border-slate-200 bg-white text-slate-900 pl-11 pr-4">
                      <SelectValue placeholder="Moneda Principal" />
                    </SelectTrigger>

                    <SelectContent className="bg-white border border-slate-200 text-slate-900">
                      <SelectItem value="ARS">ARS</SelectItem>
                      <SelectItem value="USD">USD</SelectItem>
                      <SelectItem value="BRL">BRL</SelectItem>
                    </SelectContent>
                  </Select>
                </div>
              </label>
            </div>
          </div>
        )}

        {step === 2 && (
          <div className="space-y-4">
            <div className="grid gap-2 sm:grid-cols-2">
              <label className="block text-sm font-medium text-slate-700">
                Nombre Completo
                <div className="relative mt-1">
                  <User className="pointer-events-none absolute left-4 top-1/2 h-4 w-4 -translate-y-1/2 text-slate-400" />
                  <Input
                    value={form.nombreCompleto}
                    onChange={(event) => updateField("nombreCompleto", event.target.value)}
                    placeholder="Ej. Juan Pérez"
                    className="h-10 w-full rounded-xl border border-slate-200 bg-white pl-11 pr-4 text-slate-900 placeholder:text-slate-400"
                  />
                </div>
              </label>

              <label className="block text-sm font-medium text-slate-700">
                Email
                <div className="relative mt-1">
                  <Mail className="pointer-events-none absolute left-4 top-1/2 h-4 w-4 -translate-y-1/2 text-slate-400" />
                  <Input
                    value={form.email}
                    onChange={(event) => updateField("email", event.target.value)}
                    placeholder="correo@empresa.com"
                    className="h-10 w-full rounded-xl border border-slate-200 bg-white pl-11 pr-4 text-slate-900 placeholder:text-slate-400"
                  />
                </div>
              </label>
            </div>

            <div className="grid gap-2 sm:grid-cols-2">
              <label className="block text-sm font-medium text-slate-700">
                Contraseña
                <div className="relative mt-1">
                  <LockKeyhole className="pointer-events-none absolute left-4 top-1/2 h-4 w-4 -translate-y-1/2 text-slate-400" />
                  <Input
                    type="password"
                    value={form.password}
                    onChange={(event) => updateField("password", event.target.value)}
                    placeholder="••••••••"
                    className="h-10 w-full rounded-xl border border-slate-200 bg-white pl-11 pr-4 text-slate-900 placeholder:text-slate-400"
                  />
                </div>
              </label>

              <label className="block text-sm font-medium text-slate-700">
                Confirmar contraseña
                <div className="relative mt-1">
                  <LockKeyhole className="pointer-events-none absolute left-4 top-1/2 h-4 w-4 -translate-y-1/2 text-slate-400" />
                  <Input
                    type="password"
                    value={form.confirmPassword}
                    onChange={(event) => updateField("confirmPassword", event.target.value)}
                    placeholder="••••••••"
                    className="h-10 w-full rounded-xl border border-slate-200 bg-white pl-11 pr-4 text-slate-900 placeholder:text-slate-400"
                  />
                </div>
              </label>
            </div>

            {form.confirmPassword && form.password !== form.confirmPassword ? (
              <p className="text-sm text-rose-500">Las contraseñas no coinciden.</p>
            ) : null}

            <label className="flex cursor-pointer items-center gap-3 rounded-2xl border border-slate-200 bg-white px-4 py-4 text-sm text-slate-700">
              <input
                type="checkbox"
                checked={form.aceptoTerminos}
                onChange={(event) => updateField("aceptoTerminos", event.target.checked)}
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
              Gracias por registrarte. Ya recibimos los datos de tu empresa y tu usuario. Te avisaremos cuando el pago sea recibido o si necesitamos información adicional.
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
              className="h-10 rounded-2xl px-6 font-semibold text-white cursor-pointer hover:shadow-lg transition-shadow"
              style={{ background: "hsl(var(--nav-bg))" }}
            >
              {step === 1 ? "Siguiente" : "Crear"}
              <ArrowRight className="ml-2 w-4 h-4" />
            </Button>
          </div>
        )}

        {step !== 3 && (
          <p className="text-center text-sm text-slate-500">
            ¿Ya tenés cuenta? <Link to="/" className="font-semibold text-slate-900 hover:text-slate-700 transition-colors underline-offset-4 hover:underline">Ingresar</Link>
          </p>
        )}
      </form>
    </div>
  )
}
