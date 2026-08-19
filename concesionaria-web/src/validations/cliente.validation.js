import { z } from "zod";

export const clienteSchema = z.object({
  nombreCompleto: z.string().trim().nonempty("Campo obligatorio.").min(3, "Mínimo 3 caracteres.").max(100, "Máximo 100 caracteres."),
  dni: z.string().trim().regex(/^\d{8}$/, "El DNI debe tener 8 dígitos."),
  telefono: z.string().trim().nonempty("Campo obligatorio.").max(30, "Máximo 30 caracteres."),
  email: z.string().trim().email("Ingrese un email válido.").max(100, "Máximo 100 caracteres."),
  domicilio: z.string().trim().nonempty("Campo obligatorio.").min(3, "Mínimo 3 caracteres.").max(150, "Máximo 150 caracteres."),
  localidadId: z.string().min(1, "Debés seleccionar una localidad."),
});
