import { BrowserRouter, Routes, Route } from "react-router-dom"
import { ProtectedRoute } from "@/routes/ProtectedRoute" 

import { MainLayout } from "@/layouts/MainLayout"
import { AuthLayout } from "@/layouts/AuthLayout"

import LoginPage from "@/pages/Auth/LoginPage"
import RegisterPage from "@/pages/Auth/RegisterPage"
import RecoverAccessPage from "@/pages/Auth/RecoverAccessPage"
import { PerfilPage } from "@/pages/Profile/PerfilPage";

import { CategoriaGastoPage } from "@/pages/Tesoseria/CategoriaGastoPage";

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

            <Route path="categorias-gastos" element={<CategoriaGastoPage />} />
          </Route>
        </Route>

      </Routes>
    </BrowserRouter>
  )
}
