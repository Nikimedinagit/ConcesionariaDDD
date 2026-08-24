import { z } from "zod";

export const usuarioSchema = z.object({
  nombreCompleto: z
    .string()
    .trim()
    .nonempty("Campo obligatorio.")
    .min(3, "Mínimo 3 caracteres.")
    .max(100, "Máximo 100 caracteres."),
  email: z
    .string()
    .trim()
    .nonempty("Campo obligatorio.")
    .email("Email no válido.")
    .max(150, "Máximo 150 caracteres."),
  rolId: z.string().min(1, "Debés seleccionar un rol."),
  sucursalId: z.string().min(1, "Debés seleccionar una sucursal."),
});

export const usuarioCreateSchema = usuarioSchema.extend({
  password: z
    .string()
    .nonempty("Campo obligatorio.")
    .min(6, "Mínimo 6 caracteres."),
});

export const usuarioPasswordSchema = z.object({
  password: z
    .string()
    .nonempty("Campo obligatorio.")
    .min(6, "Mínimo 6 caracteres."),
});
