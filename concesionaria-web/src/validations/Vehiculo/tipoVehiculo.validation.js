import { z } from "zod";

export const tipoVehiculoSchema = z.object({
  nombre: z
    .string()
    .trim()
    .nonempty("Campo obligatorio.")
    .min(2, "Mínimo 2 caracteres.")
    .max(50, "Máximo 50 caracteres."),
});
