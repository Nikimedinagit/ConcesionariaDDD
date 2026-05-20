export function AuthHero() {
  const features = [
    "Ventas",
    "Gestoría",
    "Contable",
    "Vehículos",
    "Gastos",
    "Tesorería",
  ]

  return (
    <div className="rounded-[2rem] border border-slate-200 bg-white p-8 shadow-sm lg:p-12">
      <div className="mb-10 max-w-[260px]">
        <img
          src="/logo-completo.png"
          alt="MPM Solutions"
          className="w-full object-contain"
        />
      </div>

      <div className="space-y-8">
        <div>
          <p className="text-sm font-semibold uppercase tracking-[0.28em] text-slate-500">
            Plataforma integral
          </p>

          <h1 className="mt-4 text-4xl font-black tracking-tight text-slate-900 sm:text-5xl">
            Gestión moderna para concesionarias.
          </h1>
        </div>

        <p className="max-w-xl text-base leading-7 text-slate-600">
          Un ecosistema pensado para ventas, gestoría, contabilidad y el control total de vehículos, gastos y tesorería.
        </p>

        <div className="flex flex-wrap gap-3">
          {features.map((feature) => (
            <span
              key={feature}
              className="rounded-full border border-slate-200 bg-slate-50 px-4 py-2 text-sm font-medium text-slate-700"
            >
              {feature}
            </span>
          ))}
        </div>
      </div>
    </div>
  )
}
