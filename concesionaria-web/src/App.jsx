import { NavbarHeader } from "@/components/layout/NavbarHeader"
import { Footer } from "@/components/layout/Footer"

function App() {
  return (
    <div className="min-h-screen bg-background text-foreground antialiased transition-colors duration-300">
      <NavbarHeader />

      <main className="flex-grow">
        {/* CONTENIDO DE LAS PÁGINAS */}
      </main>
      
      <Footer /> 
    </div>
  )
}

export default App