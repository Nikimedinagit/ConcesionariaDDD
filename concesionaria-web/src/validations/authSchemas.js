import { z } from "zod";

export const empresaSchema = z.object({
  razonSocial: z.string().min(3),
  nombreFantasia: z.string().min(3),
  cuit: z.string().min(11),
  localidadId: z.string().min(1),
  moneda: z.string().min(1),
});

export const usuarioSchema = z.object({
  nombre: z.string().min(3),
  email: z.string().email(),
  password: z.string().min(6),
});