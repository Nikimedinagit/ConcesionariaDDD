import { Outlet } from "react-router-dom"

export function AuthLayout() {
  return (
    <div className="min-h-screen overflow-auto lg:overflow-hidden bg-white">
      <div className="mx-auto grid min-h-screen max-w-[1400px] items-center gap-6 px-4 py-8 md:px-8 lg:grid-cols-[1.2fr_1fr] lg:py-10">
        <div className="hidden h-full min-h-0 lg:flex items-center justify-start">
          <div className="w-full max-w-xl">
            <img
              src="/logo-completo.png"
              alt="MPM Solutions"
              className="mb-10 w-[320px] object-contain"
            />

            <p className="text-sm uppercase tracking-[0.24em] text-slate-500">
              Plataforma integral
            </p>

            <h1 className="mt-6 text-5xl font-black tracking-tight text-slate-900 leading-[0.95]">
              Gestión moderna para concesionarias.
            </h1>

            <p className="mt-6 max-w-xl text-2xl leading-relaxed text-slate-500">
              Ventas, gestoría, contabilidad, vehículos, gastos y tesorería en una sola plataforma.
            </p>

            <div className="mt-10 flex flex-wrap gap-3">
              {[
                "Ventas",
                "Gestoría",
                "Contable",
                "Vehículos",
                "Gastos",
                "Tesorería",
              ].map((feature) => (
                <span
                  key={feature}
                  className="rounded-full border border-slate-200 bg-slate-50 px-4 py-2 text-sm font-medium text-slate-700"
                >
                  {feature}
                </span>
              ))}
            </div>

            <div className="mt-12 flex items-center gap-3 text-slate-500 text-sm">
              <span className="h-3 w-3 rounded-full bg-slate-900" />
              Plataforma moderna para gestión empresarial
            </div>
          </div>
        </div>

        <div className="flex h-full min-h-0 items-center justify-center px-3 py-3 bg-white sm:py-5 lg:justify-end">
          <div className="w-full max-w-full lg:ml-auto">
            <div className="mb-6 flex flex-col items-center justify-center gap-4 lg:hidden">
              <img
                src="/logo-completo.png"
                alt="MPM Solutions"
                className="w-[180px] object-contain"
              />

              <p className="text-center text-sm uppercase tracking-[0.24em] text-slate-500">
                Plataforma integral para concesionarias
              </p>
            </div>

            <Outlet />
          </div>
        </div>
      </div>
    </div>
  )
}
