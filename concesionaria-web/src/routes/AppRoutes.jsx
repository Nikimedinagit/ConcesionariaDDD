import { BrowserRouter, Routes, Route } from "react-router-dom"
import { ProtectedRoute } from "@/routes/ProtectedRoute" 

import { MainLayout } from "@/layouts/MainLayout"
import { AuthLayout } from "@/layouts/AuthLayout"

import LoginPage from "@/pages/Auth/LoginPage"
import RegisterPage from "@/pages/Auth/RegisterPage"
import RecoverAccessPage from "@/pages/Auth/RecoverAccessPage"
import { PerfilPage } from "@/pages/Profile/PerfilPage";
import { UsuarioPage } from "@/pages/Acceso/UsuarioPage";

import { CategoriaGastoPage } from "@/pages/Tesoseria/CategoriaGastoPage";
import { CuentaPage } from "@/pages/Contabilidad/CuentaPage";
import { LocalidadPage } from "@/pages/Ubicacion/LocalidadPage";
import { ProvinciaPage } from "@/pages/Ubicacion/ProvinciaPage";
import { SucursalPage } from "@/pages/Ubicacion/SucursalPage";

export default function AppRoutes() {
  return (
    <BrowserRouter>
      <Routes>

        {/* AUTH (Público) */}
        <Route element={<AuthLayout />}>
          <Route path="/" element={<LoginPage />} />
          <Route path="/register" element={<RegisterPage />} />
          <Route path="/recuperar-acceso" element={<RecoverAccessPage />} />
        </Route>

        {/* SISTEMA (Protegido) */}
        <Route element={<ProtectedRoute />}>
          <Route path="/layout" element={<MainLayout />}>
  
            <Route path="perfil" element={<PerfilPage />} />
            <Route path="usuarios" element={<UsuarioPage />} />

            <Route path="categorias-gastos" element={<CategoriaGastoPage />} />
            <Route path="cuentas" element={<CuentaPage />} />
            <Route path="provincias" element={<ProvinciaPage />} />
            <Route path="localidades" element={<LocalidadPage />} />
            <Route path="sucursales" element={<SucursalPage />} />
          </Route>
        </Route>

      </Routes>
    </BrowserRouter>
  )
}
