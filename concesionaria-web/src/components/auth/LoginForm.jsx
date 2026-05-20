import { Link } from "react-router-dom"

import {
  ArrowRight,
  LockKeyhole,
  Mail,
} from "lucide-react"

import { Button } from "@/components/ui/button"

import { Input } from "@/components/ui/input"

export function LoginForm() {
  return (
    <div className="rounded-[2rem] border border-slate-200 bg-white p-5 sm:p-7 shadow-sm w-full max-w-[900px] mx-auto max-h-[calc(100vh-2rem)] overflow-hidden">
      <div className="mb-8">
        <p className="text-sm uppercase tracking-[0.24em] text-slate-500">
          Acceso
        </p>

        <h2 className="mt-4 text-3xl font-black tracking-tight text-slate-900 sm:text-4xl">
          Bienvenidos
        </h2>

        <p className="mt-3 text-sm leading-relaxed text-slate-500 sm:text-base">
          Accede a tu espacio de gestión.
        </p>
      </div>

      {/* FORM */}
      <div className="space-y-3">

        {/* EMAIL */}
        <div className="space-y-1">
          <label className="text-sm font-medium text-slate-700">
            Email
          </label>

          <div className="relative">
            <Mail className="absolute left-4 top-1/2 -translate-y-1/2 w-4 h-4 text-slate-400" />

            <Input
              type="email"
              placeholder="tuemail@empresa.com"
              className="h-10 rounded-xl border border-slate-200 bg-white pl-10 text-slate-900 placeholder:text-slate-400 focus-visible:ring-2 focus-visible:ring-slate-300"
            />
          </div>
        </div>

        {/* PASSWORD */}
        <div className="space-y-1">
          <label className="text-sm font-medium text-slate-700">
            Contraseña
          </label>

          <div className="relative">
            <LockKeyhole
              className="
                absolute
                left-4
                top-1/2
                -translate-y-1/2
                w-4
                h-4
                text-slate-400
              "
            />

            <Input
              type="password"
              placeholder="••••••••"
              className="
                h-10
                rounded-xl
                border border-slate-200
                bg-white
                pl-10
                text-slate-900
                placeholder:text-slate-400
                focus-visible:ring-2
                focus-visible:ring-slate-300
              "
            />
          </div>
        </div>

        {/* BUTTON */}
        <Button
          className="
            w-full
            h-10
            rounded-2xl
            font-semibold
            text-[14px]
            mt-1
            text-white
          "
          style={{
            background:
              "hsl(var(--nav-bg))",
          }}
        >
          Ingresar

          <ArrowRight className="ml-2 w-4 h-4" />
        </Button>

        {/* FOOTER */}
        <div
          className="
            flex
            flex-row
            flex-wrap
            items-center
            justify-between
            gap-3
            pt-2
            text-sm
          "
        >
          <button className="text-slate-500 hover:text-slate-900 transition-colors">
            Recuperar acceso
          </button>

          <Link
            to="/register"
            className="font-semibold text-slate-900"
          >
            Crear cuenta
          </Link>
        </div>
      </div>
    </div>
  )
}