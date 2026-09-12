import { useEffect, useMemo, useRef, useState } from "react";
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
  const [open, setOpen] = useState(false);
  const [activeIndex, setActiveIndex] = useState(0);
  const searchInputRef = useRef(null);

  const filteredOptions = useMemo(() => {
    if (!searchable || !search.trim()) return options;

    return options.filter((option) =>
      String(getOptionLabel(option, optionLabel) || "")
        .toLowerCase()
        .includes(search.toLowerCase()),
    );
  }, [options, optionLabel, search, searchable]);

  useEffect(() => {
    setActiveIndex(0);
  }, [search]);

  useEffect(() => {
    if (!open || !searchable) return;

    const frame = requestAnimationFrame(() => searchInputRef.current?.focus());
    return () => cancelAnimationFrame(frame);
  }, [open, searchable]);

  const handleValueChange = (nextValue) => {
    onValueChange(nextValue);
    setSearch("");
    setOpen(false);
  };

  const handleSearchKeyDown = (event) => {
    event.stopPropagation();

    if (!filteredOptions.length) return;

    if (event.key === "ArrowDown") {
      event.preventDefault();
      setActiveIndex((current) => (current + 1) % filteredOptions.length);
    } else if (event.key === "ArrowUp") {
      event.preventDefault();
      setActiveIndex(
        (current) =>
          (current - 1 + filteredOptions.length) % filteredOptions.length,
      );
    } else if (event.key === "Enter") {
      event.preventDefault();
      handleValueChange(
        String(getOptionValue(filteredOptions[activeIndex], optionValue)),
      );
    }
  };

  return (
    <div className={`space-y-1.5 ${className}`}>
      {label && (
        <label className="text-sm font-semibold text-slate-700">{label}</label>
      )}

      <Select
        open={open}
        onOpenChange={setOpen}
        value={value}
        onValueChange={handleValueChange}
        disabled={disabled}
      >
        <SelectTrigger
          className={`h-[40px] w-full overflow-hidden rounded-lg border-slate-300 bg-white shadow-sm hover:border-slate-400 focus-visible:border-[hsl(var(--nav-bg))] focus-visible:ring-2 focus-visible:ring-[hsl(var(--nav-bg)/0.14)] ${
            error ? "border-red-500 focus-visible:border-red-500 focus-visible:ring-red-500/20" : ""
          }`}
        >
          <div className="flex min-w-0 items-center gap-2 truncate">
            {Icon && <Icon className="h-4 w-4 shrink-0 text-slate-400" />}
            <span className="min-w-0 truncate">
              <SelectValue placeholder={placeholder} />
            </span>
          </div>
        </SelectTrigger>

        <SelectContent
          position="popper"
          sideOffset={4}
          className="z-[10050] max-h-[240px] w-[var(--radix-select-trigger-width)] max-w-[var(--radix-select-trigger-width)] overflow-y-auto overflow-x-hidden rounded-lg border border-slate-200 bg-white p-1 text-slate-900 shadow-xl"
        >
          {searchable && (
            <div className="sticky top-0 z-10 -mx-1 -mt-1 mb-1 border-b border-slate-100 bg-white p-2">
              <div className="relative">
                <Search className="pointer-events-none absolute left-2.5 top-1/2 h-3.5 w-3.5 -translate-y-1/2 text-slate-400" />
                <Input
                  ref={searchInputRef}
                  value={search}
                  onChange={(e) => setSearch(e.target.value.toUpperCase())}
                  onKeyDown={handleSearchKeyDown}
                  onPointerDown={(e) => e.stopPropagation()}
                  placeholder={searchPlaceholder}
                  className="h-8 w-full rounded-md border-slate-200 bg-slate-50 pl-8 pr-3 text-xs text-slate-900 placeholder:text-slate-400 focus-visible:border-[hsl(var(--nav-bg))] focus-visible:ring-1 focus-visible:ring-[hsl(var(--nav-bg)/0.18)]"
                />
              </div>
            </div>
          )}

          {filteredOptions.length === 0 ? (
            <div className="px-2 py-3 text-center text-sm text-slate-500">
              {emptyText}
            </div>
          ) : (
            filteredOptions.map((option, index) => {
              const itemValue = String(getOptionValue(option, optionValue));
              const itemLabel = getOptionLabel(option, optionLabel);

              return (
                <SelectItem
                  key={itemValue}
                  value={itemValue}
                  className={`min-h-9 rounded-md py-2 pl-2.5 pr-8 text-sm text-slate-700 data-[highlighted]:bg-slate-100 data-[highlighted]:text-slate-950 data-[state=checked]:bg-[hsl(var(--nav-bg)/0.1)] data-[state=checked]:font-medium data-[state=checked]:text-[hsl(var(--nav-bg))] ${
                    searchable && index === activeIndex
                      ? "bg-slate-100 text-slate-950"
                      : ""
                  }`}
                >
                  <span className="block max-w-full truncate">
                    {itemLabel}
                  </span>
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
