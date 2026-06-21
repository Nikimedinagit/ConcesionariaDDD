import { useState } from "react";
import { Search } from "lucide-react";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";

const DataTableToolbar = ({ tipo, setTipo, onSearch }) => {
  const [searchValue, setSearchValue] = useState("");

  const handleSearch = (value) => {
    const nextValue = value.toUpperCase();
    setSearchValue(nextValue);
    onSearch(nextValue);
  };

  return (
    <div
      className="
        flex flex-col gap-4
        border-b border-slate-100
        px-4 py-4
        md:flex-row md:items-center md:justify-between
      "
    >
      <div
        className="
        flex items-center gap-2
        justify-center
        md:justify-start
      "
      >
        <Button
          size="sm"
          onClick={() => setTipo("activas")}
          className={`
            transition-colors
            ${
              tipo === "activas"
                ? "bg-[hsl(var(--nav-bg))] text-white hover:opacity-90"
                : "bg-transparent text-slate-600 border border-slate-200 hover:bg-slate-50"
            }
          `}
        >
          Activos
        </Button>

        <Button
          size="sm"
          onClick={() => setTipo("inactivas")}
          className={`
            transition-colors
            ${
              tipo === "inactivas"
                ? "bg-[hsl(var(--nav-bg))] text-white hover:opacity-90"
                : "bg-transparent text-slate-600 border border-slate-200 hover:bg-slate-50"
            }
          `}
        >
          Inactivos
        </Button>
      </div>

      <div className="relative w-full md:w-[280px]">
        <Search
          className="
            absolute left-3 top-1/2
            h-4 w-4
            -translate-y-1/2
            text-slate-400
          "
        />

        <Input
          placeholder="Buscar..."
          value={searchValue}
          onChange={(e) => handleSearch(e.target.value)}
          className="pl-9 border-slate-200 bg-slate-50/60 focus-visible:ring-[hsl(var(--nav-bg))]"
        />
      </div>
    </div>
  );
};

export default DataTableToolbar;
