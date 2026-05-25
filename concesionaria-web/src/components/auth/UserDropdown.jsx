import { jwtDecode } from "jwt-decode";
import { Link } from "react-router-dom";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { User, ChevronDown, Settings, Palette } from "lucide-react";
import { THEMES } from "../../constants/themes";
import { LogoutButton } from "./LogoutButton";
import { useAuth } from "@/context/AuthContext";

export function UserDropdown() {
  const { user } = useAuth();
 if (!user) return null;
 
  const token = localStorage.getItem("token");
  let userData = { name: "Invitado", email: "", avatarURL: "" };

  if (token) {
    try {
      const decoded = jwtDecode(token);

      userData = {
        name: decoded.nombre || "Usuario",
        email: decoded.email || "",
        avatarURL: decoded.avatarUrl || "",
      };
    } catch (e) {
      console.error("Error al decodificar token", e);
    }
  }

  const changeTheme = (themeId) => {
    document.documentElement.setAttribute("data-theme", themeId);
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
        <div className="px-3 py-2">
          <p className="text-sm font-semibold text-white">{userData.name}</p>
          <p className="text-sm font-medium text-white/60 truncate">
            {userData.email}
          </p>
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
                className="h-6 w-6 rounded-full border border-black/20 transition-all cursor-pointer hover:scale-105"
                style={{ backgroundColor: t.color }}
                onClick={() => changeTheme(t.id)}
                title={t.name}
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
