import api from "@/api/axios";

export const getLocalidades = async () => {
  const response = await api.get("/ubicaciones/localidades");
  return response.data;
};