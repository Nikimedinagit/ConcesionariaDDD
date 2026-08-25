export function PermissionSwitch({ checked, onCheckedChange, label, disabled = false }) {
  return (
    <label className={`flex items-center justify-between gap-3 rounded-lg border px-3 py-2 transition-colors ${disabled ? "cursor-not-allowed opacity-80" : "cursor-pointer"} ${checked ? "border-[hsl(var(--nav-bg)/0.2)] bg-[hsl(var(--nav-bg)/0.07)]" : "border-slate-200 bg-slate-50/80 hover:bg-slate-100"}`}>
      <span className={`text-sm ${checked ? "font-semibold text-slate-800" : "text-slate-500"}`}>
        {label}
      </span>
      <span className="relative shrink-0">
        <input
          type="checkbox"
          checked={checked}
          disabled={disabled}
          onChange={(event) => onCheckedChange(event.target.checked)}
          className="peer sr-only"
        />
        <span className="block h-5 w-9 rounded-full bg-slate-200 transition-colors peer-checked:bg-[hsl(var(--nav-bg))] peer-focus-visible:ring-2 peer-focus-visible:ring-[hsl(var(--nav-bg)/0.25)]" />
        <span className="pointer-events-none absolute left-0.5 top-0.5 h-4 w-4 rounded-full bg-white shadow-sm transition-transform peer-checked:translate-x-4" />
      </span>
    </label>
  );
}
