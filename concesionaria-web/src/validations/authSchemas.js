import { z } from "zod";

export const empresaSchema = z.object({
  razonSocial: z.string().min(3, "Razón social debe tener al menos 3 caracteres."),
  nombreFantasia: z.string().min(3, "Nombre fantasía debe tener al menos 3 caracteres."),
  cuit: z
    .string()
    .refine((value) => /^\d{11}$/.test(value.replace(/\D/g, "")), {
      message: "CUIT debe tener 11 dígitos.",
    }),
  localidadId: z.string().min(1, "Debés seleccionar una localidad."),
  moneda: z.string().min(1, "Debés seleccionar una moneda."),
});

export const usuarioSchema = z.object({
  nombre: z.string().min(3, "Nombre completo debe tener al menos 3 caracteres."),
  email: z.string().email("Email debe contener @ y ."),
  password: z
    .string()
    .min(6, "Contraseña mínimo 6 caracteres.")});

export const loginSchema = z.object({
  email: z.string().email(),
  password: z.string().min(6),
});

export const recuperarAccesoSchema = z.object({
  contact: z.string().min(1, "El campo es obligatorio.")
    .refine((val) => {
      const isEmail = /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(val);
      const isPhone = /^\+?\d{10,15}$/.test(val.replace(/\s/g, ''));
      return isEmail || isPhone;
    }, { 
      message: "Ingresá un email (con @ y .) o un número (ej: +543562123456)." 
    }),
});


