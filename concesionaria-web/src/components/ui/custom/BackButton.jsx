import { ArrowLeft } from "lucide-react";
import { Button } from "@/components/ui/button";

export function BackButton({ children = "Volver", ...props }) {
  return (
    <Button
      type="button"
      variant="outline"
      className="h-9 rounded-xl border-slate-200 bg-white px-3.5 font-semibold text-slate-600 shadow-sm transition-all hover:border-[hsl(var(--nav-bg)/0.35)] hover:bg-[hsl(var(--nav-bg))] hover:text-white hover:shadow-md"
      {...props}
    >
      <ArrowLeft className="mr-2 h-4 w-4 transition-transform group-hover/button:-translate-x-0.5" />
      {children}
    </Button>
  );
}
