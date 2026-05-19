import { Suspense} from "react"
import { Routes, Route } from "react-router-dom"
import { NavbarHeader } from "@/components/NavbarHeader"

function App() {
  return (
    <div className="min-h-screen bg-background text-foreground antialiased transition-colors duration-300">
      
      {/* El Navbar queda fijo y no se vuelve a renderizar innecesariamente */}
      <NavbarHeader />
      
      {/* Contenedor de las pantallas con un esqueleto de carga intermedio */}
      <main className="container mx-auto max-w-7xl p-6">
        <Suspense fallback={<div className="text-center py-10 text-muted-foreground">Cargando módulo...</div>}>
          <Routes>
            <Route path="/" element={
              <div className="p-12 text-center border-2 border-dashed rounded-2xl">
                <h2 className="text-2xl font-bold tracking-tight">AutoGest Pro 🧉</h2>
                <p className="text-muted-foreground mt-2">Seleccioná un módulo del menú para operar.</p>
              </div>
            } />
            
            {/* Rutas con URL reales eficientes */}
          </Routes>
        </Suspense>
      </main>

    </div>
  )
}

export default App