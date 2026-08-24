import api from "@/api/axios";

const TipoVehiculoService = {
  getActivas: async (filtro = "") => {
    const response = await api.get("/TiposVehiculos/activas", {
      params: { filtro },
    });
    return response.data;
  },

  getInactivas: async (filtro = "") => {
    const response = await api.get("/TiposVehiculos/inactivas", {
      params: { filtro },
    });
    return response.data;
  },

  crear: async (payload) => {
    const response = await api.post("/TiposVehiculos", payload);
    return response.data;
  },

  actualizar: async (id, payload) => {
    const response = await api.put(`/TiposVehiculos/${id}`, payload);
    return response.data;
  },

  desactivar: async (id) => {
    const response = await api.put(`/TiposVehiculos/desactivar/${id}`, {
      tipoVehiculoId: id,
      eliminado: true,
    });
    return response.data;
  },

  activar: async (id) => {
    const response = await api.put(`/TiposVehiculos/activar/${id}`, {
      tipoVehiculoId: id,
      eliminado: false,
    });
    return response.data;
  },
};

export default TipoVehiculoService;
