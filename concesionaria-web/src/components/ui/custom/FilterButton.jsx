// src/components/ui/custom/FilterButton.jsx
import { SlidersHorizontal } from "lucide-react";
import { Button } from "@/components/ui/button";

const FilterButton = ({
  children = "Filtros",
  onClick,
  className = "",
}) => {
  return (
    <Button
      variant="outline"
      onClick={onClick}
      className={`
        border-[hsl(var(--nav-bg)/0.3)]
        bg-white
        hover:bg-[hsl(var(--nav-bg)/0.05)]
        text-slate-700
        shadow-sm
        font-medium
        transition-all duration-200
        hover:border-[hsl(var(--nav-bg)/0.5)]
        active:scale-95
        ${className}
      `}
    >
      <SlidersHorizontal className="mr-2 h-4 w-4 text-[hsl(var(--nav-bg))]" />
      {children}
    </Button>
  );
};

export default FilterButton;