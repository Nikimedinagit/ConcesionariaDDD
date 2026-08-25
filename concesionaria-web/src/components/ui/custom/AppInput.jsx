import { Input } from "@/components/ui/input";
import { Eye, EyeOff } from "lucide-react";
import { useState } from "react";

export function AppInput({
  label,
  icon: Icon,
  error, 
  className,
  passwordToggle = false,
  ...props 
}) {
  const [visible, setVisible] = useState(false);
  const inputType = passwordToggle && visible ? "text" : props.type;

  return (
    <div className={`space-y-1.5 ${className}`}>
      {label && <label className="text-sm font-semibold text-slate-700">{label}</label>}
      <div className="relative">
        {Icon && (
          <Icon className="absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 text-slate-400" />
        )}
        <Input
          {...props}
          type={inputType}
          className={`h-[40px] w-full rounded-lg border border-slate-300 bg-white shadow-sm
            ${Icon ? "pl-10" : "pl-4"} ${passwordToggle ? "pr-10" : "pr-4"}
            hover:border-slate-400
            focus-visible:border-[hsl(var(--nav-bg))]
            focus-visible:ring-2 focus-visible:ring-[hsl(var(--nav-bg)/0.14)]
            ${props.disabled ? "bg-slate-50 text-slate-500 cursor-not-allowed" : ""}
            ${error ? "border-red-500 focus-visible:ring-red-500" : ""}
            
          `}
        />
        {passwordToggle && (
          <button
            type="button"
            onClick={() => setVisible((current) => !current)}
            className="absolute right-3 top-1/2 -translate-y-1/2 cursor-pointer text-slate-400 transition-colors hover:text-[hsl(var(--nav-bg))]"
            aria-label={visible ? "Ocultar contraseña" : "Mostrar contraseña"}
          >
            {visible ? <EyeOff className="h-4 w-4" /> : <Eye className="h-4 w-4" />}
          </button>
        )}
      </div>
      {error && <p className="text-sm text-red-500 font-medium">{error}</p>}
    </div>
  );
}
