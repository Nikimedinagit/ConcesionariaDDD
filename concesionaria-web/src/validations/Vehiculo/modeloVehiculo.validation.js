import { z } from "zod";

export const modeloVehiculoSchema = z.object({
  nombre: z.string().trim().nonempty("Campo obligatorio.").min(2, "Mínimo 2 caracteres.").max(50, "Máximo 50 caracteres."),
  marcaVehiculoId: z.string().min(1, "Seleccione una marca."),
  tipoVehiculoId: z.string().min(1, "Seleccione un tipo de vehículo."),
});
