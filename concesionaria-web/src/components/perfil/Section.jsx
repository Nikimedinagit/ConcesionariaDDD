export function Section({
  title,
  icon: Icon,
  children,
  badge,
}) {
  const isInactive = String(badge).toLowerCase() === "inactivo";

  return (
    <div className="bg-white p-6 rounded-xl border border-slate-200 shadow-sm">
      <div className="flex justify-between items-center mb-6">
        <h3 className="text-lg font-bold text-slate-900 flex items-center gap-2 uppercase tracking-tight">
          <Icon className="w-5 h-5 text-slate-400" />
          {title}
        </h3>

        {badge && (
          <span
            className={
              isInactive
                ? "rounded-full bg-slate-100 px-2.5 py-1 text-[10px] font-bold uppercase text-slate-600 ring-1 ring-slate-200"
                : "rounded-full bg-emerald-50 px-2.5 py-1 text-[10px] font-bold uppercase text-emerald-700 ring-1 ring-emerald-200"
            }
          >
            {badge}
          </span>
        )}
      </div>

      {children}
    </div>
  );
}
