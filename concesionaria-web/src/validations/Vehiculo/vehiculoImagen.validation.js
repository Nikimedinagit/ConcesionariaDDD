import { z } from "zod";

export const MAX_VEHICULO_IMAGES = 10;
export const MAX_VEHICULO_IMAGE_SIZE = 5 * 1024 * 1024;
export const VEHICULO_IMAGE_TYPES = [
  "image/jpeg",
  "image/png",
  "image/webp",
];

export const vehiculoImageFileSchema = z
  .instanceof(File, { error: "La imagen seleccionada no es válida." })
  .refine(
    (file) => VEHICULO_IMAGE_TYPES.includes(file.type),
    "Solo se permiten imágenes JPG, PNG o WebP.",
  )
  .refine((file) => file.size > 0, "La imagen seleccionada está vacía.")
  .refine(
    (file) => file.size <= MAX_VEHICULO_IMAGE_SIZE,
    "Cada imagen puede pesar como máximo 5 MB.",
  );

export const vehiculoImagesSchema = z
  .array(
    z.object({
      id: z.string().min(1),
      file: vehiculoImageFileSchema.nullable(),
      preview: z.string().min(1),
      isPrincipal: z.boolean(),
      isExisting: z.boolean().optional(),
    }),
  )
  .max(
    MAX_VEHICULO_IMAGES,
    `Podés cargar hasta ${MAX_VEHICULO_IMAGES} imágenes.`,
  )
  .superRefine((images, context) => {
    const fileKeys = images
      .filter((image) => image.file)
      .map(
        (image) =>
          `${image.file.name}-${image.file.size}-${image.file.lastModified}`,
      );

    if (new Set(fileKeys).size !== fileKeys.length) {
      context.addIssue({
        code: "custom",
        message: "No podés agregar imágenes duplicadas.",
      });
    }

    if (
      images.length > 0 &&
      images.filter((image) => image.isPrincipal).length !== 1
    ) {
      context.addIssue({
        code: "custom",
        message: "Debe existir una única imagen principal.",
      });
    }
  });
