import api from "@/api/axios";

const MarcaService = {
  getActivas: async (filtro = "") => {
    const response = await api.get("/MarcasVehiculos/activas", {
      params: { filtro },
    });
    return response.data;
  },

  getInactivas: async (filtro = "") => {
    const response = await api.get("/MarcasVehiculos/inactivas", {
      params: { filtro },
    });
    return response.data;
  },

  crear: async (payload) => {
    const response = await api.post("/MarcasVehiculos", payload);
    return response.data;
  },

  actualizar: async (id, payload) => {
    const response = await api.put(`/MarcasVehiculos/${id}`, payload);
    return response.data;
  },

  desactivar: async (id) => {
    const response = await api.put(`/MarcasVehiculos/desactivar/${id}`, {
      marcaVehiculoId: id,
      eliminado: true,
    });
    return response.data;
  },

  activar: async (id) => {
    const response = await api.put(`/MarcasVehiculos/activar/${id}`, {
      marcaVehiculoId: id,
      eliminado: false,
    });
    return response.data;
  },
};

export default MarcaService;
