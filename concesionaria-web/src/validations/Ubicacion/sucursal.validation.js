import { z } from "zod";

export const sucursalSchema = z.object({
  nombre: z
    .string()
    .trim()
    .nonempty("Campo obligatorio.")
    .min(3, "Mínimo 3 caracteres.")
    .max(50, "Máximo 50 caracteres."),
  direccion: z
    .string()
    .trim()
    .nonempty("Campo obligatorio.")
    .min(3, "Mínimo 3 caracteres.")
    .max(100, "Máximo 100 caracteres."),
  localidadId: z.string().min(1, "Debés seleccionar una localidad."),
});
