import api from "@/api/axios";

const CuentaService = {
  getActivas: async () => {
    const response = await api.get("/Cuentas/activas", {
      params: { filtro: "" },
    });
    return response.data;
  },

  getInactivas: async () => {
    const response = await api.get("/Cuentas/inactivas", {
      params: { filtro: "" },
    });
    return response.data;
  },

  crear: async (payload) => {
    const response = await api.post("/Cuentas", payload);
    return response.data;
  },

  actualizar: async (id, payload) => {
    const response = await api.put(`/Cuentas/${id}`, payload);
    return response.data;
  },

  desactivar: async (id) => {
    const response = await api.put(`/Cuentas/desactivar/${id}`, {
      cuentaId: id,
    });
    return response.data;
  },

  activar: async (id) => {
    const response = await api.put(`/Cuentas/activar/${id}`, {
      cuentaId: id,
    });
    return response.data;
  },
};

export default CuentaService;
