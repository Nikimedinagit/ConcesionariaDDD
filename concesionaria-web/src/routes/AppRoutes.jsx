import { BrowserRouter, Routes, Route } from "react-router-dom";
import { ProtectedRoute } from "@/routes/ProtectedRoute";

import { MainLayout } from "@/layouts/MainLayout";
import { AuthLayout } from "@/layouts/AuthLayout";

import LoginPage from "@/pages/Auth/LoginPage";
import RegisterPage from "@/pages/Auth/RegisterPage";
import RecoverAccessPage from "@/pages/Auth/RecoverAccessPage";
import { PerfilPage } from "@/pages/Profile/PerfilPage";
import { UsuarioPage } from "@/pages/Acceso/UsuarioPage";

import { CategoriaGastoPage } from "@/pages/Tesoseria/CategoriaGastoPage";
import { CuentaPage } from "@/pages/Contabilidad/CuentaPage";
import { LocalidadPage } from "@/pages/Ubicacion/LocalidadPage";
import { ProvinciaPage } from "@/pages/Ubicacion/ProvinciaPage";
import { SucursalPage } from "@/pages/Ubicacion/SucursalPage";
import { MarcaPage } from "@/pages/Vehiculos/MarcaPage";
import { TipoVehiculoPage } from "@/pages/Vehiculos/TipoVehiculoPage";
import { ModeloVehiculoPage } from "@/pages/Vehiculos/ModeloVehiculoPage";
import { ClientePage } from "@/pages/Personas/ClientePage";
import { ProveedorPage } from "@/pages/Personas/ProveedorPage";
import { ProveedorDetallePage } from "@/pages/Personas/ProveedorDetallePage";
import { ClienteDetallePage } from "@/pages/Personas/ClienteDetallePage";
import { PermisosPage } from "@/pages/Acceso/PermisosPage";
import { VehiculoPage } from "@/pages/Vehiculos/VehiculoPage";
import { VehiculoDetallePage } from "@/pages/Vehiculos/VehiculoDetallePage";

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
            <Route path="permisos" element={<PermisosPage />} />

            <Route path="categorias-gastos" element={<CategoriaGastoPage />} />
            <Route path="cuentas" element={<CuentaPage />} />
            <Route path="provincias" element={<ProvinciaPage />} />
            <Route path="localidades" element={<LocalidadPage />} />
            <Route path="sucursales" element={<SucursalPage />} />
            <Route path="marcas" element={<MarcaPage />} />
            <Route path="modelos" element={<ModeloVehiculoPage />} />
            <Route path="tipos-vehiculos" element={<TipoVehiculoPage />} />
            <Route path="clientes" element={<ClientePage />} />
            <Route path="clientes/:id" element={<ClienteDetallePage />} />
            <Route path="proveedores" element={<ProveedorPage />} />
            <Route path="proveedores/:id" element={<ProveedorDetallePage />} />
            <Route path="vehiculos" element={<VehiculoPage />} />
            <Route path="vehiculos/:id" element={<VehiculoDetallePage />} />
          </Route>
        </Route>
      </Routes>
    </BrowserRouter>
  );
}
