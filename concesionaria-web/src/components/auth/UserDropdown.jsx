import { useState, useEffect } from "react"; // 1. Importa useState
import { Link } from "react-router-dom";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import {
  User,
  ChevronDown,
  Settings,
  Palette,
  ShieldCheck,
  Building2,
} from "lucide-react";
import { THEMES } from "../../constants/themes";
import { LogoutButton } from "./LogoutButton";
import { useAuth } from "@/context/AuthContext";
import SucursalService from "@/services/Ubicacion/sucursalService";
import { AppSelect } from "@/components/ui/custom/AppSelect";

export function UserDropdown() {
  const { user, loading, activeSucursal, changeActiveSucursal } = useAuth();
  const [sucursales, setSucursales] = useState([]);

  // 2. Estado para rastrear el ID del tema activo
  const [activeTheme, setActiveTheme] = useState(
    () => localStorage.getItem("app-theme") || "default",
  );

  useEffect(() => {
    const savedTheme = localStorage.getItem("app-theme");
    if (savedTheme) {
      document.documentElement.setAttribute("data-theme", savedTheme);
      setActiveTheme(savedTheme);
    }
  }, []);

  const isAdministrator = user?.roles?.some(
    (role) => String(role).toUpperCase() === "ADMINISTRADOR",
  );

  useEffect(() => {
    if (!isAdministrator) return;

    SucursalService.getActivas()
      .then((result) => {
        setSucursales(result);

        const selectedIsActive = result.some(
          (sucursal) => sucursal.sucursalId === activeSucursal?.id,
        );

        if (!selectedIsActive) {
          const defaultSucursal =
            result.find((sucursal) => sucursal.sucursalId === user.sucursalId) ||
            result[0];

          if (defaultSucursal) changeActiveSucursal(defaultSucursal);
        }
      })
      .catch(() => setSucursales([]));
  }, [isAdministrator, user?.sucursalId]);

  if (loading) return null;
  if (!user) return null;

  const changeTheme = (themeId) => {
    document.documentElement.setAttribute("data-theme", themeId);
    localStorage.setItem("app-theme", themeId);
    setActiveTheme(themeId); // 3. Actualiza el estado al cambiar
  };

  return (
    <DropdownMenu>
      <DropdownMenuTrigger className="flex items-center gap-2 rounded-lg border border-white/10 bg-white/10 px-3 md:px-4 h-10 text-sm font-medium text-white hover:bg-white/15 transition-colors cursor-pointer outline-none">
        <div className="h-8 w-8 rounded-full bg-white/20 overflow-hidden flex items-center justify-center">
          <img
            src={user.avatarURL || "/logo-solo.png"}
            alt="Perfil"
            className="h-full w-full object-cover"
          />
        </div>
        <span className="hidden sm:inline">{user.name}</span>
        <ChevronDown className="h-3.5 w-3.5 opacity-75" />
      </DropdownMenuTrigger>

      <DropdownMenuContent
        align="end"
        className="w-64 p-2 shadow-2xl rounded-lg border-0 mt-1"
        style={{ backgroundColor: "hsl(var(--nav-bg))", border: "none" }}
      >
        <div className="px-2 py-2">
          <div className="flex items-center gap-3 rounded-lg bg-white/[0.06] p-3">
            <div className="flex h-11 w-11 shrink-0 items-center justify-center overflow-hidden rounded-full border border-white/15 bg-white/10 shadow-sm">
              <img
                src={user.avatarURL || "/logo-solo.png"}
                alt="Perfil"
                className="h-full w-full object-cover"
              />
            </div>

            <div className="min-w-0 flex-1">
              <p className="truncate text-sm font-semibold text-white">
                {user.name}
              </p>
              <p className="mt-0.5 truncate text-xs text-white/55">
                {user.email}
              </p>
            </div>
          </div>

          <div className="mt-2 grid gap-1 px-1">
            <div className="flex items-center gap-2 rounded-md px-2 py-1.5 text-xs text-white/70">
              <ShieldCheck className="h-3.5 w-3.5 shrink-0 text-white/45" />
              <span className="truncate font-medium">
                {user.role || "Sin rol asignado"}
              </span>
            </div>

            <div className="flex items-center gap-2 rounded-md px-2 py-1.5 text-xs text-white/70">
              <Building2 className="h-3.5 w-3.5 shrink-0 text-white/45" />
              {isAdministrator ? (
                <AppSelect
                  value={activeSucursal?.id || user.sucursalId}
                  onValueChange={(value) => {
                    const sucursal = sucursales.find(
                      (item) => item.sucursalId === value,
                    );
                    if (sucursal) changeActiveSucursal(sucursal);
                  }}
                  options={sucursales}
                  optionValue="sucursalId"
                  optionLabel="nombre"
                  placeholder="Seleccione una sucursal"
                  className="min-w-0 flex-1 [&_button]:h-8 [&_button]:border-white/15 [&_button]:bg-white/10 [&_button]:text-white [&_button]:shadow-none [&_button:hover]:bg-white/15"
                />
              ) : (
                <span className="truncate">
                  {user.sucursalNombre || "Sin sucursal asignada"}
                </span>
              )}
            </div>
          </div>
        </div>

        <DropdownMenuSeparator className="bg-white/10 my-1" />

        <DropdownMenuItem asChild>
          <Link
            to="/layout/perfil"
            className="flex items-center gap-2.5 rounded-md px-2.5 py-2.5 text-sm text-white/90 hover:bg-white/15 focus:bg-white/15 focus:text-white transition-colors outline-none cursor-pointer"
          >
            <User className="h-4 w-4 opacity-70" /> Mi Perfil
          </Link>
        </DropdownMenuItem>

        <DropdownMenuItem asChild>
          <Link
            to="/configuracion"
            className="flex items-center gap-2.5 rounded-md px-2.5 py-2.5 text-sm text-white/90 hover:bg-white/15 focus:bg-white/15 focus:text-white transition-colors outline-none cursor-pointer"
          >
            <Settings className="h-4 w-4 opacity-70" /> Configurar Sistema
          </Link>
        </DropdownMenuItem>

        <DropdownMenuSeparator className="bg-white/10 my-1" />

        <div className="px-2.5 py-2.5">
          <div className="flex items-center gap-2 mb-2 text-xs text-white/50 font-bold uppercase tracking-wider">
            <Palette className="h-3.5 w-3.5" /> Color del Sistema
          </div>
          <div className="grid grid-cols-6 gap-2">
            {THEMES.map((t) => (
              <button
                key={t.id}
                onClick={() => changeTheme(t.id)}
                className={`h-7 w-7 rounded-full transition-all cursor-pointer hover:scale-105 
                ${
                  activeTheme === t.id
                    ? "ring-2 ring-white outline outline-2 outline-black/20"
                    : "border border-black/20"
                }`}
                style={{ backgroundColor: t.color }}
              />
            ))}
          </div>
        </div>

        <DropdownMenuSeparator className="bg-white/10 my-1" />

        <LogoutButton className="flex items-center gap-2.5 rounded-md px-2.5 py-2.5 text-sm font-medium text-red-400 hover:bg-red-500/20 focus:bg-red-500/20 text-red-300 transition-colors outline-none cursor-pointer" />
      </DropdownMenuContent>
    </DropdownMenu>
  );
}
