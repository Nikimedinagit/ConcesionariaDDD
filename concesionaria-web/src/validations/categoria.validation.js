import { z } from "zod";

export const categoriaSchema = z.object({
  nombre: z
    .string()
    .trim()
    .nonempty("Campo obligatorio.")
    .min(3, "Mínimo 3 caracteres.")
    .max(50, "Máximo 50 caracteres."),
});