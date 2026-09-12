import { ArrowLeft } from "lucide-react";
import { Button } from "@/components/ui/button";

export function BackButton({ children = "Volver", className = "", ...props }) {
  return (
    <Button
      type="button"
      className={`group/button bg-[hsl(var(--nav-bg))] font-medium text-white shadow-sm transition-all duration-200 hover:opacity-90 ${className}`}
      {...props}
    >
      <ArrowLeft className="mr-2 h-4 w-4 transition-transform group-hover/button:-translate-x-0.5" />
      {children}
    </Button>
  );
}
