import { Outlet } from "react-router-dom"

import { NavbarHeader } from "@/components/layout/NavbarHeader"
import { Footer } from "@/components/layout/Footer"

export function MainLayout() {
  return (
    <div className="min-h-screen bg-background text-foreground antialiased transition-colors duration-300 flex flex-col">
      
      <NavbarHeader />

      <main className="flex-1">
        <Outlet />
      </main>

      <Footer />
    </div>
  )
}