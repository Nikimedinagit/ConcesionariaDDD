import {
  BrowserRouter,
  Routes,
  Route,
} from "react-router-dom"

import { MainLayout } from "@/layouts/MainLayout"
import { AuthLayout } from "@/layouts/AuthLayout"

import LoginPage from "@/pages/Auth/LoginPage"
import RegisterPage from "@/pages/Auth/RegisterPage"
import RecoverAccessPage from "@/pages/Auth/RecoverAccessPage"

export default function AppRoutes() {
  return (
    <BrowserRouter>
      <Routes>

        {/* AUTH */}
        <Route element={<AuthLayout />}>
          <Route path="/" element={<LoginPage />} />
          <Route path="/register" element={<RegisterPage />} />
          <Route path="/recuperar-acceso" element={<RecoverAccessPage />} />
        </Route>

        {/* SISTEMA */}
        <Route path="/layout" element={<MainLayout />}>
          
          {/* DASHBOARD */}
          {/* <Route path="/dashboard" element={<Dashboard />} /> */}

        </Route>

      </Routes>
    </BrowserRouter>
  )
}