import { useState } from "react";
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
  Menu,
  X,
  Settings,
  Palette,
  LogOut,
} from "lucide-react";
import { THEMES } from "../constants/themes";

export function NavbarHeader() {
  const [currentTheme, setCurrentTheme] = useState("slate");
  const [isMobileMenuOpen, setIsMobileMenuOpen] = useState(false);
  const [openGroup, setOpenGroup] = useState(null);

  const changeTheme = (themeId) => {
    setCurrentTheme(themeId);
    document.documentElement.setAttribute("data-theme", themeId);
  };

  const navGroups = [
    {
      title: "Acceso",
      items: [
        { to: "/empresa", name: "Empresa" },
        { to: "/roles", name: "Roles" },
        { to: "/usuarios", name: "Usuarios" },
      ],
    },
    {
      title: "Contabilidad",
      items: [
        { to: "/cuentas", name: "Cuentas" },
        { to: "/asientos", name: "Asientos Contables" },
        { to: "/asientos-detalle", name: "Asientos Detalle" },
      ],
    },
    {
      title: "Tesorería",
      items: [
        { to: "/cheques", name: "Cheques" },
        { to: "/gastos", name: "Gastos" },
        { to: "/categorias-gastos", name: "Categorías de Gastos" },
      ],
    },
    {
      title: "Operaciones",
      items: [
        { to: "/compras", name: "Compras" },
        { to: "/ventas", name: "Ventas" },
        { to: "/detalle-pago", name: "Detalle Pago Ventas" },
      ],
    },
    {
      title: "Gestoría",
      items: [
        { to: "/tramites", name: "Trámites" },
        { to: "/alertas", name: "Alertas y Recordatorios" },
      ],
    },
    {
      title: "Vehículos",
      items: [
        { to: "/vehiculos", name: "Vehículos" },
        { to: "/marcas", name: "Marcas" },
        { to: "/modelos", name: "Modelos" },
        { to: "/tipos", name: "Tipos de Vehículos" },
        { to: "/gastos-v", name: "Gastos de Vehículos" },
      ],
    },
    {
      title: "Personas",
      items: [
        { to: "/clientes", name: "Clientes" },
        { to: "/proveedores", name: "Proveedores" },
        { to: "/vendedores", name: "Vendedores" },
      ],
    },
    {
      title: "Ubicaciones",
      items: [
        { to: "/provincias", name: "Provincias" },
        { to: "/localidades", name: "Localidades" },
        { to: "/sucursales", name: "Sucursales" },
      ],
    },
  ];

  return (
    <header
      className="sticky top-0 z-50 w-full transition-all duration-300 shadow-lg border-b border-white/5"
      style={{
        backgroundColor: "hsl(var(--nav-bg))",
        color: "hsl(var(--nav-foreground))",
      }}
    >
      <div className="flex h-16 items-center justify-between px-4 md:px-8">
        <div className="flex items-center gap-4 md:gap-6">
          <Link to="/" className="flex items-center gap-2.5 group select-none">
            <div className="flex h-10 w-10 items-center justify-center transition-transform group-hover:scale-105">
              <img
                src="/logo-solo.png"
                alt="MPM Solutions"
                className="h-full w-full object-contain object-left drop-shadow-[0_2px_4px_rgba(0,0,0,0.15)]"
              />
            </div>
            <span className="text-xl font-black tracking-tight text-white font-sans">
              MPM
            </span>
          </Link>
          <div className="hidden md:block h-6 w-px bg-white/20" />
        </div>

        <div className="hidden lg:flex items-center gap-1">
          {navGroups.map((group) => (
            <DropdownMenu key={group.title}>
              <DropdownMenuTrigger className="flex items-center gap-1 h-10 px-3 text-sm font-medium text-white/95 rounded-md hover:bg-white/10 outline-none transition-colors">
                {group.title} <ChevronDown className="h-3.5 w-3.5 opacity-60" />
              </DropdownMenuTrigger>
              <DropdownMenuContent
                align="start"
                className="w-[200px] p-2 shadow-2xl rounded-lg border-0 mt-1"
                style={{ backgroundColor: "hsl(var(--nav-bg))" }}
              >
                {group.items.map((item) => (
                  <DropdownMenuItem key={item.to} asChild>
                    <Link
                      to={item.to}
                      className="flex items-center rounded-md p-2.5 text-sm font-medium text-white/90 hover:bg-white/15 outline-none cursor-pointer"
                    >
                      {item.name}
                    </Link>
                  </DropdownMenuItem>
                ))}
              </DropdownMenuContent>
            </DropdownMenu>
          ))}
        </div>

      <div className="flex items-center gap-2">
  <DropdownMenu>
    <DropdownMenuTrigger className="flex items-center gap-2 rounded-lg border border-white/10 bg-white/10 px-3 md:px-4 h-10 text-sm font-medium text-white hover:bg-white/15 transition-colors cursor-pointer outline-none">
      <div className="h-6 w-6 rounded-full bg-white/20 overflow-hidden flex items-center justify-center">
         <img src="/logo-solo.png" alt="Perfil" className="h-full w-full object-cover" />
      </div>
      <span className="hidden sm:inline">Ignacio Medina</span>
      <ChevronDown className="h-3.5 w-3.5 opacity-75" />
    </DropdownMenuTrigger>

    <DropdownMenuContent
      align="end"
      className="w-64 p-2 shadow-2xl rounded-lg border-0 mt-1"
      style={{ backgroundColor: "hsl(var(--nav-bg))", border: "none" }}
    >
      <div className="px-3 py-2">
        <p className="text-sm font-semibold text-white">Ignacio Medina</p>
        <p className="text-sm font-medium text-white/60 truncate">ignacio@concesionaria.com</p>
      </div>

      <DropdownMenuSeparator className="bg-white/10 my-1" />

      
      <DropdownMenuSeparator className="bg-white/10 my-1" />

      <DropdownMenuItem asChild>
        <Link to="/perfil" className="flex items-center gap-2.5 rounded-md px-2.5 py-2.5 text-sm text-white/90 hover:bg-white/15 focus:bg-white/15 focus:text-white transition-colors outline-none cursor-pointer">
          <User className="h-4 w-4 opacity-70" /> Mi Perfil
        </Link>
      </DropdownMenuItem>

      <DropdownMenuItem asChild>
        <Link to="/configuracion" className="flex items-center gap-2.5 rounded-md px-2.5 py-2.5 text-sm text-white/90 hover:bg-white/15 focus:bg-white/15 focus:text-white transition-colors outline-none cursor-pointer">
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
              className={`h-6 w-6 rounded-full border border-black/20 transition-all cursor-pointer ${
                currentTheme === t.id ? "ring-2 ring-white scale-110" : "hover:scale-105"
              }`}
              style={{ backgroundColor: t.color }}
              onClick={() => changeTheme(t.id)}
              title={t.name}
            />
          ))}
        </div>
      </div>

      <DropdownMenuSeparator className="bg-white/10 my-1" />

      <DropdownMenuItem className="flex items-center gap-2.5 rounded-md px-2.5 py-2.5 text-sm font-medium text-red-400 hover:bg-red-500/20 focus:bg-red-500/20 text-red-300 transition-colors outline-none cursor-pointer">
        <LogOut className="h-4 w-4" /> Cerrar Sesión
      </DropdownMenuItem>
    </DropdownMenuContent>
  </DropdownMenu>

  <button
    onClick={() => setIsMobileMenuOpen(!isMobileMenuOpen)}
    className="flex lg:hidden items-center justify-center h-10 w-10 rounded-lg text-white hover:bg-white/10 transition-colors cursor-pointer outline-none"
  >
    {isMobileMenuOpen ? <X className="h-6 w-6" /> : <Menu className="h-6 w-6" />}
  </button>
