import api from "@/api/axios";

const SucursalService = {
  getActivas: async (filtro = "") => {
    const response = await api.get("/Sucursales/activas", {
      params: { filtro },
    });
    return response.data;
  },

  getInactivas: async (filtro = "") => {
    const response = await api.get("/Sucursales/inactivas", {
      params: { filtro },
    });
    return response.data;
  },

  crear: async (payload) => {
    const response = await api.post("/Sucursales", payload);
    return response.data;
  },

  actualizar: async (id, payload) => {
    const response = await api.put(`/Sucursales/${id}`, payload);
    return response.data;
  },

  desactivar: async (id) => {
    const response = await api.put(`/Sucursales/desactivar/${id}`, {
      sucursalId: id,
    });
    return response.data;
  },

  activar: async (id) => {
    const response = await api.put(`/Sucursales/activar/${id}`, {
      sucursalId: id,
    });
    return response.data;
  },
};

export default SucursalService;
