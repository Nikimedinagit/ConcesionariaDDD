import { z } from "zod";

export const cuentaSchema = z.object({
  nombre: z
    .string()
    .trim()
    .nonempty("Campo obligatorio.")
    .min(3, "Mínimo 3 caracteres.")
    .max(50, "Máximo 50 caracteres."),
});

export const cuentaRaizSchema = cuentaSchema.extend({
  tipo: z.string().min(1, "Debés seleccionar un tipo de cuenta."),
});
