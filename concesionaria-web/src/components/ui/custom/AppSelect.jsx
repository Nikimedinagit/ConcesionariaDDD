import { useMemo, useState } from "react";
import { Search } from "lucide-react";
import { Input } from "@/components/ui/input";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";

const getOptionValue = (option, optionValue) =>
  typeof optionValue === "function" ? optionValue(option) : option[optionValue];

const getOptionLabel = (option, optionLabel) =>
  typeof optionLabel === "function" ? optionLabel(option) : option[optionLabel];

function SelectField({
  label,
  icon: Icon,
  value,
  onValueChange,
  options = [],
  placeholder = "Seleccione...",
  optionValue = "value",
  optionLabel = "label",
  error,
  disabled = false,
  className = "",
  searchable = false,
  searchPlaceholder = "Buscar...",
  emptyText = "Sin resultados",
}) {
  const [search, setSearch] = useState("");

  const filteredOptions = useMemo(() => {
    if (!searchable || !search.trim()) return options;

    return options.filter((option) =>
      String(getOptionLabel(option, optionLabel) || "")
        .toLowerCase()
        .includes(search.toLowerCase()),
    );
  }, [options, optionLabel, search, searchable]);

  const handleValueChange = (nextValue) => {
    onValueChange(nextValue);
    setSearch("");
  };

  return (
    <div className={`space-y-1.5 ${className}`}>
      {label && (
        <label className="text-sm font-medium text-slate-700">{label}</label>
      )}

      <Select
        value={value}
        onValueChange={handleValueChange}
        disabled={disabled}
      >
        <SelectTrigger
          className={`h-[40px] rounded-lg border-slate-200 ${
            error ? "border-red-500 focus-visible:ring-red-500" : ""
          }`}
        >
          <div className="flex items-center gap-2 truncate">
            {Icon && <Icon className="h-4 w-4 text-slate-400" />}
            <SelectValue placeholder={placeholder} />
          </div>
        </SelectTrigger>

        <SelectContent
          position="popper"
          sideOffset={4}
          className="z-[10050] w-[var(--radix-select-trigger-width)] border bg-white text-slate-900 shadow-lg"
        >
          {searchable && (
            <div className="px-3 pt-3">
              <div className="relative">
                <Search className="pointer-events-none absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 text-slate-400" />
                <Input
                  value={search}
                  onChange={(e) => setSearch(e.target.value)}
                  onKeyDown={(e) => e.stopPropagation()}
                  onPointerDown={(e) => e.stopPropagation()}
                  placeholder={searchPlaceholder}
                  className="mb-2 h-[40px] w-full rounded-lg border border-slate-200 bg-slate-50 pl-9 pr-4 text-slate-900 placeholder:text-slate-400"
                />
              </div>
            </div>
          )}

          {filteredOptions.length === 0 ? (
            <div className="px-3 py-2 text-sm text-slate-500">
              {emptyText}
            </div>
          ) : (
            filteredOptions.map((option) => {
              const itemValue = String(getOptionValue(option, optionValue));
              const itemLabel = getOptionLabel(option, optionLabel);

              return (
                <SelectItem key={itemValue} value={itemValue}>
                  {itemLabel}
                </SelectItem>
              );
            })
          )}
        </SelectContent>
      </Select>

      {error && <p className="text-sm font-medium text-red-500">{error}</p>}
    </div>
  );
}

export function AppSelect(props) {
  return <SelectField {...props} />;
}

export function AppSearchSelect(props) {
  return <SelectField {...props} searchable />;
}
