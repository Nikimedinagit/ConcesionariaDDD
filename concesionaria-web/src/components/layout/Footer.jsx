import { useState } from "react";
import { Link } from "react-router-dom";
import {
  FaLinkedin,
  FaInstagram,
  FaYoutube,
  FaChevronUp,
  FaChevronDown,
} from "react-icons/fa";
import { MdMail, MdMap, MdPhone } from "react-icons/md";

export function Footer() {
  const [isOpen, setIsOpen] = useState(false);

  return (
    <footer
      className="fixed bottom-0 left-0 w-full z-50 border-t border-white/5 transition-all duration-300"
      style={{
        backgroundColor: "hsl(var(--nav-bg))",
        color: "hsl(var(--nav-foreground))",
      }}
    >
      <div className="mx-auto max-w-[1400px] px-4 md:px-8">
        <div
          className="h-12 flex items-center justify-between cursor-pointer group"
          onClick={() => setIsOpen(!isOpen)}
        >
          <div className="flex items-center gap-3">
            <span className="text-sm font-black tracking-tight text-white">
              MPM Solutions
            </span>
            <span className="text-[13px] font-medium text-white/60 border-l border-white/10 pl-3 hidden sm:inline-block">
              © {new Date().getFullYear()} MPM Solutions. Todos los derechos reservados.
            </span>
          </div>

          <div className="flex items-center gap-2 text-[13px] font-bold tracking-widest text-white/90 group-hover:text-white transition-colors">
            {isOpen ? "Ocultar" : "Más información"}
            {isOpen ? (
              <FaChevronDown className="h-3 w-3" />
            ) : (
              <FaChevronUp className="h-3 w-3" />
            )}
          </div>
        </div>

        <div
          className={`overflow-y-auto transition-all duration-300 ease-out 
          ${isOpen ? "max-h-[60vh] opacity-100 border-t border-white/10" : "max-h-0 opacity-0"}`}
        >
          <div className="grid grid-cols-1 md:grid-cols-4 gap-8 py-6">
            
            <div className="space-y-3">
              <p className="text-sm text-white/60 leading-relaxed font-medium">
                Gestión integral para tu concesionaria. Operaciones, ventas y contabilidad centralizadas.
              </p>
              <div className="flex gap-4">
                <FaLinkedin className="h-5 w-5 text-white/50 hover:text-white cursor-pointer transition-colors" />
                <FaInstagram className="h-5 w-5 text-white/50 hover:text-white cursor-pointer transition-colors" />
                <FaYoutube className="h-5 w-5 text-white/50 hover:text-white cursor-pointer transition-colors" />
              </div>
            </div>

            <div className="space-y-3">
              <h3 className="text-[13px] font-bold tracking-wider text-white/90 uppercase">
                Ayuda y Soporte
              </h3>
              <ul className="text-sm text-white/60 space-y-2 font-medium">
                <li><Link to="/acerca" className="hover:text-white transition-colors">Acerca de MPM</Link></li>
                <li><Link to="/faq" className="hover:text-white transition-colors">Preguntas Frecuentes</Link></li>
                <li><Link to="/soporte" className="hover:text-white transition-colors">Centro de Soporte</Link></li>
              </ul>
            </div>

            <div className="space-y-3">
              <h3 className="text-[13px] font-bold tracking-wider text-white/90 uppercase">
                Contacto
              </h3>
              <ul className="text-sm text-white/60 space-y-2 font-medium">
                <li className="flex items-center gap-2">
                  <MdMap className="h-4 w-4 opacity-70" /> Morteros, Córdoba
                </li>
                <li className="flex items-center gap-2">
                  <MdPhone className="h-4 w-4 opacity-70" /> +54 (3562) 000-000
                </li>
                <li className="flex items-center gap-2">
                  <MdMail className="h-4 w-4 opacity-70" /> hola@mpm.com
                </li>
              </ul>
            </div>

            <div className="space-y-3">
              <h3 className="text-[13px] font-bold tracking-wider text-white/90 uppercase">
                Acceso Rápido
              </h3>
              <Link
                to="/login"
                className="block text-sm text-white/60 hover:text-white transition-colors font-medium"
              >
                Portal de Administración
              </Link>
            </div>
          </div>
        </div>
      </div>
    </footer>
  );
}