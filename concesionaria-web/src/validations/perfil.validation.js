import { z } from "zod";

export const passwordSchema = z.object({
  passwordActual: z.string().min(1, "Contraseña incorrecta."),
  passwordNueva: z.string().min(6, "Contraseña mínimo 6 caracteres."),
});

export const nombreFantasiaSchema = z.object({
  nombreFantasia: z.string().min(1, "Nombre Fantasía no puede estar vacío."),
});

export const nombreCompletoSchema = z.object({
  nombreCompleto: z.string().min(1, "Nombre Completo no puede estar vacío."),
});

export const telefonoSchema = z.object({
  telefono: z.string().min(1, "Teléfono no puede estar vacío.")
    .refine((val) => /^\+\d{1,3}\s\d{2,4}\s\d{6,10}$/.test(val), {
      message: "Formato inválido. Use: +54 3562 123456",
    }),
});