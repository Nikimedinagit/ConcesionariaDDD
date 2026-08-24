import { z } from "zod";

export const proveedorSchema = z.object({
    nombre: z.string().trim().nonempty("Campo obligatorio.").min(3, "Mínimo 3 caracteres.").max(100, "Máximo 100 caracteres."),
    cuil: z.string().trim().regex(/^\d{11}$/, "El CUIL debe tener 11 dígitos."),
    telefono: z.string().trim().nonempty("Campo obligatorio.").max(30, "Máximo 30 caracteres."),
    email: z.string().trim().email("Ingrese un email válido.").max(100, "Máximo 100 caracteres."),
    domicilio: z.string().trim().nonempty("Campo obligatorio.").min(3, "Mínimo 3 caracteres.").max(150, "Máximo 150 caracteres."),
    servicio: z.string().trim().nonempty("Campo obligatorio.").min(3, "Mínimo 3 caracteres.").max(150, "Máximo 150 caracteres."),
    observacion: z.string().max(500, "Máximo 500 caracteres."),
    localidadId: z.string().min(1, "Debés seleccionar una localidad."),
})
