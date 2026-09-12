import { useState } from "react";
import { Search } from "lucide-react";
import { Button } from "@/components/ui/button";
import { AppInput } from "@/components/ui/custom/AppInput";

const DataTableToolbar = ({
  tipo,
  setTipo,
  onSearch,
  searchValue: controlledSearchValue,
  showStatusFilter = true,
  actions,
}) => {
  const [searchValue, setSearchValue] = useState("");

  const handleSearch = (value) => {
    const nextValue = value.toUpperCase();
    setSearchValue(nextValue);
    onSearch(nextValue);
  };

  const displayedSearchValue = controlledSearchValue ?? searchValue;

  return (
    <div
      className="
        flex flex-col gap-4
        border-b border-slate-200 bg-slate-50/60
        px-4 py-4
        md:flex-row md:items-center md:justify-between
      "
    >
      {showStatusFilter && (
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
      )}

      <AppInput
        icon={Search}
        placeholder="Buscar..."
        value={displayedSearchValue}
        onChange={(event) => handleSearch(event.target.value)}
        autoComplete="new-password"
        name="table-filter-value"
        autoCapitalize="none"
        spellCheck={false}
        data-form-type="other"
        data-lpignore="true"
        className="w-full [&_input]:h-8 md:ml-auto md:w-[280px]"
      />

      {actions}
    </div>
  );
};

export default DataTableToolbar;
