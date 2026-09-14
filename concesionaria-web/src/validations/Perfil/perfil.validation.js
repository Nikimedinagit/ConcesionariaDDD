import { z } from "zod";

export const passwordSchema = z.object({
  passwordActual: z.string().min(1, "Contraseña incorrecta."),
  passwordNueva: z.string().min(6, "Contraseña mínimo 6 caracteres."),
});

export const nombreFantasiaSchema = z.object({
  nombreFantasia: z
    .string()
    .min(3, "Nombre Fantasía debe tener al menos 3 caracteres.")
    .max(100, "Nombre Fantasía debe tener como máximo 100 caracteres."),
});

export const nombreCompletoSchema = z.object({
  nombreCompleto: z.string()
    .min(3, "Nombre Completo debe tener al menos 3 caracteres.")
    .max(50, "Nombre Completo debe tener como máximo 50 caracteres."),
});

export const telefonoSchema = z.object({
  codigoPais: z
    .string()
    .min(1, "Seleccioná un código de país."),

  codigoArea: z
    .string()
    .min(2, "Código de área inválido.")
    .max(4, "Máximo 4 números."),

  telefono: z
    .string()
    .min(6, "Teléfono inválido.")
    .max(10, "Máximo 10 números."),
});
