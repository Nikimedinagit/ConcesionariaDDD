import { z } from "zod";

const anioActual = new Date().getFullYear();

export const vehiculoSchema = z.object({
    modeloId: z.string().min(1, "Debés seleccionar un modelo."),

    version: z.string().trim().nonempty("Campo obligatorio.").min(3, "Mínimo 3 caracteres.").max(100, "Máximo 100 caracteres."),

    patente: z.string().trim().nonempty("Campo obligatorio.")
        .min(2, "Mínimo 2 caracteres.")
        .max(15, "Máximo 15 caracteres.")
        .regex(
            /^[A-Za-z0-9 -]+$/,
            "La patente solo puede contener letras, números, espacios y guiones."
        ),

    color: z.string().trim().nonempty("Campo obligatorio.").min(3, "Mínimo 3 caracteres.").max(50, "Máximo 50 caracteres."),

    anio: z.coerce.number({ error: "Ingresá un año válido.", })
        .int("El año debe ser un número entero.")
        .min(1900, "El año mínimo es 1900.")
        .max(anioActual, `El año máximo es ${anioActual}.`),

    kilometraje: z
        .string({
            error: "Campo obligatorio.",
        })
        .trim()
        .nonempty("Campo obligatorio.")
        .refine(
            (value) => !Number.isNaN(Number(value)),
            "Ingresá un kilometraje válido.",
        )
        .transform((value) => Number(value))
        .pipe(
            z
                .number()
                .int("El kilometraje debe ser un número entero.")
                .min(0, "El kilometraje no puede ser negativo."),
        ),

    condicion: z.coerce
        .number({
            error: "Debés seleccionar una condición.",
        })
        .int()
        .positive("Debés seleccionar una condición."),

    precioCompra: z.coerce
        .number({
            error: "Ingresá un precio de compra válido.",
        })
        .positive("El precio de compra debe ser mayor a cero."),

    precioVenta: z.coerce
        .number({
            error: "Ingresá un precio de venta válido.",
        })
        .positive("El precio de venta debe ser mayor a cero."),
})
    .superRefine((vehiculo, context) => {
        const condicionNuevo = 1;
        const condicionUsado = 2;

        if (
            vehiculo.condicion === condicionNuevo &&
            vehiculo.kilometraje !== 0
        ) {
            context.addIssue({
                code: "custom",
                path: ["kilometraje"],
                message: "El kilometraje de un vehículo nuevo debe ser 0.",
            });
        }

        if (
            vehiculo.condicion === condicionUsado &&
            vehiculo.kilometraje <= 0
        ) {
            context.addIssue({
                code: "custom",
                path: ["kilometraje"],
                message: "El kilometraje de un vehículo usado debe ser mayor a cero.",
            });
        }

    });

