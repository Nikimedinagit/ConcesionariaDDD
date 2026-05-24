export function Section({
  title,
  icon: Icon,
  children,
  badge,
}) {
  return (
    <div className="bg-white p-6 rounded-xl border border-slate-200 shadow-sm">
      <div className="flex justify-between items-center mb-6">
        <h3 className="text-lg font-bold text-slate-900 flex items-center gap-2 uppercase tracking-tight">
          <Icon className="w-5 h-5 text-slate-400" />
          {title}
        </h3>

        {badge && (
          <span className="flex items-center gap-1.5 px-3 py-1 rounded-full bg-emerald-50 text-emerald-700 text-xs font-bold uppercase border border-emerald-200">
            <span className="w-1.5 h-1.5 rounded-full bg-emerald-500 animate-pulse"></span>
            {badge}
          </span>
        )}
      </div>

      {children}
    </div>
  );
}