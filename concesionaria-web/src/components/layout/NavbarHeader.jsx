import { useState } from "react";
import { Link } from "react-router-dom";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { ChevronDown, Menu, X } from "lucide-react";
import { UserDropdown } from "@/components/auth/UserDropdown";

export function NavbarHeader() {
  const [isMobileMenuOpen, setIsMobileMenuOpen] = useState(false);
  const [openGroup, setOpenGroup] = useState(null);

  

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
        { to: "/layout/categorias-gastos", name: "Categorías de Gastos" },
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
          <UserDropdown />

          <button
            onClick={() => setIsMobileMenuOpen(!isMobileMenuOpen)}
            className="flex lg:hidden items-center justify-center h-10 w-10 rounded-lg text-white hover:bg-white/10 transition-colors cursor-pointer outline-none"
          >
            {isMobileMenuOpen ? (
              <X className="h-6 w-6" />
            ) : (
              <Menu className="h-6 w-6" />
            )}
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
                className="w-full flex items-center justify-between p-3 text-sm font-bold text-white/70 tracking-wider hover:bg-white/5 transition-colors"
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
