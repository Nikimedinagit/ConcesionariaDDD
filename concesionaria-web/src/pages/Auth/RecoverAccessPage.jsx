import { useState } from "react"
import { Link } from "react-router-dom"
import { ArrowRight, Mail, Smartphone } from "lucide-react"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"

export default function RecoverAccessPage() {
  const [contact, setContact] = useState("")

  function handleSubmit(event) {
    event.preventDefault()
    // Aquí puede ir la llamada a la API para enviar el enlace de recuperación.
    alert(`Se enviará la información a: ${contact}`)
  }

  return (
    <div className="rounded-[2rem] border border-slate-200 bg-white p-6 sm:p-8 shadow-sm w-full max-w-[1080px] mx-auto max-h-[calc(100vh-20vh)] overflow-hidden">
      <div className="mb-5">
        <p className="text-sm uppercase tracking-[0.24em] text-slate-500">
          Recuperar acceso
        </p>

        <h2 className="mt-4 text-3xl font-black tracking-tight text-slate-900 sm:text-4xl">
          Ayuda y recuperación
        </h2>

        <p className="mt-3 text-sm leading-relaxed text-slate-500 sm:text-base">
          Ingresá tu correo o número registrado y te enviaremos un enlace para recuperar el acceso.
          Si necesitás ayuda, contactanos por email o WhatsApp.
        </p>
      </div>

      <form className="space-y-3" onSubmit={handleSubmit}>
        <div className="space-y-1">
          <label className="text-sm font-medium text-slate-700">
            Email o número de teléfono
          </label>

          <div className="relative">
            <Smartphone className="absolute left-4 top-1/2 -translate-y-1/2 w-4 h-4 text-slate-400" />
            <Input
              type="text"
              value={contact}
              onChange={(event) => setContact(event.target.value)}
              placeholder="correo@empresa.com o +549351..."
              className="h-11 rounded-xl border border-slate-200 bg-white pl-11 text-slate-900 placeholder:text-slate-400 focus-visible:ring-2 focus-visible:ring-slate-300"
            />
          </div>
        </div>

        <Button
          type="submit"
          className="w-full h-11 rounded-2xl font-semibold text-white"
          style={{ background: "hsl(var(--nav-bg))" }}
        >
          Enviar enlace
          <ArrowRight className="ml-2 w-4 h-4" />
        </Button>

        <div className="rounded-3xl border border-slate-200 bg-slate-50 p-4 text-sm text-slate-600">
          <p className="font-semibold text-slate-900">¿Necesitás ayuda?</p>
          <p className="mt-3 text-sm leading-relaxed text-slate-500">
            Si querés ayuda rápida, escribinos por email o WhatsApp y te respondemos enseguida.
          </p>

          <div className="mt-3 grid gap-2 sm:grid-cols-2">
            <Button asChild className="w-full rounded-2xl bg-red-500 text-white hover:bg-red-600">
              <a href="mailto:soporte@concesionaria.com">
                <span className="inline-flex items-center justify-center gap-2">
                  <Mail className="h-4 w-4" />
                  Email
                </span>
              </a>
            </Button>

            <Button asChild className="w-full rounded-2xl bg-emerald-500 text-white hover:bg-emerald-600">
              <a href="https://wa.me/5493511234567" target="_blank" rel="noreferrer">
                <span className="inline-flex items-center justify-center gap-2">
                  <Smartphone className="h-4 w-4" />
                  WhatsApp
                </span>
              </a>
            </Button>
          </div>
        </div>

        <p className="text-center text-sm text-slate-500">
          Volver a <Link to="/" className="font-semibold text-slate-900 hover:text-slate-700 transition-colors underline-offset-4 hover:underline">Ingresar</Link>
        </p>
      </form>
    </div>
  )
}
