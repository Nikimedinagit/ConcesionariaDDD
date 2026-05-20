import api from "@/api/axios";

export const getLocalidades = async () => {
  const response = await api.get("/localidades");
  return response.data;
};