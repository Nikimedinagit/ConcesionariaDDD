import { ChevronDown, Filter } from "lucide-react";
import { Button } from "@/components/ui/button";

export function FilterButton({ isOpen, activeCount = 0, onToggle }) {
  return (
    <Button
      type="button"
      size="sm"
      variant="outline"
      onClick={onToggle}
      className={`h-8 gap-2 shadow-sm ${
        isOpen
          ? "border-[hsl(var(--nav-bg))] bg-[hsl(var(--nav-bg))] text-white hover:bg-[hsl(var(--nav-bg))] hover:text-white hover:opacity-90"
          : activeCount
            ? "border-[hsl(var(--nav-bg)/0.35)] bg-white text-[hsl(var(--nav-bg))] hover:bg-slate-100"
            : "border-slate-300 bg-white text-slate-600 hover:bg-slate-100 hover:text-slate-900"
      }`}
      aria-expanded={isOpen}
    >
      <Filter className="h-3.5 w-3.5" />
      Filtros
      {activeCount > 0 && (
        <span
          className={`flex h-5 min-w-5 items-center justify-center rounded-full px-1 text-[11px] font-bold ${
            isOpen
              ? "bg-white text-[hsl(var(--nav-bg))]"
              : "bg-[hsl(var(--nav-bg))] text-white"
          }`}
        >
          {activeCount}
        </span>
      )}
      <ChevronDown
        className={`h-3.5 w-3.5 transition-transform ${isOpen ? "rotate-180" : ""}`}
      />
    </Button>
  );
}
