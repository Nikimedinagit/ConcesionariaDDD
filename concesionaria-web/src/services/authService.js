import api from "@/api/axios";



export const loginRequest = async (data) => {
  const response = await api.post("/auth/login", data);
  return response.data;
};

export const registerRequest = async (data) => {
  const response = await api.post("/auth/register", data);
  return response.data;
};

export const solicitarCodigo = async (data) => {
  const response = await api.post("/auth/solicitar-codigo", data);
  return response.data;
};

export const validarCodigo = async (data) => {
  const response = await api.post("/auth/validar-codigo", data);
  return response.data;
};

export const cambiarPassword = async (data) => {
  const payload = {
    Contacto: data.contacto,
    Token: data.token,
    NuevaPassword: data.password
  };
  const response = await api.post("/auth/cambiar-password", payload);
  return response.data;
}