</div>
      </div>

      {isMobileMenuOpen && (
        <div
          className="lg:hidden w-full border-t border-white/10 p-4 space-y-2"
          style={{ backgroundColor: "hsl(var(--nav-bg))" }}
        >
          {navGroups.map((group) => (
            <div
              key={group.title}
              className="border-b border-white/5 last:border-none"
            >
              <button
                onClick={() =>
                  setOpenGroup(openGroup === group.title ? null : group.title)
                }
                className="w-full flex items-center justify-between p-3 text-sm font-bold text-white/70 uppercase tracking-wider hover:bg-white/5 transition-colors"
              >
                {group.title}
                <ChevronDown
                  className={`h-4 w-4 transition-transform ${openGroup === group.title ? "rotate-180" : ""}`}
                />
              </button>

              {openGroup === group.title && (
                <div className="pb-2 px-2 animate-in slide-in-from-top-1 fade-in duration-200">
                  {group.items.map((item) => (
                    <Link
                      key={item.to}
                      to={item.to}
                      onClick={() => {
                        setIsMobileMenuOpen(false);
                        setOpenGroup(null);
                      }}
                      className="block p-3 text-sm text-white/70 hover:text-white hover:bg-white/10 rounded-md transition-all"
                    >
                      {item.name}
                    </Link>
                  ))}
                </div>
              )}
            </div>
          ))}
        </div>
      )}
    </header>
  );
}
