import { cn } from "@/lib/utils";

export function AppTextarea({
  label,
  icon: Icon,
  error,
  className,
  ...props
}) {
  return (
    <div className={cn("space-y-1.5", className)}>
      {label && (
        <label className="text-sm font-semibold text-slate-700">{label}</label>
      )}
      <div className="relative">
        {Icon && (
          <Icon className="absolute left-3 top-3 h-4 w-4 text-slate-400" />
        )}
        <textarea
          {...props}
          className={cn(
            "min-h-[96px] w-full resize-y rounded-lg border border-slate-300 bg-white px-4 py-2.5 text-sm shadow-sm outline-none transition-colors placeholder:text-muted-foreground",
            "hover:border-slate-400",
            "focus-visible:border-[hsl(var(--nav-bg))] focus-visible:ring-2 focus-visible:ring-[hsl(var(--nav-bg)/0.14)]",
            Icon && "pl-10",
            props.disabled && "cursor-not-allowed bg-slate-50 text-slate-500",
            error && "border-red-500 focus-visible:ring-red-500",
          )}
        />
      </div>
      {error && <p className="text-sm font-medium text-red-500">{error}</p>}
    </div>
  );
}
