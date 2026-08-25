import { useState } from "react"
import { Link, useNavigate } from "react-router-dom"
import { ArrowRight, LockKeyhole, Mail } from "lucide-react"
import { Button } from "@/components/ui/button"
import { AppInput } from "@/components/ui/custom/AppInput"
import { loginRequest } from "@/services/Auth/authService"
import { useAuth } from "@/context/AuthContext" 

export function LoginForm() {
  const navigate = useNavigate()
  const { updateUserData } = useAuth() 
  const [email, setEmail] = useState("")
  const [password, setPassword] = useState("")
  const [error, setError] = useState("")
  const [isSubmitting, setIsSubmitting] = useState(false)

  const emailError = email.length > 0 && (!email.includes("@") || !email.includes("."));
  const passwordError = password.length > 0 && password.length < 6;
  const loginValid = email.length > 0 && password.length >= 6 && !emailError && !passwordError;

  async function handleSubmit(event) {
    event.preventDefault()
    setError("")

    if (!loginValid) {
      setError("Completa con un email y contraseña válidos.")
      return
    }

    setIsSubmitting(true)

    try {
      const response = await loginRequest({ email, password });
      const token = response.token;

      if (!token) {
        setError("No se recibió el token.");
        return;
      }

      localStorage.setItem("token", token);
      
      updateUserData(); 
      
      navigate("/layout");
    } catch (err) {
      console.error(err);
      const message = err?.response?.data?.message ?? err?.message ?? "Error al iniciar sesión.";
      setError(message);
    } finally {
      setIsSubmitting(false);
    }
  }

  return (
    <div className="rounded-[2rem] border border-slate-200 bg-white p-6 sm:p-7 shadow-sm w-full max-w-[1080px] mx-auto">
      <div className="mb-5">
        <p className="text-sm uppercase tracking-[0.24em] text-slate-500">
          Acceso
        </p>

        <h2 className="mt-4 text-3xl font-black tracking-tight text-slate-900 sm:text-4xl">
          Bienvenidos
        </h2>

        <p className="mt-3 text-sm leading-relaxed text-slate-500 sm:text-base">
          Accede a tu espacio de gestión.
        </p>
      </div>

      {error ? (
        <div className="mb-4 rounded-2xl border border-rose-200 bg-rose-50 px-4 py-3 text-sm text-rose-700">
          {error}
        </div>
      ) : null}

      <form className="space-y-4" onSubmit={handleSubmit}>

        <div className="space-y-1">
          <AppInput
            label="Email"
            icon={Mail}
            type="email"
            value={email}
            onChange={(event) => setEmail(event.target.value.toLowerCase())}
            placeholder="tuemail@empresa.com"
            className="[&>label]:font-medium [&>label]:text-slate-700"
          />
          {emailError && <p className="text-sm font-mediu text-rose-500">El email debe contener @ y .</p>}
        </div>

        <div className="space-y-1">
          <AppInput
            label="Contraseña"
            icon={LockKeyhole}
            type="password"
            value={password}
            onChange={(event) => setPassword(event.target.value)}
            placeholder="••••••••"
            className="[&>label]:font-medium [&>label]:text-slate-700"
          />
          {passwordError && <p className="text-sm font-medium text-rose-500">Mínimo 6 caracteres.</p>}
        </div>

        <Button
          type="submit"
          disabled={isSubmitting}
          className="w-full h-[44px] rounded-2xl font-semibold text-[14px] mt-1 text-white cursor-pointer hover:shadow-lg transition-shadow disabled:cursor-not-allowed disabled:opacity-50"
          style={{ background: "hsl(var(--nav-bg))" }}
        >
          {isSubmitting ? "Ingresando..." : "Ingresar"}
          <ArrowRight className="ml-2 w-4 h-4" />
        </Button>

        <div className="flex flex-row flex-wrap items-center justify-between gap-3 pt-2 text-sm">
          <Link
            to="/recuperar-acceso"
            className="text-slate-500 hover:text-slate-900 transition-colors underline-offset-4 hover:underline"
          >
            Recuperar acceso
          </Link>

          <Link
            to="/register"
            className="font-semibold text-slate-900 hover:text-slate-700 transition-colors underline-offset-4 hover:underline"
          >
            Crear cuenta
          </Link>
        </div>
      </form>
    </div>
  )
}
